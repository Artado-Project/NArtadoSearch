using System.Security.Cryptography;
using NArtadoSearch.Core.Utilities.Mapping;
using NArtadoSearch.Core.Utilities.Mapping.Rules;

class ProductMapper : EntityMapperBase<ProductDto, Product>
{
    public override void ConfigureRules()
    {
        Rules.AddRule(new UseFactoryMappingRule<ProductDto, Product>(
            dto => dto.PriceDividedToTen,
            product => product.Price,
            c => (decimal)c * 10));
        
        Rules.AddRule(new TargetValueFactoryRule<ProductDto, Product>(c=>c.Id, () => RandomNumberGenerator.GetInt32(0, 150)));
    }
}