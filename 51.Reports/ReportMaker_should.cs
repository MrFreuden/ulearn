using NUnit.Framework;

namespace Delegates.Reports;

[TestFixture]
public class ReportMaker_should
{
	private readonly List<Measurement> data = new()
	{
		new Measurement
		{
			Humidity = 1,
			Temperature = -10
		},
		new Measurement
		{
			Humidity = 2,
			Temperature = 2
		},
		new Measurement
		{
			Humidity = 3,
			Temperature = 14
		},
		new Measurement
		{
			Humidity = 2,
			Temperature = 30
		}
	};

	[Test]
	public void MeanAndStdHtml()
	{
		var expected =
			@"<h1>Mean and Std</h1><ul><li><b>Temperature</b>: 9±17.08800749063506<li><b>Humidity</b>: 2±0.816496580927726</ul>";
		var actual = ReportMakerHelper.MeanAndStdHtmlReport(data);
		Assert.AreEqual(expected, actual);
	}

	[Test]
	public void MedianMarkdown()
	{
		var expected = "## Median\n\n * **Temperature**: 8\n\n * **Humidity**: 2\n\n";
		var actual = ReportMakerHelper.MedianMarkdownReport(data);
		Assert.AreEqual(expected, actual);
	}

	[Test(Description = "Новый тест, который нужно сделать рабочим после рефакторинга")]
	public void MeanAndStdMarkdown()
	{
		var expected =
			"## Mean and Std\n\n * **Temperature**: 9±17.08800749063506\n\n * **Humidity**: 2±0.816496580927726\n\n";
		var actual = ReportMakerHelper.MeanAndStdMarkdownReport(data);
		Assert.AreEqual(expected, actual);
	}

	[Test(Description = "Новый тест, который нужно сделать рабочим после рефакторинга")]
	public void MedianHtml()
	{
		var expected = "<h1>Median</h1><ul><li><b>Temperature</b>: 8<li><b>Humidity</b>: 2</ul>";
		var actual = ReportMakerHelper.MedianHtmlReport(data);
		Assert.AreEqual(expected, actual);
	}
}