using System.Text;

namespace TextAnalysis;

static class TextGeneratorTask
{
    public const int MaxNGrams = 3;
    public static string ContinuePhrase(
        Dictionary<string, string> nextWords,
        string phraseBeginning,
        int wordsCount)
    {
        var splitWords = phraseBeginning.Split(' ').ToList();
        var phraseBuilder = new StringBuilder();
        var currentWords = splitWords.ToList();
        
        phraseBuilder.Append(phraseBeginning + " ");
        for (int i = 0; i < wordsCount && currentWords.Count > 0; )
        {
            if (currentWords.Count > MaxNGrams && currentWords.Count > 1)
            {
                currentWords.RemoveRange(0, currentWords.Count - MaxNGrams);
            }
            if (!nextWords.TryGetValue(string.Join(' ', currentWords), out var nextWord))
            {
                currentWords.RemoveAt(0);
                continue;
            }

            currentWords.Add(nextWord);
            phraseBuilder.Append(nextWord + " ");
            i++;
        }
        return phraseBuilder.ToString().TrimEnd();
    }
}