using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
	public static void Test(string input, string[] expectedResult)
	{
		var actualResult = FieldsParserTask.ParseLine(input);
		Assert.AreEqual(expectedResult.Length, actualResult.Count);
		for (int i = 0; i < expectedResult.Length; ++i)
		{
			Assert.AreEqual(expectedResult[i], actualResult[i].Value);
		}
	}

    [TestCase("text", new[] { "text" })]
    [TestCase("hello world", new[] { "hello", "world" })]
    [TestCase("hello   world", new[] { "hello", "world" })]
    [TestCase("\"\\\"a\\\"\"", new[] { "\"a\"" })]
    [TestCase("\'\"a\"\'", new[] { "\"a\"" })]
    [TestCase("\"\'a\\\'\"", new[] { "\'a\'" })]
    [TestCase("\'\\\'a\\\'\'", new[] { "\'a\'" })]
    [TestCase("\"text", new[] { "text" })]
    [TestCase("abc 'feq'", new[] { "abc", "feq" })]
    [TestCase("abc'feq'", new[] { "abc", "feq" })]
    [TestCase("'t' e", new[] { "t", "e" })]
    [TestCase("'t'e", new[] { "t", "e" })]
    [TestCase("''e", new[] { "", "e" })]
    [TestCase("'a\\\\'", new[] { "a\\" })]
    [TestCase(" a", new[] { "a" })]
    [TestCase("", new string[0])]
    [TestCase("\' \'", new[] { " " })]
    [TestCase("\'abc ", new[] { "abc " })]
    public static void RunTests(string input, string[] expectedOutput)
    {
        Test(input, expectedOutput);
    }
}

public class FieldsParserTask
{
	public static List<Token> ParseLine(string line)
	{
		var tokens = new List<Token>();
        var index = 0;
        while (index < line.Length)
        {
            if (line[index] == ' ')
            {
                index++;
                continue;
            }
            else if (IsQuote(line[index]))
            {
                tokens.Add(ReadQuotedField(line, index));
            }
            else
            {
                tokens.Add(ReadField(line, index));
            }
            index = tokens[tokens.Count - 1].GetIndexNextToToken();
        }
        return tokens;
	}

        
	private static Token ReadField(string line, int startIndex)
	{
        var index = startIndex;
        while (index < line.Length && (!IsQuote(line[index]) && line[index] != ' '))
        {
            index++;
        }
		return new Token(line.Substring(startIndex, index - startIndex), startIndex, index - startIndex);
	}

    private static bool IsQuote(char c)
    {
        return c == '\'' || c == '\"';
    }

    public static Token ReadQuotedField(string line, int startIndex)
	{
		return QuotedFieldTask.ReadQuotedField(line, startIndex);
	}
}