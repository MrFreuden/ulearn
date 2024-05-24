using System;

namespace TextAnalysis;

static class SentencesParserTask
{
    public static List<List<string>> ParseSentences(string text)
    {
        var sentecneSeparators = new char[] { '.', '!', '?', ';', ':', '(', ')'};
        var wordSeparators = new char[] {
                                            '^', '#', '$', '-', '—', '+', '0', '1',
                                            '2', '3', '4', '5', '6', '7', '8', '9', '=',
                                            '\t', '\n', '\r', ',', '…', '“',
                                            '”', '<', '>', '‘', '*', ' ', '/', '\u00A0'};
        var sentencesList = new List<List<string>>();
        var sentences = text.ToLowerInvariant().Split(sentecneSeparators, StringSplitOptions.RemoveEmptyEntries);

        foreach (var sentence in sentences)
        {
            var words = sentence.Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 0)
            {
                sentencesList.Add(words.ToList());
            }
        }
        return sentencesList;
    }
}