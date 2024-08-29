using System.Collections;

namespace NArtadoSearch.Core.Utilities.Mapping.Rules;

public interface IEntityMappingRuleCollection<TSource, TTarget> : 
    ICollection<IEntityMappingRule<TSource, TTarget>>, 
    IEnumerable<IEntityMappingRule<TSource, TTarget>>, 
    IEnumerable
{
    void AddRule(IEntityMappingRule<TSource, TTarget> rule);
    void ApplyRules(TSource source, TTarget target);
}