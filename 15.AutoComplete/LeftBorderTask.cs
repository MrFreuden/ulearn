using System;
using System.Collections.Generic;

namespace Autocomplete;

public class LeftBorderTask
{
    /// <returns>
    /// Возвращает индекс левой границы.
    /// То есть индекс максимальной фразы, которая не начинается с prefix и меньшая prefix.
    /// Если такой нет, то возвращает -1
    /// </returns>
    /// <remarks>
    /// Функция должна быть рекурсивной
    /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
    /// </remarks>
    public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
    {
        if (left == right - 1)
        {
            return left;
        }
        var middle = (left + right) / 2;
        var comparison = string.Compare(prefix, 0, phrases[middle], 0, prefix.Length,
                                            StringComparison.InvariantCultureIgnoreCase);
        if (comparison <= 0)
        {
            return GetLeftBorderIndex(phrases, prefix, left, middle);
        }
        return GetLeftBorderIndex(phrases, prefix, middle, right);
    }
}