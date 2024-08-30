using System.Linq.Expressions;
using NArtadoSearch.Core.Utilities.Mapping.Abstractions;
using NArtadoSearch.Core.Utilities.Mapping.Rules;

namespace NArtadoSearch.Core.Utilities.Mapping;

public abstract class EntityMapperBase<TSource, TTarget>
    : IEntityMapper<TSource, TTarget>
    where TTarget : class, new() where TSource : class, new()
{
    protected readonly IEntityMappingRuleCollection<TSource, TTarget> Rules =
        new EntityMappingRuleCollection<TSource, TTarget>();

    public abstract void ConfigureRules();

    public TTarget Map(TSource source, TTarget? target = null, Action<MapperConfiguration>? configure = null)
    {
        ConfigureRules();

        var mapperConfiguration = new MapperConfiguration();
        if (configure != null)
            configure(mapperConfiguration);

        TTarget targetObject;
        if (target != null)
        {
            targetObject = target;
        }
        else
        {
            targetObject = new TTarget();
        }

        var sourceProperties = typeof(TSource).GetProperties();
        var targetProperties = typeof(TTarget).GetProperties();

        for (int i = 0; i < targetProperties.Length; i++)
        {
            var propertyInfo = targetProperties[i];
            var sourceProperty = typeof(TSource).GetProperty(propertyInfo.Name);
            if (sourceProperty == null) continue;
            try
            {
                propertyInfo.SetValue(targetObject, sourceProperty.GetValue(source));
            }
            catch
            {
                continue;
            }
        }

        Rules.ApplyRules(source, targetObject);

        return targetObject;
    }

    public async Task<TTarget> MapAsync(TSource source, TTarget? target = null,
        Action<MapperConfiguration>? configure = null)
    {
        return await Task.Run(() => Map(source, target: target, configure: configure));
    }
}