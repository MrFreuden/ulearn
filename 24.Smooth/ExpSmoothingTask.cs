using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		double? previousSmoothedValue = null;

        foreach (var item in data)
		{
			if (previousSmoothedValue == null)
			{
				previousSmoothedValue = item.OriginalY;
			}
			else
			{
                previousSmoothedValue = alpha * item.OriginalY + (1 - alpha) * previousSmoothedValue.Value;
            }
            yield return item.WithExpSmoothedY(previousSmoothedValue.Value);
		}
	}
}
