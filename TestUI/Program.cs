using System.Security.Authentication.ExtendedProtection;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using NArtadoSearch.Core.Security.Authentication.Abstractions;
using NArtadoSearch.Core.Security.Authentication.Artado;
using NArtadoSearch.Core.Utilities.Mapping;
using NArtadoSearch.Core.Utilities.Mapping.Abstractions;
using NArtadoSearch.Core.Utilities.Mapping.Rules;
using Newtonsoft.Json;

var serviceCollection = new ServiceCollection();
serviceCollection.Configure<ArtadoTokenOptions>(c =>
{
    c.TokenLifetime = TimeSpan.FromHours(10);
    c.Key = new byte[16];
    RandomNumberGenerator.Fill(c.Key);
});
serviceCollection.AddSingleton<ITokenGenerator, ArtadoTokenGenerator>();
serviceCollection.AddSingleton<ITokenResolver, ArtadoTokenResolver>();
var serviceProvider = serviceCollection.BuildServiceProvider();

var generator = serviceProvider.GetRequiredService<ITokenGenerator>();
var resolver = serviceProvider.GetRequiredService<ITokenResolver>();

Dictionary<string, string> claims = new Dictionary<string, string>();
claims["name"] = "John Doe";

var token = generator.GenerateToken(claims);

var resolved = resolver.Resolve(token.Token);
Console.WriteLine(resolved["name"]);

Console.ReadKey();