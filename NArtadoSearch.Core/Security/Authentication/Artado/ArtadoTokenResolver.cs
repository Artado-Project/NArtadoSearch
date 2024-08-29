using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using NArtadoSearch.Core.Security.Authentication.Abstractions;
using NArtadoSearch.Core.Security.Encryption;
using Newtonsoft.Json;

namespace NArtadoSearch.Core.Security.Authentication.Artado;

public class ArtadoTokenResolver(IOptionsSnapshot<ArtadoTokenOptions> options) : ITokenResolver
{
    private readonly ArtadoTokenOptions _options = options.Value;

    public Dictionary<string, string>? Resolve(string token)
    {
        var nonce = Convert.FromBase64String(token.Split("ARTADO_PREMIUM_KEY")[1]);
        var bytes = Convert.FromBase64String(token.Split("ARTADO_PREMIUM_KEY")[0]);
        var result = AesEncryption.Decrypt(bytes, nonce, _options.Key);
        ArtadoTokenBody? body = JsonConvert.DeserializeObject<ArtadoTokenBody>(result);
        if (body == null)
            return null;
        
        return body.Expires < DateTime.UtcNow ?
            null : 
            body.Claims;
    }
}