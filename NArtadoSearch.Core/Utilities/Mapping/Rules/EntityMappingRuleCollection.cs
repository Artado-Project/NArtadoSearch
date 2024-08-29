namespace NArtadoSearch.Core.Utilities.Mapping.Rules;

public class EntityMappingRuleCollection<TSource, TTarget> : List<IEntityMappingRule<TSource, TTarget>>, IEntityMappingRuleCollection<TSource, TTarget>
{
    public void AddRule(IEntityMappingRule<TSource, TTarget> rule)
    {
        Add(rule);   
    }
}