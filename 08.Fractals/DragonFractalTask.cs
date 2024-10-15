using System;

namespace Fractals;

internal static class DragonFractalTask
{
    public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
    {
        double x = 1, y = 0;
        var random = new Random(seed);

        for (int i = 0; i < iterationsCount; i++)
        {
            double angleInDegrees = random.Next() % 2 == 0 ? 45 : 135;
            double angleInRadians = ToRadians(angleInDegrees);
            (x, y) = Transform(x, y, angleInDegrees, angleInRadians);
            pixels.SetPixel(x, y);
        }
    }

    private static double ToRadians(double angleInDegrees)
    {
        return Math.PI * angleInDegrees / 180.0;
    }

    private static (double, double) Transform(double x, double y, double angleInDegrees, double angleInRadians)
    {
        double newX = (x * Math.Cos(angleInRadians) - y * Math.Sin(angleInRadians)) / Math.Sqrt(2);
        double newY = (x * Math.Sin(angleInRadians) + y * Math.Cos(angleInRadians)) / Math.Sqrt(2);

        if (angleInDegrees == 135)
        {
            newX += 1;
        }

        return (newX, newY);
    }
}
