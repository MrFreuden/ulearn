using System.Text;

namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public const int MaxNGrams = 3;
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var ngramFrequencies = new Dictionary<string, Dictionary<string, int>>();
        foreach (var sentence in text)
        {
            for (int currentWordIndex = 0; currentWordIndex < sentence.Count - 1; currentWordIndex++)
            {
                UpdateNGramFrequencies(ngramFrequencies, sentence, currentWordIndex);
            }
        }
        return GetMostFrequentWords(ngramFrequencies);
    }

    private static void UpdateNGramFrequencies(Dictionary<string, Dictionary<string, int>> tempDict, 
        List<string> words, int index)
    {
        var keyBuilder = new StringBuilder();
        for (int offset = 0; offset < MaxNGrams - 1 && offset < words.Count - 1 - index; offset++)
        {
            if (offset > 0)
                keyBuilder.Append(' ');
            keyBuilder.Append(words[offset + index]);

            var key = keyBuilder.ToString();
            var nextWord = words[offset + index + 1];
            if (!tempDict.TryGetValue(key, out var nextWordFrequencies))
            {
                nextWordFrequencies = new Dictionary<string, int>();
                tempDict.Add(key, nextWordFrequencies);
            }
            if (!tempDict[key].TryGetValue(nextWord, out var count))
            {
                count = 0;
            }
            nextWordFrequencies[nextWord] = count + 1;
        }
    }

    private static Dictionary<string, string> GetMostFrequentWords(Dictionary<string, Dictionary<string, int>> tempDict)
    {
        var resultDict = new Dictionary<string, string>();
        foreach (var dict in tempDict)
        {
            var countDict = dict.Value;
            string mostFrequentNextWord = "";
            int maxFrequency = int.MinValue;
            foreach (var item in countDict)
            {
                if (item.Value > maxFrequency || 
                    item.Value == maxFrequency && string.CompareOrdinal(mostFrequentNextWord, item.Key) > 0)
                {
                    maxFrequency = item.Value;
                    mostFrequentNextWord = item.Key;
                }
            }
            resultDict.Add(dict.Key, mostFrequentNextWord);
        }
        return resultDict;
    }
}