namespace Delegates.Observers;

public class StackEventData<T>
{
	public bool IsPushed { get; set; }
	public T Value { get; init; }
	public override string ToString()
	{
		return (IsPushed ? "+" : "-") + Value;
	}
}