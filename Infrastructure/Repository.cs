using SmartHomeIoTSimulator.Core;

namespace SmartHomeIoTSimulator.Infrastructure;

public class Repository<T> where T : IIdentifiable
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T? GetById(int id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    public IEnumerable<T> Find(Func<T, bool> predicate)
    {
        return _items.Where(predicate);
    }

    public int Count()
    {
        return _items.Count;
    }
}