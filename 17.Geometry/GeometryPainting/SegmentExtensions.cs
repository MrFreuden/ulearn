using Avalonia.Media;
using GeometryTasks;
using System.Collections.Generic;

namespace GeometryPainting;

static class SegmentExtensions
{
    private static Dictionary<Segment, Color> colors = new();

    public static Color GetColor(this Segment segment)
    {
        if (colors.TryGetValue(segment, out var val)) return val;
        return Colors.Black;
    }

    public static void SetColor(this Segment segment, Color color)
    {
        colors[segment] = color;
    }
}