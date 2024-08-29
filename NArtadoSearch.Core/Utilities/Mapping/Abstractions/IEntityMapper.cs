namespace NArtadoSearch.Core.Utilities.Mapping.Abstractions;

public interface IEntityMapper<in TSource, TTarget> where TTarget : class, new() where TSource : class, new()
{
    TTarget Map(TSource source, Action<MapperConfiguration>? configure = null);
    Task<TTarget> MapAsync(TSource source, Action<MapperConfiguration>? configure = null);
}