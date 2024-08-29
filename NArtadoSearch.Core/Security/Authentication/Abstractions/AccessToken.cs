namespace NArtadoSearch.Core.Security.Authentication.Abstractions;

public class AccessToken
{
    public DateTime Expires { get; set; }
    public string Token { get; set; }
}