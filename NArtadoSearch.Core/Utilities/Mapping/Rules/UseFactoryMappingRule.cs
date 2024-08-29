using System.Linq.Expressions;
using System.Reflection;
using NArtadoSearch.Core.Utilities.Mapping.Helpers;

namespace NArtadoSearch.Core.Utilities.Mapping.Rules;

public class UseFactoryMappingRule<TSource, TTarget>(
    Expression<Func<TSource, object>> sourceProperty,
    Expression<Func<TTarget, object>> targetProperty,
    Func<object?, object> factory)
    : IEntityMappingRule<TSource, TTarget>
{
    private readonly PropertyInfo? _sourceProperty = ExpressionHelper.GetProperty(sourceProperty);
    private readonly PropertyInfo? _targetProperty = ExpressionHelper.GetProperty(targetProperty);

    public void ApplyRule(TSource source, TTarget target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(_sourceProperty);
        ArgumentNullException.ThrowIfNull(_targetProperty);
        
        var sourceValue = _sourceProperty.GetValue(source);
        sourceValue = factory(sourceValue);
        _targetProperty.SetValue(target, sourceValue);
    }
}