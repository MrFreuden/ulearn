using System.Collections.Generic;
namespace rocket_bot;

public class Channel<T> where T : class
{
    private readonly object _lock = new object();
    private readonly List<T> _items = new List<T>();
    /// <summary>
    /// Возвращает элемент по индексу или null, если такого элемента нет.
    /// При присвоении удаляет все элементы после.
    /// Если индекс в точности равен размеру коллекции, работает как Append.
    /// </summary>
    public T this[int index]
    {
        get
        {
            lock (_lock)
            {
                if (index >= this.Count)
                {
                    return null;
                }

                return _items[index];
            }
        }

        set
        {
            lock (_lock)
            {
                if (index == this.Count)
                {
                    _items.Add(value);
                }

                else
                {
                    lock (_lock)
                    {
                        _items[index] = value;
                        if (index + 1 < this.Count)
                        {
                            _items.RemoveRange(index + 1, this.Count - index - 1);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Возвращает последний элемент или null, если такого элемента нет
    /// </summary>
    public T LastItem()
    {
        lock (_lock)
        {
            if (this.Count > 0)
            {
                return _items[this.Count - 1];
            }
        }
        return null;
    }

    /// <summary>
    /// Добавляет item в конец только если lastItem является последним элементом
    /// </summary>
    public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
    {
        lock (_lock)
        {
            if (this.Count > 0)
            {

                if (_items[this.Count - 1].Equals(knownLastItem))
                {
                    _items.Add(item);
                }
            }

            else if (this.Count == 0 && knownLastItem == null)
            {
                _items.Add(item);
            }
        }
    }

    /// <summary>
    /// Возвращает количество элементов в коллекции
    /// </summary>
    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _items.Count;
            }
        }
    }
}