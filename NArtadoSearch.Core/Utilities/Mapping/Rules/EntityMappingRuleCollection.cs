namespace NArtadoSearch.Core.Utilities.Mapping.Rules;

public class EntityMappingRuleCollection<TSource, TTarget> : List<IEntityMappingRule<TSource, TTarget>>, IEntityMappingRuleCollection<TSource, TTarget>
{
    public void AddRule(IEntityMappingRule<TSource, TTarget> rule)
    {
        Add(rule);   
    }

    public void ApplyRules(TSource source, TTarget target)
    {
        foreach (var rule in this)
        {
            rule.ApplyRule(source, target);
        }
    }
}