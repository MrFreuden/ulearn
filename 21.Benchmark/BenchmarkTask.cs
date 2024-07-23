using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking;
public class Benchmark : IBenchmark
{
    public double MeasureDurationInMs(ITask task, int repetitionCount)
    {
        if (repetitionCount <= 0)
            throw new ArgumentException("Repetition count must be greater than zero.", nameof(repetitionCount));

        GC.Collect();
        GC.WaitForPendingFinalizers();
        
        task.Run();
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < repetitionCount; i++)
        {
            task.Run();
        }
        sw.Stop();
        return sw.Elapsed.TotalMilliseconds / repetitionCount;
	}
}

[TestFixture]
public class RealBenchmarkUsageSample
{
    public class StringConstructor : ITask
    {
        private int repetitionCount;

        public StringConstructor(int repetitionCount)
        {
            this.repetitionCount = repetitionCount;
        }

        public void Run()
        {
            var stringConstructor = new string('a', repetitionCount);
        }
    }

    public class StringBuilderConstructor : ITask
    {
        private int repetitionCount;

        public StringBuilderConstructor(int repetitionCount)
        {
            this.repetitionCount = repetitionCount;
        }

        public void Run()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < repetitionCount; i++)
            {
                sb.Append('a');
            }
            var stringBuilder = sb.ToString();
        }
    }

    [Test]
    public void StringConstructorFasterThanStringBuilder()
    {
        var repetitionCount = 10000;
        var benchmark = new Benchmark();
        var stringTask = new StringConstructor(repetitionCount);
        var sbTask = new StringBuilderConstructor(repetitionCount);

        var stringConstructorResult = benchmark.MeasureDurationInMs(stringTask, repetitionCount);
        var sbConstructorResult = benchmark.MeasureDurationInMs(sbTask, repetitionCount);

        Assert.Less(stringConstructorResult, sbConstructorResult);
    }
}