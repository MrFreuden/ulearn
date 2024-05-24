namespace TextAnalysis;

static class SentencesParserTask
{
    public static List<List<string>> ParseSentences(string text)
    {
        var sentecnesSeparator = new char[] { '.', '!', '?', ';', ':', '(', ')'};
        var wordsSeparator = new char[] {
                                            '^', '#', '$', '-', '—', '+', '0', '1',
                                            '2', '3', '4', '5', '6', '7', '8', '9', '=',
                                            '\t', '\n', '\r', ',', '…', '“',
                                            '”', '<', '>', '‘', '*', ' ', '/', '\u00A0'};
        var sentencesList = new List<List<string>>();
        var sentences = text.ToLower().Split(sentecnesSeparator, StringSplitOptions.RemoveEmptyEntries);

        foreach (var sentence in sentences)
        {
            var words = sentence.Split(wordsSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 0)
            {
                sentencesList.Add(words.ToList());
            }
        }
        return sentencesList;
    }
}