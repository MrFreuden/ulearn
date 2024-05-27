using NUnit.Framework;
using System.Text;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
    [TestCase("'a'", 0, "a", 3)]
    public void SingleQuotes(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("\"a\"", 0, "a", 3)]
    public void DoubleQuotes(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("abc 'feq'", 4, "feq", 5)]
    [TestCase("abcgh 'feqg' adf", 6, "feqg", 6)]
    public void StartIndexNotZero(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("abcgh \"fe'qg\" adf", 6, "fe'qg", 7)]
    [TestCase("abcgh 'fe\"qg' adf", 6, "fe\"qg", 7)]
    public void ValueHasQuotes(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("text 'value other_text", 5, "value other_text", 17)]
    [TestCase("text \"value other_text", 5, "value other_text", 17)]
    public void QuoteHasNoPaar(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("'a' 'b'", 0, "a", 3)]
    public void TwoFields(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("\"a 'b' 'c' d\" '\"1\" \"2\" \"3\"'", 0, "a 'b' 'c' d", 13)]
    public void TextWithManyQuotesPaar(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("\"\\\"a\\\"\"", 0, "\"a\"", 7)]
    [TestCase("'a \\\"b'", 0, "a \"b", 7)]

    public void TextWithEscapedQuotes(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }
}

public class QuotedFieldTask
{
    public static Token ReadQuotedField(string line, int startIndex)
    {
        var valueBuilder = new StringBuilder();
        var startQuote = line[startIndex];
        var index = startIndex + 1;

        while (index < line.Length && line[index] != startQuote)
        {
            if (line[index] == '\\')
            {
                index++;
            }
            valueBuilder.Append(line[index]);
            index++;
        }

        var length = index < line.Length ? index - startIndex + 1 : index - startIndex;
        return new Token(valueBuilder.ToString(), startIndex, length);
    }
}