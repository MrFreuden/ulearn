using System.Text;

namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    const int MaxNGrams = 3;
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var tempDict = new Dictionary<string, Dictionary<string, int>>();
        foreach (var sentence in text)
        {
            for (int currentWordIndex = 0; currentWordIndex < sentence.Count - 1; currentWordIndex++)
            {
                MakeNGram(tempDict, sentence, currentWordIndex);
            }
        }
        return GetMostFrequentWords(tempDict);
    }

    private static void MakeNGram(Dictionary<string, Dictionary<string, int>> tempDict, List<string> words, int index)
    {
        var keyBuilder = new StringBuilder();
        for (int offset = 0; offset < MaxNGrams - 1 && offset < words.Count - 1 - index; offset++)
        {
            if (offset > 0)
                keyBuilder.Append(' ');
            keyBuilder.Append(words[offset + index]);

            var key = keyBuilder.ToString();
            var nextWord = words[offset + index + 1];
            if (!tempDict.TryGetValue(key, out var countDict))
            {
                countDict = new Dictionary<string, int>();
                tempDict.Add(key, countDict);
            }
            if (!tempDict[key].TryGetValue(nextWord, out var count))
            {
                count = 0;
            }
            countDict[nextWord] = count + 1;
        }
    }

    private static Dictionary<string, string> GetMostFrequentWords(Dictionary<string, Dictionary<string, int>> tempDict)
    {
        var resultDict = new Dictionary<string, string>();
        foreach (var dict in tempDict)
        {
            var countDict = dict.Value;
            string key = "";
            int max = int.MinValue;
            foreach (var item in countDict)
            {
                if (item.Value > max)
                {
                    max = item.Value;
                    key = item.Key;
                }
                else if (item.Value == max)
                {
                    if (string.CompareOrdinal(key, item.Key) > 0)
                    {
                        key = item.Key;
                    }
                }
            }
            resultDict.Add(dict.Key, key);
        }
        return resultDict;
    }
}