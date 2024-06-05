using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete;

public class RightBorderTask
{
	/// <returns>
	/// Возвращает индекс правой границы. 
	/// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
	/// Если такого нет, то возвращает items.Length
	/// </returns>
	/// <remarks>
	/// Функция должна быть НЕ рекурсивной
	/// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
	/// </remarks>
    public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
    {
        var prefixLength = prefix.Length;
        while (left < right - 1)
        {
            var middle = (left + right) / 2;
            var comparison = string.Compare(prefix, 0, phrases[middle], 0, prefixLength, 
                                            StringComparison.InvariantCultureIgnoreCase);
            if (comparison >= 0)
            {
				left = middle;
            }
            else
            {
                right = middle;
            }
        }
        return right;
    }
}