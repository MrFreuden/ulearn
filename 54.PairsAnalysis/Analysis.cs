using NUnit.Framework;

namespace Delegates.PairsAnalysis;

public static class Analysis
{
    public static int FindMaxPeriodIndex(params DateTime[] data)
    {
        return Analyze(
            data,
            temp =>
            {
                if (!temp.Any()) throw new InvalidOperationException();
                return temp.MaxIndex();
            },
            (source1, source2) => (source2 - source1).TotalSeconds
        );
    }

    public static double FindAverageRelativeDifference(params double[] data)
    {
        return Analyze(
            data,
            temp =>
            {
                return temp.Average();
            },
            (source1, source2) => (source2 - source1) / source1
        );
    }

    private static TResult Analyze<TSource, TIntermediate, TResult>(
        IEnumerable<TSource> data,
        Aggregate<TIntermediate, TResult> aggregate,
        Process<TSource, TIntermediate> process)
    {
        var dataList = data.ToList();

        if (dataList.Count < 2)
            throw new InvalidOperationException();

        var temp = dataList.Pairs().Select(pair => process(pair.Item1, pair.Item2));
        return aggregate(temp);
    }

    public delegate TResult Aggregate<TIntermediate, TResult>(IEnumerable<TIntermediate> temp);
    public delegate TIntermediate Process<TSource, TIntermediate>(TSource source1, TSource source2);
}

public static class TExtension
{
    public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> data)
    {
        T previous = default;
        var hasPrevious = false;

        foreach (var current in data)
        {
            if (hasPrevious)
            {
                yield return Tuple.Create(previous, current);
            }

            previous = current;
            hasPrevious = true;
        }
    }

    public static int MaxIndex<T>(this IEnumerable<T> data) where T : IComparable<T>
    {
        var maxIndex = -1;
        var currentIndex = 0;
        T maxValue = default;
        var hasValue = false;

        foreach (var item in data)
        {
            if (!hasValue || item.CompareTo(maxValue) > 0)
            {
                maxValue = item;
                maxIndex = currentIndex;
                hasValue = true;
            }
            currentIndex++;
        }
        if (maxIndex == -1)
            throw new InvalidOperationException();
        return maxIndex;
    }
}