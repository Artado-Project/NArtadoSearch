using NArtadoSearch.Core.Utilities.Caching.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NArtadoSearch.Core.Utilities.Caching.Redis;

public class RedisCacheManager(IConnectionMultiplexer connection) : ICacheManager
{
    private readonly IDatabase _database = connection.GetDatabase();

    public void Add<T>(string key, T value)
    {
        _database.StringSet(key, JsonConvert.SerializeObject(value));
    }

    public void Add<T>(string key, T value, TimeSpan expiration)
    {
        _database.StringSet(key, JsonConvert.SerializeObject(value), expiration);
    }

    public void Delete(string key)
    {
        if(_database.KeyExists(key))
            _database.KeyDelete(key);
    }

    public T? Get<T>(string key)
    {
        if (_database.KeyExists(key))
        {
            var value = _database.StringGet(key);
            if(value.HasValue)
                return JsonConvert.DeserializeObject<T>(value);
        }

        return default(T);
    }

    public bool TryGet<T>(string key, out T? value)
    {
        var strValue = _database.StringGet(key);
        if (strValue.HasValue)
        {
            value = JsonConvert.DeserializeObject<T>(strValue);
            if (value != null)
                return true;
        }

        value = default;
        return false;
    }

    public bool CheckExists(string key)
    {
        return _database.KeyExists(key);
    }

    public void ClearCache()
    {
        var endPoints = _database.Multiplexer.GetEndPoints();
        _database.Multiplexer.GetServer(endPoints[0]).FlushDatabase();
    }
}