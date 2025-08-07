using System.Diagnostics;

namespace Memory.Timers;

public class Timer : IDisposable
{
    private readonly StringWriter _writer;
    private readonly string _name;
    private readonly Stopwatch _stopwatch;
    private readonly int _level;
    private readonly List<Timer> _childTimer = new();
    private string _line;

    public Timer(StringWriter writer, string name, int level = 0)
    {
        _writer = writer;
        _name = name;
        _level = level;
        _stopwatch = Stopwatch.StartNew();
    }

    public static Timer Start(StringWriter writer, string name = "*")
	{
        return new Timer(writer, name);
	}

    public Timer StartChildTimer(string name)
    {
        var child = new Timer(_writer, name, _level + 1);
        _childTimer.Add(child);
        return child;
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        _line = FormatReportLine(_name, _level, _stopwatch.ElapsedMilliseconds);
        if (_level == 0)
        {
            WriteReport(this);
        }
        
        GC.SuppressFinalize(this);
    }
    
    private void WriteReport(Timer timer)
    {
        _writer.Write(timer._line);
        long childTotalTime = 0;
        foreach (var child in timer._childTimer)
        {
            childTotalTime += child._stopwatch.ElapsedMilliseconds;
            WriteReport(child);
        }

        var restTime = timer._stopwatch.ElapsedMilliseconds - childTotalTime;
        if (timer._childTimer.Count > 0)
        {
            _writer.Write(FormatReportLine("Rest", timer._level + 1, restTime));
        }  
    }

    private static string FormatReportLine(string timerName, int level, long value)
    {
        var intro = new string(' ', level * 4) + timerName;
        return $"{intro,-20}: {value}\n";
    }
}