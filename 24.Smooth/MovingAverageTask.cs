using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
		Queue<double> window = new();
		var sumValues = 0.0;
        foreach (var item in data)
		{
            window.Enqueue(item.OriginalY);
			sumValues += item.OriginalY;

            if (window.Count > windowWidth)
			{
                sumValues -= window.Dequeue();
			}
		
			var average = sumValues / window.Count;
            yield return item.WithAvgSmoothedY(average);
		}
	}
}