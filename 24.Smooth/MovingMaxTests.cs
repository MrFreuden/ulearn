using System.Collections.Generic;
using System;
using System.Linq;
using NUnit.Framework;

namespace yield;

[TestFixture]
public class MovingMaxTests
{
	[Test]
	public void EmptySequence()
	{
		CheckMax(10, new double[] { }, new double[] { });
	}

	[Test]
	public void SingleZero()
	{
		CheckMax(10, new[] { 0.0 }, new[] { 0.0 });
	}

	[Test]
	public void SingleNonZeroValue()
	{
		CheckMax(10, new[] { 100.0 }, new[] { 100.0 });
	}

	[Test]
	public void TwoValuesIncreasing()
	{
		CheckMax(2, new[] { 1, 20.0 }, new[] { 1, 20.0 });
	}

	[Test]
	public void TwoValuesDecreasing()
	{
		CheckMax(2, new[] { 10, 0.0 }, new[] { 10, 10.0 });
	}

	[Test]
	public void TwoValuesWithSmallWindow()
	{
		CheckMax(1, new[] { 10, 0.0 }, new[] { 10, 0.0 });
	}

	[Test]
	public void WithWindow2()
	{
		CheckMax(2, new double[] { 1, 2, 5, 1, 0, 6 }, new double[] { 1, 2, 5, 5, 1, 6});
	}

	[Test]
	public void SmoothWithLargeWindow()
	{
		CheckMax(100500, new double[] { 1, 2, 5, 1, 0, 6 }, new double[] { 1, 2, 5, 5, 5, 6 });
	}

    [Test]
    public void TestMovingMaxOnLargeData()
    {
        int dataSize = 100000;
        int windowWidth = 100;
        var random = new Random();
        var data = new List<double>();

        for (int i = 0; i < dataSize; i++)
        {
            data.Add(random.NextDouble() * 1000);
        }

        var expectedMax = new double[dataSize];
        var deque = new LinkedList<int>();

        for (int i = 0; i < dataSize; i++)
        {
            if (deque.Count > 0 && deque.First.Value <= i - windowWidth)
            {
                deque.RemoveFirst();
            }

            while (deque.Count > 0 && data[deque.Last.Value] <= data[i])
            {
                deque.RemoveLast();
            }

            deque.AddLast(i);

            expectedMax[i] = data[deque.First.Value];
        }

        CheckMax(windowWidth, data.ToArray(), expectedMax);
    }

    private void CheckMax(int windowWidth, double[] ys, double[] expectedYs)
	{
		var dataPoints = ys.Select((v, index) => new DataPoint(GetX(index), v));
		var actual = Factory.CreateAnalyzer().MovingMax(dataPoints, windowWidth).ToList();
		Assert.AreEqual(ys.Length, actual.Count);
		for (var i = 0; i < actual.Count; i++)
		{
			Assert.AreEqual(GetX(i), actual[i].X, 1e-7);
			Assert.AreEqual(ys[i], actual[i].OriginalY, 1e-7);
			Assert.AreEqual(expectedYs[i], actual[i].MaxY, 1e-7);
		}
	}

	private double GetX(int index)
	{
		return (index - 3.0) / 2;
	}
}