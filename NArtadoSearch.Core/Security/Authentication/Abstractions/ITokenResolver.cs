using System.Security.Claims;

namespace NArtadoSearch.Core.Security.Authentication.Abstractions;

public interface ITokenResolver
{
    Dictionary<string, string>? Resolve(string token);
}