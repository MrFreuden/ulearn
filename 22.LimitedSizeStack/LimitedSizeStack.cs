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