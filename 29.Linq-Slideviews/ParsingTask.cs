using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
            .Skip(1)
            .Where(line => !string.IsNullOrEmpty(line) && char.IsDigit(line[0]))
            .SelectMany(line =>
            {
                var arr = line.Split(';');
                if (arr.Length != 3)
                    return Enumerable.Empty<SlideRecord>();

                if (!int.TryParse(arr[0], out int id))
                    return Enumerable.Empty<SlideRecord>();

                if (arr[1].Length == 0)
                    return Enumerable.Empty<SlideRecord>();

                var typeString = char.ToUpper(arr[1][0]) + arr[1].Substring(1);
                if (!Enum.TryParse(typeString, out SlideType slideType))
                    return Enumerable.Empty<SlideRecord>();

                return new[] { new SlideRecord(id, slideType, arr[2]) };
            })
            .GroupBy(record => record.SlideId)
            .ToDictionary(slide => slide.Key, slide => slide.First());
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1)
            .SelectMany(line =>
            {
                var arr = line.Split(';', 3);
                if (arr.Length != 3)
                    throw new FormatException($"Wrong line [{line}]");


                if (!int.TryParse(arr[0], out int userId))
                    throw new FormatException($"Wrong line [{line}]");


                if (!int.TryParse(arr[1], out int slideId))
                    throw new FormatException($"Wrong line [{line}]");

                string format = "yyyy-MM-dd;HH:mm:ss";
                if (!DateTime.TryParseExact(arr[2], format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                    throw new FormatException($"Wrong line [{line}]");

                if (!slides.TryGetValue(slideId, out var record))
                    throw new FormatException($"Wrong line [{line}]");
                return new[] { new VisitRecord(userId, slideId, dateTime, record.SlideType) };
            });
    }
}