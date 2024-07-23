using System.Collections.Generic;

namespace StructBenchmarking;

public class Experiments
{
	public static ChartData BuildChartDataForArrayCreation(
		IBenchmark benchmark, int repetitionsCount)
	{
		var classesTimes = new List<ExperimentResult>();
		var structuresTimes = new List<ExperimentResult>();

        foreach (var fieldCount in Constants.FieldCounts)
        {
			classesTimes.Add(new ExperimentResult(fieldCount, benchmark.MeasureDurationInMs(new ClassArrayCreationTask(fieldCount), repetitionsCount)));
			structuresTimes.Add(new ExperimentResult(fieldCount, benchmark.MeasureDurationInMs(new StructArrayCreationTask(fieldCount), repetitionsCount)));
        }

        return new ChartData
		{
			Title = "Create array",
			ClassPoints = classesTimes,
			StructPoints = structuresTimes,
		};
	}

	public static ChartData BuildChartDataForMethodCall(
		IBenchmark benchmark, int repetitionsCount)
	{
		var classesTimes = new List<ExperimentResult>();
		var structuresTimes = new List<ExperimentResult>();


		foreach (var fieldCount in Constants.FieldCounts)
        {
            classesTimes.Add(new ExperimentResult(fieldCount, benchmark.MeasureDurationInMs(new MethodCallWithClassArgumentTask(fieldCount), repetitionsCount)));
            structuresTimes.Add(new ExperimentResult(fieldCount, benchmark.MeasureDurationInMs(new MethodCallWithStructArgumentTask(fieldCount), repetitionsCount)));
        }

        return new ChartData
		{
			Title = "Call method with argument",
			ClassPoints = classesTimes,
			StructPoints = structuresTimes,
		};
	}
}
