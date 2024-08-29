namespace NArtadoSearch.Core.Entities;

/// <summary>
/// All classes which implements IEntity is a database entity.
/// </summary>
public interface IEntity
{
    public int Id { get; set; }
}