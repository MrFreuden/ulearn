namespace rocket_bot;

public class Channel<T> where T : class
{
	/// <summary>
	/// Возвращает элемент по индексу или null, если такого элемента нет.
	/// При присвоении удаляет все элементы после.
	/// Если индекс в точности равен размеру коллекции, работает как Append.
	/// </summary>
	public T this[int index]
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	/// <summary>
	/// Возвращает последний элемент или null, если такого элемента нет
	/// </summary>
	public T LastItem()
	{
		return null;
	}

	/// <summary>
	/// Добавляет item в конец только если lastItem является последним элементом
	/// </summary>
	public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
	{
	}

	/// <summary>
	/// Возвращает количество элементов в коллекции
	/// </summary>
	public int Count
	{
		get
		{
			return 0;
		}
	}
}