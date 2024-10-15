using System;

namespace DistanceTask;

public static class DistanceTask
{
    public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
    {
        var ABLengthSquared = GetSquaredLength(ax, ay, bx, by);
        var distanceToA = GetDistance(ax, ay, x, y);
        var distanceToB = GetDistance(bx, by, x, y);

        if (ABLengthSquared == 0)
            return distanceToA;

        var t = GetProjection(ax, ay, bx, by, x, y, ABLengthSquared);

        if (t < 0)
            return distanceToA;
        else if (t > 1)
            return distanceToB;

        var Dx = ax + (bx - ax) * t;
        var Dy = ay + (by - ay) * t;

        return GetDistance(Dx, Dy, x, y);
    }

    private static double GetSquaredLength(double x1, double y1, double x2, double y2)
    {
        var dx = x2 - x1;
        var dy = y2 - y1;
        return dx * dx + dy * dy;
    }

    private static double GetDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(GetSquaredLength(x1, y1, x2, y2));
    }

    private static double GetProjection(double ax, double ay, double bx, double by, double x, double y,
        double ABLengthSquared)
    {
        var APx = x - ax;
        var APy = y - ay;
        var ABx = bx - ax;
        var ABy = by - ay;
        var dotProduct = APx * ABx + APy * ABy;
        return dotProduct / ABLengthSquared;
    }
}
