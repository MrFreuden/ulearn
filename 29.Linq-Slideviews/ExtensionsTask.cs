using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
        var sortedItems = items.OrderBy(x => x).ToList();
        var count = sortedItems.Count;
        if (count == 0)
			throw new InvalidOperationException("Sequence contains no elements");

		var q = count % 2 == 0 
			? sortedItems.Skip(count / 2 - 1).Take(2).Average() 
			: sortedItems.Skip(count / 2).Take(1).First();
		return q;
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
        var hasPrev = false;
		var prevItem = default(T);
		foreach (var item in items)
		{
			if (hasPrev)
				yield return (prevItem, item);

			hasPrev = true;
			prevItem = item;
		}
    }
}