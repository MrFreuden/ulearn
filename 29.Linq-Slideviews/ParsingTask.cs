using System;
using System.Collections.Generic;
using System.IO;
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
                var id = int.Parse(arr[0]);
                var typeString = char.ToUpper(arr[1][0]) + arr[1].Substring(1);

                if (!Enum.TryParse(typeString, out SlideType slideType))
                {
                    return Enumerable.Empty<SlideRecord>();

                }
                else
                {
                    return new[] { new SlideRecord(id, slideType, arr[2]) };
                }
            })
            .ToDictionary(slide => slide.SlideId, slide => slide);
	}

	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
	/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
	/// Такой словарь можно получить методом ParseSlideRecords</param>
	/// <returns>Список информации о посещениях</returns>
	/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
	public static IEnumerable<VisitRecord> ParseVisitRecords(
		IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
	{
		throw new NotImplementedException();
	}
}