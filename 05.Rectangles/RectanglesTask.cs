using System;

namespace Rectangles;

public static class RectanglesTask
{
    public static bool AreIntersected(Rectangle r1, Rectangle r2)
    {
        return r1.Left <= r2.Right && r1.Right >= r2.Left && r1.Top <= r2.Bottom && r1.Bottom >= r2.Top;
    }

    public static int IntersectionSquare(Rectangle r1, Rectangle r2)
    {
        int left = Math.Max(r1.Left, r2.Left);
        int top = Math.Max(r1.Top, r2.Top);
        int right = Math.Min(r1.Right, r2.Right);
        int bottom = Math.Min(r1.Bottom, r2.Bottom);
        int width = right - left;
        int height = bottom - top;
        if (width < 0 || height < 0)
            return 0;
        return width * height;
    }

    public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
    {
        if (r1.Left <= r2.Left && r1.Right >= r2.Right && r1.Top <= r2.Top && r1.Bottom >= r2.Bottom)
            return 1;
        if (r1.Left >= r2.Left && r1.Right <= r2.Right && r1.Top >= r2.Top && r1.Bottom <= r2.Bottom)
            return 0;
        return -1;
    }
}