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
		var s = lines
			.Skip(1)
			.Where(line => !string.IsNullOrEmpty(line) && char.IsDigit(line[0]))
			.Select(line => line.Split(';'))
			.Select(arr => arr
				.Select((line, index) => index == 1 ? char.ToUpper(line[0]) + line.Substring(1) : line)
				.ToArray())
			.Where(arr => arr.Length == 3)
			.Select(arr => new SlideRecord(int.Parse(arr[0]), (SlideType)Enum.Parse(typeof(SlideType), arr[1]), arr[2]))
			.ToDictionary(g => g.SlideId, g => g);
		;
		return s;
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