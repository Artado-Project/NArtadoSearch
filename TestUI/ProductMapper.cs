using System.Security.Cryptography;
using NArtadoSearch.Core.Utilities.Mapping;
using NArtadoSearch.Core.Utilities.Mapping.Rules;

class ProductMapper : EntityMapperBase<ProductDto, Product>
{
    public override void ConfigureRules()
    {
        _rules.AddRule(new UseFactoryMappingRule<ProductDto, Product>(
            dto => dto.PriceDividedToTen,
            product => product.Price,
            c => (decimal)c * 10));
        
        _rules.AddRule(new TargetValueFactoryRule<ProductDto, Product>(c=>c.Id, () => RandomNumberGenerator.GetInt32(0, 150)));
    }
}