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

    private string _indexName = $"artado-search-{CultureInfo.GetCultureInfo("en-US").TextInfo.ToLower(nameof(IndexedWebUrl))}";

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
        List<Action<FunctionScoreDescriptor<IndexedWebUrl>>> functionScores = new List<Action<FunctionScoreDescriptor<IndexedWebUrl>>>();
        var queryWords = query.Split(' ');

        if (queryWords.Length > 1)
        {
            /*foreach (var field in fields)
            {
                functionScores.Add(t =>
                {
                    var boolQuery = new BoolQuery();
                    boolQuery.Should = new List<Query>();
                    foreach (var word in queryWords)
                    {
                        boolQuery.Should.Add(new MatchQuery(field)
                        {
                            Query = field == "url" ? RemoveDiacritics(word.ToLower()) : word.ToLower()
                        });
                    }

                    t.Filter(boolQuery).Weight(60);
                });
            }*/

            functionScores.Add(t =>
            {
                t.ScriptScore(ss =>
                {
                    ss.Script(script =>
                    {
                        script.Source(
                            @"
            double score = 0;
            for (String field : params.fields) {
              String fieldKeyword = field + "".keyword"";
              if (doc[fieldKeyword].size() > 0) {
                String fieldValue = doc[fieldKeyword].value.toLowerCase();
                double hIndex = 5;
                for (String query_term : params.query_terms) {
                  if (fieldValue.contains("" "" + query_term.toLowerCase()) || fieldValue.contains(query_term.toLowerCase() + "" "") || fieldValue.contains(query_term.toLowerCase() + "","") || fieldValue.contains("","" + query_term.toLowerCase())) {
                    score += hIndex;
                  }
                hIndex -= 1;
                }
              }
            }
            return score;
            "
                        ).Params(dict => dict.Add("fields", fields).Add("query_terms", queryWords));
                    });
                }).Weight(1000.0);
            });
        }

        if (queryWords.Length == 1)
        {
            functionScores.Add(t =>
            {
                var regexp = new RegexpQuery(new Field("url.keyword"))
                {
                    Field = "url.keyword",
                    Value = @"https://(www\.)?" + queryWords[0] + "\\.(com|org|net)/"
                };

                t.Filter(regexp).Weight(100.0);
            });
        }

        var response = await _client.SearchAsync<IndexedWebUrl>(q =>
            q.Index(_indexName).Query(s => s.FunctionScore(r =>
                    r.Query(
                        p => p.MultiMatch(qq =>
                            qq.Query(query).Type(TextQueryType.BestFields).Fuzziness(new Fuzziness(1))
                                .Fields(Fields.FromString("*")).Analyzer("standard").Boost(10)
                        )
                    ).Functions(
                        functionScores.ToArray()
                    ).ScoreMode(FunctionScoreMode.Sum).BoostMode(FunctionBoostMode.Sum)
                )
            ).Size(pageSize));

        if (response.IsSuccess())
            return response.Documents;
        return new List<IndexedWebUrl>();
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