using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace NArtadoSearch.Core.Entities;

public abstract class IndexedEntityBase : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string Path { get; set; }
    public DateTime LastCrawled { get; set; }
    public int Popularity { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public string Keywords { get; set; }
}