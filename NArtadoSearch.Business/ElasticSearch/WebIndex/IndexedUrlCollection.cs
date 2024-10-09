using NArtadoSearch.Entities.Concrete;

namespace NArtadoSearch.Business.ElasticSearch.WebIndex;

public class IndexedUrlCollection : List<IndexedWebUrl>
{
    public long TotalCount { get; set; }
}

public static class IndexedUrlCollectionExtensions
{
    public static IndexedUrlCollection ToIndexedUrlCollection(this IEnumerable<IndexedWebUrl> collection, long totalCount)
    {
        if (totalCount <= 0)
            throw new ArgumentException($"{nameof(IndexedWebUrl)}->{nameof(totalCount)}: total count must be greater than zero.");
        
        var urlCollection = new IndexedUrlCollection();
        urlCollection.AddRange(collection);
        urlCollection.TotalCount = totalCount;
        return urlCollection;
    }
}