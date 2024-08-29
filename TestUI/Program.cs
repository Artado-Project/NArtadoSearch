using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using NArtadoSearch.Core.Utilities.Mapping;
using NArtadoSearch.Core.Utilities.Mapping.Abstractions;
using NArtadoSearch.Core.Utilities.Mapping.Rules;
using Newtonsoft.Json;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IEntityMapper<ProductDto, Product>, ProductMapper>();
var serviceProvider = serviceCollection.BuildServiceProvider();

var mapper = serviceProvider.GetService<IEntityMapper<ProductDto, Product>>();

var dto = new ProductDto()
{
    Name = "Bilgisayar",
    Description = "Mükemmel bir bilgisayar."
};

var product = mapper.Map(dto);

Console.WriteLine(JsonConvert.SerializeObject(product));
Console.ReadLine();
class ProductMapper : EntityMapperBase<ProductDto, Product>
{
    public override void ConfigureRules()
    {
        _rules.AddRule(new CustomMappingRule<ProductDto, Product>(dto=>dto.OwnerName, product=>product.Owner));
        _rules.AddRule(new UseDefaultValueIfNullRule<ProductDto, Product>(dto => dto.Quantity, product => product.Quantity, 120));
        _rules.AddRule(new UseDefaultValueIfNullRule<ProductDto, Product>(dto => dto.OwnerName, product => product.Owner, "Abdülaziz"));
    }
}

class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string OwnerName { get; set; }
    public int? Quantity { get; set; }
}

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public int  Quantity { get; set; }
}