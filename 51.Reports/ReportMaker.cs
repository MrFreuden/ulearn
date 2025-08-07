using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Text;

namespace Delegates.Reports;

public class ReportMaker<T>
{
    private readonly IMarkup _markupMaker;
    private readonly IStatisticsMaker<T> _statisticsMaker;

    public ReportMaker(IMarkup operation, IStatisticsMaker<T> statValue)
    {
        _markupMaker = operation;
        _statisticsMaker = statValue;
    }

    public string MakeReport(IEnumerable<Measurement> measurements, string caption)
	{
		var data = measurements.ToList();
		var result = new StringBuilder();
		result.Append(_markupMaker.MakeCaption(caption));
		result.Append(_markupMaker.BeginList());
        AppendMeasurementStatistics(result, data);
        result.Append(_markupMaker.EndList());
		return result.ToString();
	}

    private void AppendMeasurementStatistics(StringBuilder result, List<Measurement> data)
    {
        AppendStatisticsItem(result, "Temperature", data.Select(m => m.Temperature));
        AppendStatisticsItem(result, "Humidity", data.Select(m => m.Humidity));
    }

    private void AppendStatisticsItem(StringBuilder result, string name, IEnumerable<double> values)
    {
        var statistics = _statisticsMaker.MakeStatistics(values);
        result.Append(_markupMaker.MakeItem(name, statistics.ToString()));
    }
}

public interface IMarkup
{
    string MakeCaption(string caption);
    string BeginList();
    string MakeItem(string valueType, string entry);
    string EndList();
}

public class HTMLMarkup : IMarkup
{
    public string BeginList()
    {
        return "<ul>";
    }

    public string EndList()
    {
        return "</ul>";
    }

    public string MakeCaption(string caption)
    {
        return $"<h1>{caption}</h1>";
    }

    public string MakeItem(string valueType, string entry)
    {
        return $"<li><b>{valueType}</b>: {entry}";
    }
}

public class MarkDownMarkup : IMarkup
{
    public string BeginList()
    {
        return "";
    }

    public string EndList()
    {
        return "";
    }

    public string MakeCaption(string caption)
    {
        return $"## {caption}\n\n";
    }

    public string MakeItem(string valueType, string entry)
    {
        return $" * **{valueType}**: {entry}\n\n";
    }
}

public interface IStatisticsMaker<TOut>
{
    TOut MakeStatistics(IEnumerable<double> data);
}

public class MeanAndStdStatistics : IStatisticsMaker<MeanAndStd>
{
    public MeanAndStd MakeStatistics(IEnumerable<double> data)
    {
        var list = data.ToList();
        var mean = list.Average();
        var std = Math.Sqrt(list.Select(z => Math.Pow(z - mean, 2)).Sum() / (list.Count - 1));

        return new MeanAndStd
        {
            Mean = mean,
            Std = std
        };
    }
}

public class MedianStatistics : IStatisticsMaker<double>
{
    public double MakeStatistics(IEnumerable<double> data)
    {
        var list = data.OrderBy(z => z).ToList();
        if (list.Count % 2 == 0)
            return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

        return list[list.Count / 2];
    }
}

public static class ReportMakerHelper
{
	public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
	{
        return new ReportMaker<MeanAndStd>(new HTMLMarkup(), new MeanAndStdStatistics())
            .MakeReport(data, "Mean and Std");
	}

	public static string MedianMarkdownReport(IEnumerable<Measurement> data)
	{
        return new ReportMaker<double>(new MarkDownMarkup(), new MedianStatistics())
            .MakeReport(data, "Median");
    }

	public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
	{
        return new ReportMaker<MeanAndStd>(new MarkDownMarkup(), new MeanAndStdStatistics())
            .MakeReport(measurements, "Mean and Std");
    }

	public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
	{
        return new ReportMaker<double>(new HTMLMarkup(), new MedianStatistics())
            .MakeReport(measurements, "Median");
    }
}