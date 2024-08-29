using System.Runtime.Serialization;
using System.Security.Claims;

namespace NArtadoSearch.Core.Security.Authentication.Artado;

public class ArtadoTokenBody
{
    public Dictionary<string, string>? Claims { get; set; }
    public DateTime Expires { get; set; }
}