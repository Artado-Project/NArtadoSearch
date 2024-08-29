namespace NArtadoSearch.Core.Utilities.Mapping.Rules;

public interface IEntityMappingRule<TSource, TTarget>
{
    void ApplyRule(TSource source, TTarget target);
}