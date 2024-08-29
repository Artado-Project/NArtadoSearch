namespace NArtadoSearch.Core.Security.Authentication.Artado;

public class ArtadoTokenOptions
{
    public byte[] Key { get; set; }
    public TimeSpan TokenLifetime { get; set; } = TimeSpan.FromMinutes(5);
}