namespace NArtadoSearch.Entities.Models;

public class WebSearchResults
{
    public IEnumerable<WebSearchResultItem> Results { get; set; } = new List<WebSearchResultItem>();
    public long ElapsedMilliseconds { get; set; }
    public int Size { get; set; }
    public int Page { get; set; }
}