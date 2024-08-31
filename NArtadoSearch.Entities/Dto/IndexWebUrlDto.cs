namespace NArtadoSearch.Entities.Dto;

public class IndexWebUrlDto
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string Path { get; set; }
    public DateTime LastCrawled { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public IEnumerable<string> Keywords { get; set; }
}