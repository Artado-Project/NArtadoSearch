using System.Linq.Expressions;
using System.Reflection;
using NArtadoSearch.Core.Utilities.Mapping.Helpers;

namespace NArtadoSearch.Core.Utilities.Mapping.Rules;

public class UseDefaultValueIfNullRule<TSource, TTarget>(
    Expression<Func<TSource, object>> sourceProperty,
    Expression<Func<TTarget, object>> targetProperty,
    object defaultValue)
    : IEntityMappingRule<TSource, TTarget>
{
    private readonly PropertyInfo? _targetProperty = ExpressionHelper.GetProperty(targetProperty);
    private readonly PropertyInfo? _sourceProperty = ExpressionHelper.GetProperty(sourceProperty);

    public void ApplyRule(TSource source, TTarget target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(_sourceProperty);
        ArgumentNullException.ThrowIfNull(_targetProperty);
        
        var sourceValue = _sourceProperty.GetValue(source);
        if(sourceValue == null)
            _targetProperty.SetValue(target, defaultValue);
    }
}