using System.Collections.Generic;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        Queue<double> window = new();
        LinkedList<double> maxValueApplicants = new();

        foreach (var item in data)
        {
            window.Enqueue(item.OriginalY);

            if (window.Count > windowWidth)
            {
                RemoveOldestElement(window, maxValueApplicants);
            }

            UpdateMaxValueApplicants(maxValueApplicants, item.OriginalY);

            yield return item.WithMaxY(maxValueApplicants.First.Value);
        }
    }

    private static void RemoveOldestElement(Queue<double> window, LinkedList<double> maxValueApplicants)
    {
        var removed = window.Dequeue();
        if (removed == maxValueApplicants.First.Value)
        {
            maxValueApplicants.RemoveFirst();
        }
    }

    private static void UpdateMaxValueApplicants(LinkedList<double> maxValueApplicants, double newValue)
    {
        while (maxValueApplicants.Count > 0 && maxValueApplicants.Last.Value < newValue)
        {
            maxValueApplicants.RemoveLast();
        }
        maxValueApplicants.AddLast(newValue);
    }
}