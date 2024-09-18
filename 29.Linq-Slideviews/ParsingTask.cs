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
            .Select(line =>
            {
                var arr = line.Split(';');
                if (arr.Length != 3 
                || !int.TryParse(arr[0], out int id)
                || !Enum.TryParse(arr[1], true, out SlideType slideType))
                    return null;

                return new SlideRecord(id, slideType, arr[2]);
            })
            .Where(record => record != null)
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
            .Select(line =>
            {
                var arr = line.Split(';');
                if (arr.Length != 4
                || !int.TryParse(arr[0], out int userId)
                || !int.TryParse(arr[1], out int slideId)
                || !DateTime.TryParseExact($"{arr[2]};{arr[3]}", "yyyy-MM-dd;HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)
                || !slides.TryGetValue(slideId, out var record))
                    throw new FormatException($"Wrong line [{line}]");

                return new VisitRecord(userId, slideId, dateTime, record.SlideType);
            });
    }
}