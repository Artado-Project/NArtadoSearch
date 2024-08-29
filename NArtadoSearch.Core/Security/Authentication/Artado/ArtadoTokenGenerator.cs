using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using NArtadoSearch.Core.Security.Authentication.Abstractions;
using NArtadoSearch.Core.Security.Encryption;
using Newtonsoft.Json;

namespace NArtadoSearch.Core.Security.Authentication.Artado;

public class ArtadoTokenGenerator(IOptionsSnapshot<ArtadoTokenOptions> options) : ITokenGenerator
{
    private ArtadoTokenOptions _options = options.Value;

    public AccessToken GenerateToken(Dictionary<string, string> claims)
    {
        ArtadoTokenBody body = new ArtadoTokenBody
        {
            Claims = claims,
            Expires = DateTime.UtcNow.Add(_options.TokenLifetime)
        };

        return new AccessToken
        {
            Expires = body.Expires,
            Token = CreateTokenString(body)
        };
    }

    private string CreateTokenString(ArtadoTokenBody body)
    {
        var bodyJson = JsonConvert.SerializeObject(body);
        var result = AesEncryption.Encrypt(bodyJson, _options.Key);
        return Convert.ToBase64String(result.cipherText) + "ARTADO_PREMIUM_KEY" + Convert.ToBase64String(result.nonce);
    }
}