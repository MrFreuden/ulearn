using System.Text;

namespace Delegates.Observers;

public class StackOperationsLogger
{
    private readonly StringBuilder _log = new();
	public void SubscribeOn<T>(ObservableStack<T> stack)
	{
		stack.OnEdit += HandleEvent;
	}

    private void HandleEvent<T>(StackEventData<T> eventData)
    {
        _log.Append(eventData);
    }

    public string GetLog()
	{
		return _log.ToString();
	}
}

public class ObservableStack<T>
{
	public event Action<StackEventData<T>> OnEdit;

	private readonly List<T> _data = new();

	public void Push(T obj)
	{
		_data.Add(obj);
        OnEdit?.Invoke(new StackEventData<T> { IsPushed = true, Value = obj });
	}

	public T Pop()
	{
		if (_data.Count == 0)
			throw new InvalidOperationException();

		var result = _data[_data.Count - 1];
        OnEdit?.Invoke(new StackEventData<T> { IsPushed = false, Value = result });
		return result;
	}
}