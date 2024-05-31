using NUnit.Framework;
using System.Collections.Generic;

namespace Recognizer;

[TestFixture]
public class ThresholdFilterTaskTests
{
    [Test]
    public void With3x3MatrixAndFraction0Point2()
    {
        var original = new double[3, 3]
        {
            { 0.4, 0.8, 0.5 },
            { 0.7, 0.0, 0.2 },
            { 0.6, 0.3, 0.1 },
        };
        var whitePixelsFraction = 0.2;
        var expected = new double[3, 3]
        {
            { 0.0, 1.0, 0.0 },
            { 0.0, 0.0, 0.0 },
            { 0.0, 0.0, 0.0 },
        };

        var result = ThresholdFilterTask.ThresholdFilter(original, whitePixelsFraction);
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void WithDifferent3x3MatrixAndFraction0Point2()
    {
        var original = new double[3, 3]
        {
            { 0.4, 0.5, 0.8 },
            { 0.9, 0.4, 0.5 },
            { 0.9, 0.4, 0.4 },
        };
        var whitePixelsFraction = 0.2;
        var expected = new double[3, 3]
        {
            { 0.0, 0.0, 0.0 },
            { 1.0, 0.0, 0.0 },
            { 1.0, 0.0, 0.0 },
        };

        var result = ThresholdFilterTask.ThresholdFilter(original, whitePixelsFraction);
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void WithSinglePixelAndFractionZero_ReturnsBlackPixel()
    {
        var original = new double[1, 1]
        {
            { 123.0 }
        };
        var whitePixelsFraction = 0;
        var expected = new double[1, 1]
        {
            { 0.0 }
        };

        var result = ThresholdFilterTask.ThresholdFilter(original, whitePixelsFraction);
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void WithSinglePixelAndFractionOne_ReturnsWhitePixel()
    {
        var original = new double[1, 1]
        {
            { 123.0 }
        };
        var whitePixelsFraction = 1;
        var expected = new double[1, 1]
        {
            { 1.0 }
        };

        var result = ThresholdFilterTask.ThresholdFilter(original, whitePixelsFraction);
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void WithEmptyArray_ReturnsEmptyArray()
    {
        var original = new double[1, 1];
        var whitePixelsFraction = 0;

        var expected = new double[1, 1];
        var result = ThresholdFilterTask.ThresholdFilter(original, whitePixelsFraction);
        Assert.AreEqual(expected, result);
    }

    [Test]
    public void With1x2MatrixAndFractionOne_ReturnsAllWhitePixels()
    {
        var original = new double[1, 2]
        {
            { 1, 2 }
        };
        var whitePixelsFraction = 1;
        var expected = new double[1, 2]
        {
            { 1.0, 1.0 }
        };

        var result = ThresholdFilterTask.ThresholdFilter(original, whitePixelsFraction);
        Assert.AreEqual(expected, result);
    }
}
public static class ThresholdFilterTask
{
    public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
    {
        var countPixels = original.Length;
        var numWhitePixels = (int)(whitePixelsFraction * countPixels);

        if (numWhitePixels == 0)
        {
            return new double[original.GetLength(0), original.GetLength(1)];
        }

        var thresholdValue = GetThresholdValue(original, numWhitePixels);
        ApplyThresholdFilter(original, thresholdValue);
        return original;
    }

    private static void ApplyThresholdFilter(double[,] pixelData, double thresholdValue)
    {
        var width = pixelData.GetLength(0);
        var height = pixelData.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pixelData[x, y] >= thresholdValue)
                {
                    pixelData[x, y] = 1.0;
                }
                else
                {
                    pixelData[x, y] = 0.0;
                }
            }
        }
    }

    private static double GetThresholdValue(double[,] pixelData, int numWhitePixels)
    {
        var width = pixelData.GetLength(0);
        var height = pixelData.GetLength(1);
        var allPixels = new List<double>(pixelData.Length);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                allPixels.Add(pixelData[x, y]);
            }
        }

        allPixels.Sort();

        var T = allPixels[pixelData.Length - numWhitePixels];
        return T;
    }
}