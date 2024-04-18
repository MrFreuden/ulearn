using System;

namespace AngryBirds;

public static class AngryBirdsTask
{
    const double G = 9.8;
    public static double FindSightAngle(double v, double distance)
    {
        var ratio = (distance * G) / (v * v);
        return Math.Asin(ratio) / 2;
    }
}