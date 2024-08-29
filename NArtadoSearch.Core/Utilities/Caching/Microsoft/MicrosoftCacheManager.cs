using Microsoft.Extensions.Caching.Memory;
using NArtadoSearch.Core.Utilities.Caching.Abstractions;

namespace NArtadoSearch.Core.Utilities.Caching.Microsoft;

public class MicrosoftCacheManager(IMemoryCache memoryCache) : ICacheManager
{
    public void Add<T>(string key, T value)
    {
        memoryCache.Set(key, value);
    }

    public void Add<T>(string key, T value, TimeSpan expiration)
    {
        memoryCache.Set(key, value, expiration);
    }

    public void Delete(string key)
    {
        memoryCache.Remove(key);
    }

    public T? Get<T>(string key)
    {
        memoryCache.TryGetValue(key, out T? result);
        return result;
    }

    public bool TryGet<T>(string key, out T? value)
    {
        return memoryCache.TryGetValue(key, out value);
    }

    public bool CheckExists(string key)
    {
        return memoryCache.TryGetValue(key, out _);
    }

    public void ClearCache()
    {
        ((MemoryCache)memoryCache).Clear();
    }
}