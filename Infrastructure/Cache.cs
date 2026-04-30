namespace SmartHomeIoTSimulator.Infrastructure;

public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _storage = new();

    public void Set(TKey key, TValue value)
    {
        _storage[key] = value;
    }

    public TValue? Get(TKey key)
    {
        return _storage.TryGetValue(key, out var value) ? value : default;
    }

    public bool Contains(TKey key)
    {
        return _storage.ContainsKey(key);
    }

    public void Remove(TKey key)
    {
        _storage.Remove(key);
    }
}