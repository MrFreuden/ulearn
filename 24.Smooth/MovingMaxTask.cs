using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
        Queue<double> window = new();
        double? maxValue = null;

        foreach (var item in data)
        {
            window.Enqueue(item.OriginalY);
            if (maxValue == null)
            {
                maxValue = item.OriginalY;
            }
            if (window.Count > windowWidth)
            {
                window.Dequeue();
                maxValue = window.Max();
            }

            var max = maxValue.Value > item.OriginalY ? maxValue.Value : item.OriginalY;
            maxValue = max;
            yield return item.WithMaxY(max);
        }
    }
}