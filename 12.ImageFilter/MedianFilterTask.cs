using NUnit.Framework;
using System.Collections.Generic;

namespace Recognizer;
[TestFixture]
public class MedianFilterTaskTests
{
    [Test]
    public void TestWithSingleElement()
    {
        var original = new double[,] { { 0.5 } };
        var result = MedianFilterTask.MedianFilter(original);
        Assert.AreEqual(original, result);
    }

    [Test]
    public void TestWithMultipleElements()
    {
        var original = new double[,] { { 0.1, 0.3 } };
        var expected = new double[,] { { 0.2, 0.2 } };
        var result = MedianFilterTask.MedianFilter(original);
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestWithNoisyImage()
    {
        var original = new double[,] { { 0.1, 0.9, 0.1 }, { 0.9, 0.1, 0.9 }, { 0.1, 0.9, 0.1 } };
        var expected = new double[,] { { 0.5, 0.5, 0.5 }, { 0.5, 0.1, 0.5 }, { 0.5, 0.5, 0.5 } };
        var result = MedianFilterTask.MedianFilter(original);
        Assert.AreEqual(expected, result);
    }
}
internal static class MedianFilterTask
{
    public static double[,] MedianFilter(double[,] original)
    {
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var result = new double[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var neighborhoods = GetNeighborhood(original, x, y);
                result[x, y] = GetMiddleValue(neighborhoods);
            }
        }
        return result;
    }

    private static List<double> GetNeighborhood(double[,] original, int x, int y)
    {
        var neighborhood = new List<double>();
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var newX = x + i;
                var newY = y + j;
                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    neighborhood.Add(original[newX, newY]);
                }
            }
        }
        return neighborhood;
    }

    private static double GetMiddleValue(List<double> list)
    {
        list.Sort();
        if (list.Count % 2 == 0)
            return (list[list.Count / 2 - 1] + list[list.Count / 2]) / 2;
        return list[list.Count / 2];
    }
}
