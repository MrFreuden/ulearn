namespace Names;

internal static class HistogramTask
{
    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        var values = new double[31];
        var days = new string[31];
        for (int i = 0; i < days.Length; i++)
            days[i] = (i + 1).ToString();
        foreach (var nameData in names)
        {
            if (nameData.Name == name && nameData.BirthDate.Day != 1)
                values[nameData.BirthDate.Day - 1]++;
        }
        return new HistogramData(
            $"Рождаемость людей с именем '{name}'", 
            days, 
            values);
    }
}