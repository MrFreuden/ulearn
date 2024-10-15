using System.Xml.Linq;

namespace Names;

internal static class HeatmapTask
{
    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        var months = new string[12];
        var days = new string[30];
        var map = new double[days.Length, months.Length];

        for (int i = 0; i < months.Length; i++)
            months[i] = (i + 1).ToString();
        
        for (int i = 0; i < days.Length; i++)
            days[i] = (i + 2).ToString();

        foreach (var nameData in names)
        {
            if (nameData.BirthDate.Day == 1)
                continue;
            map[nameData.BirthDate.Day - 2, nameData.BirthDate.Month - 1]++;
        }

        return new HeatmapData(
            "Пример карты интенсивностей",
            map, 
            days, 
            months);
    }
}