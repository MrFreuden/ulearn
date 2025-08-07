using NUnit.Framework;
using System.ComponentModel;
using System.Diagnostics;

namespace Reflection.Randomness;

public class T1
{
	[FromDistribution(typeof(NormalDistribution), 1, 2)]
	public double A { get; set; }
}

public class T2
{
	[FromDistribution(typeof(NormalDistribution), -1, 2)]
	public double A { get; set; }

	[FromDistribution(typeof(ExponentialDistribution), 4)]
	[DisplayName("B-value")]
	public double B { get; set; }

	// ReSharper disable once UnusedMember.Global
	public double C => 42;

	public double D { get; set; }

	[FromDistribution(typeof(NormalDistribution))]
	public double E { get; set; }
}

public class T3
{
	// ReSharper disable once UnusedMember.Global
	[FromDistribution(typeof(NormalDistribution), 1, 2, 3)]
	public double WrongDistributionArguments { get; set; }
}

public class T4
{
	// ReSharper disable once UnusedMember.Global
	[FromDistribution(typeof(List), 1)]
	public double WrongDistributionType { get; set; }
}

[TestFixture]
public class Generator_should
{
	private const int seed = 123;
	private static readonly ExponentialDistribution defaultBDistribution = new ExponentialDistribution(4);
	private static readonly NormalDistribution defaultADistribution = new NormalDistribution(-1, 2);
	private static readonly NormalDistribution defaultEDistribution = new NormalDistribution();

	[Test]
	public void GenerateT1()
	{
		var rnd = new Random(seed);
		var e = new Generator<T1>().Generate(rnd);
		AssertPropertyFilledWithDistribution(e.A, new NormalDistribution(1, 2));
	}

	[Test]
	public void GiveUniqueResults_OnEveryGenerateRun()
	{
		var rnd = new Random(seed);
		var generator = new Generator<T1>();
		var e1 = generator.Generate(rnd);
		var e2 = generator.Generate(rnd);
		Assert.AreNotSame(e1, e2);
		Assert.AreNotEqual(e1.A, e2.A);
	}

	[Test]
	public void GenerateT2()
	{
		var rnd = new Random(seed);
		var e = new Generator<T2>().Generate(rnd);
		AssertPropertyFilledWithDistribution(e.A, defaultADistribution);
		AssertPropertyFilledWithDistribution(e.B, defaultBDistribution);
		Assert.AreEqual(0.0, e.D, 1e-3, "property without attributes should not be changed");
		Assert.AreEqual(42.0, e.C, 1e-3, "property without attributes should not be changed");
		AssertPropertyFilledWithDistribution(e.E, defaultEDistribution);
	}


	[Test]
	public void PerformanceTest()
	{
		var rnd = new Random(seed);
		var g = new Generator<T2>();
		var sw = Stopwatch.StartNew();
		for (int i = 0; i < 100000; i++)
		{
			g.Generate(rnd);
		}
		// Console.WriteLine(sw.Elapsed);
		Assert.Less(sw.ElapsedMilliseconds, 2000);
	}

	[Test]
	public void FailWithInformativeMessage_WhenAttributeUsedWithWrongNumberOfArguments()
	{
		FailWithInformativeMessageOnType<T3>("NormalDistribution", "Exception message should be informative and contain at least the name of problematic type");
	}

	[Test]
	public void FailWithInformativeMessage_WhenAttributeUsedWithWrongDistributionType()
	{
		FailWithInformativeMessageOnType<T4>("List", "Exception message should be informative and contain at least the name of problematic type");
	}
        
	public void FailWithInformativeMessageOnType<T>(string expectedMessageSubstring, string failMessage) where T : new()
	{
		ArgumentException exception = null;
		try
		{
			new Generator<T>().Generate(new Random(seed));
		}
		catch (ArgumentException e)
		{
			exception = e;

		}
		catch (TypeInitializationException e)
		{
			Assert.That(e.InnerException, Is.AssignableTo<ArgumentException>());
			exception = (ArgumentException)e.InnerException;
		}
		if (exception == null)
			Assert.Fail("Throw expected!");
		Assert.That(exception.Message, Contains.Substring(expectedMessageSubstring), failMessage);
	}


	private void AssertPropertyFilledWithDistribution(double actualValue, IContinuousDistribution distribution)
	{
		var random = new Random(seed);
		var sequenceStart = new[] { distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random) };
		random = new Random(seed);
		random.NextDouble();
		var sequence = sequenceStart.Concat(new[] { distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random), distribution.Generate(random) });
		Assert.That(sequence, Does.Contain(actualValue));
	}
}