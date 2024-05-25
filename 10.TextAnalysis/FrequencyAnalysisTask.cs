using System.Diagnostics;
using System.Reflection.PortableExecutable;
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
            if (tempDict.ContainsKey(key))
            {
                if (tempDict[key].ContainsKey(words[offset + index + 1]))
                {
                    tempDict[key][words[offset + index + 1]] += 1;
                }
                else
                {
                    tempDict[key].Add(words[offset + index + 1], 1);
                }
            }

            else
            {
                tempDict.Add(key, new Dictionary<string, int> { { words[offset + index + 1], 1 } } );
            }

        }
    }

    private static Dictionary<string, string> GetMostFrequentWords(Dictionary<string, Dictionary<string, int>> tempDict)
    {
        var resultDict = new Dictionary<string, string>();
        foreach (var dict in tempDict)
        {
            var k = dict.Value;
            var val = dict.Key;
            string keyQ = "";
            int max = int.MinValue;
            foreach (var item in k)
            {
                if (item.Value > max)
                {
                    max = item.Value;
                    keyQ = item.Key;
                }
                else if (item.Value == max)
                {
                    if (string.CompareOrdinal(keyQ, item.Key) > 0)
                    {
                        keyQ = item.Key;
                    }
                }
            }
            resultDict.Add(val, keyQ);
        }
        return resultDict;
    }
}
//                                                                                      1 1 1 1 1
//   /5 4 3 2 1

//public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
//{
//    var result = new Dictionary<string, string>();
//    var key = new StringBuilder();
//    for (int currentSentenceIndex = 0; currentSentenceIndex < text.Count; currentSentenceIndex++)
//    {
//        for (int currentWordIndex = 0; currentWordIndex < text[currentSentenceIndex].Count - 1; currentWordIndex++)
//        {
//            for (int offset = 0; offset < text[currentSentenceIndex].Count - 1 - currentWordIndex; offset++)
//            {
//                for (int innerOffset = 0; innerOffset < offset + 1; innerOffset++)
//                {
//                    key.Append(text[currentSentenceIndex][innerOffset + currentWordIndex]);
//                    if (offset > 0 && innerOffset < offset)
//                    {
//                        key.Append(' ');
//                    }
//                }

//                if (result.ContainsKey(key.ToString()))
//                {
//                    continue;
//                }

//                else
//                {
//                    result.Add(key.ToString(), text[currentSentenceIndex][offset + currentWordIndex + 1]);
//                }
//                key.Clear();
//            }
//        }
//    }
//    return result;
//}