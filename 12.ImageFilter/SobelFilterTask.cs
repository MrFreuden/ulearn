using NUnit.Framework;
using System;

namespace Recognizer;

[TestFixture]
public class SobelFilterTaskTests
{
    [Test]
    public void Test1x1()
    {
        var original = new double[1, 1]
        {
            { 1 }
        };
        var sx = new double[1, 1]
        {
            { 2.0 }
        };
        var expected = new double[1, 1]
        {
            { 2.8284271247461903 }
        };

        var result = SobelFilterTask.SobelFilter(original, sx);
        Assert.AreEqual(expected, result);
    }
}

internal static class SobelFilterTask
{
    public static double[,] SobelFilter(double[,] g, double[,] sx)
    {
        var width = g.GetLength(0);
        var height = g.GetLength(1);
        int halfKernelWidth = sx.GetLength(0) / 2;
        int halfKernelHeight = sx.GetLength(1) / 2;
        var result = new double[width, height];
        var sy = GetTransposedMatrix(sx);
        for (int x = halfKernelWidth; x < width - halfKernelWidth; x++)
            for (int y = halfKernelHeight; y < height - halfKernelHeight; y++)
            {
                var sumX = 0.0;
                var sumY = 0.0;
                for (int i = -halfKernelWidth; i <= halfKernelWidth; i++)
                {
                    for (int j = -halfKernelHeight; j <= halfKernelHeight; j++)
                    {
                        sumX += g[x + i, y + j] * sx[halfKernelWidth + i, halfKernelHeight + j];
                        sumY += g[x + i, y + j] * sy[halfKernelWidth + i, halfKernelHeight + j];
                    }
                }
                result[x, y] = Math.Sqrt(sumX * sumX + sumY * sumY);
            }
        return result;
    }

    private static double[,] GetTransposedMatrix(double[,] matrix)
    {
        var transposedMatrix = (double[,])matrix.Clone();
        var matrixLength = matrix.GetLength(0);
        for (int i = 0; i < matrixLength; i++)
        {
            for (int j = 0; j < matrixLength; j++)
            {
                transposedMatrix[j, i] = matrix[i, j];
            }
        }
        return transposedMatrix;
    }
}