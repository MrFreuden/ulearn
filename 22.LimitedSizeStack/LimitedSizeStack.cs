using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private LinkedList<T> _values;
    private readonly int _undoLimit;
    public LimitedSizeStack(int undoLimit)
    {
        _values = new LinkedList<T>();
        _undoLimit = undoLimit;
    }

    public void Push(T item)
    {
        if (_undoLimit == 0)
        {
            return;
        }
        if (Count == _undoLimit)
        {
            _values.RemoveFirst();
        }
        _values.AddLast(item);
    }

    public T Pop()
    {
        var val = _values.Last.Value;
        _values.RemoveLast();
        return val;
    }

    public int Count => _values.Count;
}

public class MyLimitedSizeStack<T>
{
    private MyLinkedList<T> _values;
    private readonly int _undoLimit;
    public MyLimitedSizeStack(int undoLimit)
    {
        _values = new MyLinkedList<T>();
        _undoLimit = undoLimit;
    }

    public void Push(T item)
    {
        if (_undoLimit == 0)
        {
            return;
        }
        if (Count == _undoLimit)
        {
            _values.RemoveFirst();
        }
        _values.AddLast(item);
    }

    public T Pop()
    {
        var val = _values.Last.Value;
        _values.RemoveLast();
        return val;
    }

    public int Count => _values.Count;
}

public class MyLinkedList<T>
{
    private ListItem<T>? _first;
    private ListItem<T>? _last;
    public ListItem<T>? First => _first;
    public ListItem<T>? Last => _last;
    private int _count;

    public MyLinkedList()
    {
        _first = null;
        _last = null;
    }

    public void AddFirst(T value)
    {
        var n = new ListItem<T> { Value = value, Next = _first };
        if (_count != 0 && _first != null)
        {
            _first.Prev = n;
        }
        _first = n;
        _count++;
    }

    public void AddLast(T value)
    {
        var n = new ListItem<T> { Value = value, Prev = _last };

        if (_count != 0 && _last != null)
        {
            _last.Next = n;
        }
        _first ??= n;
        _last = n;
        _count++;
    }

    public void RemoveFirst()
    {
        if (_count == 0)
        {
            return;
        }

        var n = _first?.Next;
        if (n != null)
        {
            n.Prev = null;
        }
        _first = n;
        _count--;
    }

    public void RemoveLast()
    {
        if (_count == 0)
        {
            return;
        }

        var n = _last?.Prev;
        if (n != null)
        {
            n.Next = null;
        }
        _last = n;
        _count--;
    }

    public int Count => _count;
}

public class ListItem<T>
{
    public T Value { get; set; } = default!;
    public ListItem<T>? Next { get; set; }
    public ListItem<T>? Prev { get; set; }
}
