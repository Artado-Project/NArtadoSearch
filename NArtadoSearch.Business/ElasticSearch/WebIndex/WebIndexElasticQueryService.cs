using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using NArtadoSearch.Core.DataAccess.ElasticSearch.Abstractions;
using NArtadoSearch.Core.Entities;
using NArtadoSearch.Entities.Concrete;

namespace NArtadoSearch.Business.ElasticSearch.WebIndex;

public class WebIndexElasticQueryService(ElasticsearchClient client) : IQueryService<IndexedWebUrl>
{
    private readonly ElasticsearchClient _client = client;

    private string _indexName =
        $"artado-search-{CultureInfo.GetCultureInfo("en-US").TextInfo.ToLower(nameof(IndexedWebUrl))}";

    public Task<IndexedWebUrl> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IndexedWebUrl>> SearchAsync(string query, int page = 1, int pageSize = 20)
    {
        var index = await _client.Indices.GetAsync<IndexedWebUrl>(_indexName);
        if (!index.IsValidResponse)
        {
            await _client.Indices.CreateAsync<IndexedWebUrl>(_indexName);
        }

        string[] fields = { "url", "title", "keywords", "description", "articlesContent" };
        List<Action<FunctionScoreDescriptor<IndexedWebUrl>>> functionScores =
            new List<Action<FunctionScoreDescriptor<IndexedWebUrl>>>();
        var queryWords = query.Split(' ');
        var importantWords = new Regex(@"\*(.*?)\*").Matches(query).Select(x => x.Groups[1].Value).ToList();

        var shouldQueries = new List<Query>();
        var importantQueries = new List<Query>();

        if (queryWords.Length == 1)
        {
            var regexp = new RegexpQuery(new Field("url.keyword"))
            {
                Field = "url.keyword",
                Value = @"https://(www\.)?" + queryWords[0] + "\\.(com|org|net)/",
                Boost = 1000
            };

            shouldQueries.Add(regexp);
        }

        foreach (var field in fields)
        {
            foreach (var importantWord in importantWords)
            {
                var termQuery = new TermQuery(field)
                {
                    Value = importantWord,
                    Boost = 800
                };
                importantQueries.Add(termQuery);
            }
        }

        var importantBoolQuery = new BoolQuery()
        {
            Should = importantQueries
        };

        foreach (var field in fields)
        {
            var matchQuery = new MatchQuery(field)
            {
                Query = query,
                Boost = GetBoost(field),
                Fuzziness = new Fuzziness(0)
            };

            shouldQueries.Add(matchQuery);
        }

        int from = (page - 1) * pageSize;

        var response = await _client.SearchAsync<IndexedWebUrl>(q =>
            q.Index(_indexName).Query(s => s
                .Bool(bq => bq.Should(
                        shouldQueries.ToArray()
                    )
                    .Must(
                        new Query[] { importantBoolQuery }
                    ))
            ).From(from).Size(pageSize));

        if (!response.IsSuccess()) return new List<IndexedWebUrl>().ToIndexedUrlCollection(0);
        var collection = new IndexedUrlCollection();
        return response.Documents.ToIndexedUrlCollection(response.Total);
    }

    public async Task PurgeAsync()
    {
        await _client.Indices.FlushAsync<IndexedWebUrl>(_indexName);
    }

    public async Task AddAsync(IndexedWebUrl entity)
    {
        var index = await _client.Indices.GetAsync<IndexedWebUrl>(_indexName);
        if (!index.IsValidResponse)
        {
            await _client.Indices.CreateAsync<IndexedWebUrl>(_indexName);
        }

        await _client.IndexAsync(entity, i => i.Index(_indexName));
    }

    private int GetBoost(string field)
    {
        switch (field)
        {
            case "url":
                return 40;
            case "title":
                return 35;
            case "keywords":
                return 20;
            case "description":
                return 10;
            case "articlesContent":
                return 6;
        }

        return 0;
    }

    public async Task UpdateAsync(IndexedWebUrl entity)
    {
        var index = await _client.Indices.GetAsync<IndexedWebUrl>(_indexName);
        if (!index.IsValidResponse)
        {
            return;
        }

        await _client.UpdateAsync<IndexedWebUrl, IndexedWebUrl>(entity.Id, i => i.Index(_indexName).Doc(entity));
    }

    private string GetFirstUrlFromQuery(string query)
    {
        string pattern = @"\b(?:https?://)?(?:www\.)?[a-z0-9._%+-]+\.(?:com|net|org|edu|gov|info)\b";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matches = regex.Matches(query);
        foreach (Match match in matches)
        {
            return match.Value;
        }

        return "";
    }

    private string RemoveDiacritics(string text)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding srcEncoding = Encoding.UTF8;
        Encoding destEncoding = Encoding.GetEncoding(1252); // Latin alphabet

        text = destEncoding.GetString(Encoding.Convert(srcEncoding, destEncoding, srcEncoding.GetBytes(text)));

        string normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < normalizedString.Length; i++)
        {
            if (!CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]).Equals(UnicodeCategory.NonSpacingMark))
            {
                result.Append(normalizedString[i]);
            }
        }

        return result.ToString();
    }
}