using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RoutePlanning;

public class Result
{
    public int[] BestOrder { get; set; }
    public double BestLength { get; set; } = double.MaxValue;
}

public static class PathFinderTask
{
    public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
    {
        if (checkpoints == null || checkpoints.Length < 2)
            return new int[1];
        var bestOrder = MakeTrivialPermutation(checkpoints.Length);
        if (checkpoints.Length > 15)
        {
            return FindPathForLargeDataSets(new int[checkpoints.Length], checkpoints, checkpoints);
        }
        var result = new Result();
        FindBestPath(checkpoints, 1, bestOrder, result);
        
        return result.BestOrder;
    }

    private static void FindBestPath(Point[] checkpoints, int startIndex, int[] currentOrder,
        Result result, double currentLength = 0)
    {
        if (startIndex == checkpoints.Length - 1)
        {
            currentLength += PointExtensions.DistanceTo(checkpoints[currentOrder[startIndex - 1]], 
                                                            checkpoints[currentOrder[startIndex]]);
            if (currentLength < result.BestLength)
            {
                result.BestLength = currentLength;
                result.BestOrder = (int[])currentOrder.Clone();
            }
            return;
        }

        for (int i = startIndex; i < checkpoints.Length; i++)
        {
            (currentOrder[startIndex], currentOrder[i]) = (currentOrder[i], currentOrder[startIndex]);
            var newLength = currentLength += PointExtensions.DistanceTo(checkpoints[currentOrder[startIndex - 1]], 
                                                                            checkpoints[currentOrder[startIndex]]);
            if (newLength < result.BestLength)
                FindBestPath(checkpoints, startIndex + 1, currentOrder, result, newLength);
            (currentOrder[startIndex], currentOrder[i]) = (currentOrder[i], currentOrder[startIndex]);
        }
    }

    private static int[] FindPathForLargeDataSets(int[] checkpointOrder, Point[] points, Point[] nearestPoints)
    {
        var availablePoints = nearestPoints.ToList();
        var currentPoint = points[0];
        for (int i = 0; i < points.Length - 1; i++)
        {
            availablePoints.RemoveAt(availablePoints.IndexOf(currentPoint));
            var min = GetNearestPoint(currentPoint, availablePoints);
            currentPoint = min;

            checkpointOrder[i + 1] = Array.IndexOf(points, min);
        }
        return checkpointOrder;
    }

    private static Point GetNearestPoint(Point startPoint, List<Point> points)
    {
        Point min = startPoint;
        double minVal = double.MaxValue;
        for (int i = 0; i < points.Count; i++)
        {
            var val = startPoint.DistanceTo(points[i]);
            if (val < minVal)
            {
                minVal = val;
                min = points[i];
            }
        }
        return min;
    }

    private static int[] MakeTrivialPermutation(int size)
    {
        var bestOrder = new int[size];
        for (var i = 0; i < bestOrder.Length; i++)
            bestOrder[i] = i;
        return bestOrder;
    }
}