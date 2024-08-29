namespace NArtadoSearch.Core.Utilities.Caching.Abstractions;

public interface ICacheManager
{
    void Add<T>(string key, T value);
    void Add<T>(string key, T value, TimeSpan expiration);
    void Delete(string key);
    T? Get<T>(string key);
    bool TryGet<T>(string key, out T? value);
    bool CheckExists(string key);
    void ClearCache();
}