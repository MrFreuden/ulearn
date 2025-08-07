namespace Memory.API;

public static class MagicAPI
{
	static HashSet<int> allocated = new();

	public static void Allocate(int id)
	{
		allocated.Add(id);
	}

	public static void Free(int id)
	{
		if (!allocated.Contains(id))
		{
			allocated.Add(100500);
			throw new ArgumentException();
		}
		allocated.Remove(id);
	}

	// Не используйте в коде — этот метод публичный только для тестов!
	public static bool Contains(int id)
	{
		return allocated.Contains(id);
	}



}