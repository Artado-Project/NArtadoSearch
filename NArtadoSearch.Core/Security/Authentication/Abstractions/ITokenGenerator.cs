using System.Security.Claims;

namespace NArtadoSearch.Core.Security.Authentication.Abstractions;

public interface ITokenGenerator
{
    AccessToken GenerateToken(Dictionary<string, string> claims);
}