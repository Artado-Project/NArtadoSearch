using NArtadoSearch.Core.Utilities.Mapping;
using NArtadoSearch.Core.Utilities.Mapping.Abstractions;
using NArtadoSearch.Core.Utilities.Mapping.Rules;
using NArtadoSearch.Entities.Concrete;
using NArtadoSearch.Entities.Dto;

namespace NArtadoSearch.Business.Mappers;

public class WebIndexMapper : EntityMapperBase<IndexWebUrlDto, IndexedWebUrl>
{
    public override void ConfigureRules()
    {
        Rules.AddRule(
            new UseFactoryMappingRule<IndexWebUrlDto, IndexedWebUrl>(s => s.Keywords, t => t.Keywords, o =>
            {
                var keywords = o as IEnumerable<string>;
                return string.Join(',', keywords);
            }));
    }
}