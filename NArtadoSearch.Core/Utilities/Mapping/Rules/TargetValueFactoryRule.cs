using System.Linq.Expressions;
using System.Reflection;
using NArtadoSearch.Core.Utilities.Mapping.Helpers;

namespace NArtadoSearch.Core.Utilities.Mapping.Rules;

public class TargetValueFactoryRule<TSource, TTarget>(Expression<Func<TTarget, object>> targetProperty, 
    Func<object> targetValueFactory)
    : IEntityMappingRule<TSource, TTarget>
{
    private readonly PropertyInfo? _targetProperty = ExpressionHelper.GetProperty(targetProperty);

    public void ApplyRule(TSource source, TTarget target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(_targetProperty);

        var value = targetValueFactory();
        _targetProperty.SetValue(target, value);
    }
}