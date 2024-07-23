using System;
using System.Collections.Generic;

namespace StructBenchmarking;

public class Experiments
{
	public static ChartData BuildChartDataForArrayCreation(
		IBenchmark benchmark, int repetitionsCount)
	{
        return new ChartData
        {
            Title = "Create array",
            ClassPoints = new ArrayCreationFabric().CreateClass(benchmark, repetitionsCount),
			StructPoints = new ArrayCreationFabric().CreateStructure(benchmark, repetitionsCount),
		};
	}

	public static ChartData BuildChartDataForMethodCall(
		IBenchmark benchmark, int repetitionsCount)
	{
        return new ChartData
		{
			Title = "Call method with argument",
            ClassPoints = new MethodCallFabric().CreateClass(benchmark, repetitionsCount),
            StructPoints = new MethodCallFabric().CreateStructure(benchmark, repetitionsCount),
        };
	}
}

public abstract class ExperimentsFabric
{
    protected List<ExperimentResult> CreateExperiment(IBenchmark benchmark, int repetitionsCount, Func<int, ITask> func)
    {
        var res = new List<ExperimentResult>();
        foreach (var fieldCount in Constants.FieldCounts)
        {
            res.Add(new ExperimentResult(fieldCount, benchmark.MeasureDurationInMs(func(fieldCount), repetitionsCount)));
        }
        return res;
    }

    public abstract List<ExperimentResult> CreateClass(IBenchmark benchmark, int repCount);

    public abstract List<ExperimentResult> CreateStructure(IBenchmark benchmark, int repCount);
}

public class ArrayCreationFabric : ExperimentsFabric
{
    public override List<ExperimentResult> CreateClass(IBenchmark benchmark, int repCount)
        => CreateExperiment(benchmark, repCount, fieldCount => new ClassArrayCreationTask(fieldCount));

    public override List<ExperimentResult> CreateStructure(IBenchmark benchmark, int repCount)
        => CreateExperiment(benchmark, repCount, fieldCount => new StructArrayCreationTask(fieldCount));
}

public class MethodCallFabric : ExperimentsFabric
{
    public override List<ExperimentResult> CreateClass(IBenchmark benchmark, int repCount)
        => CreateExperiment(benchmark, repCount, fieldCount => new MethodCallWithClassArgumentTask(fieldCount));

    public override List<ExperimentResult> CreateStructure(IBenchmark benchmark, int repCount)
        => CreateExperiment(benchmark, repCount, fieldCount => new MethodCallWithStructArgumentTask(fieldCount));
}