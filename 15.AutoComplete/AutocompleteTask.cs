using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Autocomplete;

internal class AutocompleteTask
{
	/// <returns>
	/// Возвращает первую фразу словаря, начинающуюся с prefix.
	/// </returns>
	/// <remarks>
	/// Эта функция уже реализована, она заработает, 
	/// как только вы выполните задачу в файле LeftBorderTask
	/// </remarks>
	public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
	{
		var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
		if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
			return phrases[index];
            
		return null;
	}

	/// <returns>
	/// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
	/// элементов словаря, начинающихся с prefix.
	/// </returns>
	/// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
	public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
	{
        var indexLeftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
        var totalCount = GetCountByPrefix(phrases, prefix);

        if (totalCount < count)
            count = totalCount;

        var result = new string[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = phrases[indexLeftBorder + 1 + i];
        }
        return result;
	}

	/// <returns>
	/// Возвращает количество фраз, начинающихся с заданного префикса
	/// </returns>
	public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
	{
        if (string.IsNullOrEmpty(prefix))
        {
            return phrases.Count;
        }
		var indexLeftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
        var indexRightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
        return indexRightBorder - indexLeftBorder - 1;
	}
}

[TestFixture]
public class AutocompleteTests
{
	[Test]
	public void TopByPrefix_IsEmpty_WhenNoPhrases()
	{
        var count = 2;
        var phrases = new List<string>();
        var prefix = "a";
        var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
        CollectionAssert.IsEmpty(actualTopWords);
    }

    [Test]
    public void TopByPrefix_IsEmpty_WhenNoPrefix()
    {
        var count = 0;
        var phrases = new List<string>() { "aa", "ab", "bb" };
        var prefix = "";
        var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
        CollectionAssert.IsEmpty(actualTopWords);
    }

    [Test]
    public void TopByPrefix_ReturnsCorrectPhrases_WhenPrefixIsA()
    {
        var count = 2;
        var phrases = new List<string>() { "aa", "ab", "bb" };
        var prefix = "a";
        var expectedTopWords = new List<string>() { "aa", "ab" };
        var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
        CollectionAssert.AreEqual(expectedTopWords, actualTopWords);
    }

    [Test]
    public void TopByPrefix_ReturnsCorrectPhrases_WhenCountIsGreaterThanNumberOfMatches()
    {
        var count = 3;
        var phrases = new List<string>() { "aa", "ab", "bb" };
        var prefix = "a";
        var expectedTopWords = new List<string>() { "aa", "ab" };
        var actualTopWords = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
        CollectionAssert.AreEqual(expectedTopWords, actualTopWords);
    }

    [Test]
	public void CountByPrefix_ReturnsPhrasesLength_WhenEmptyPrefix()
	{
        var expectedCount = 3;
        var phrases = new List<string>() { "aa", "ab", "bb" };
        var prefix = "";
        var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
        Assert.AreEqual(expectedCount, actualCount);
    }

    [Test]
    public void CountByPrefix_ReturnsZero_WhenEmptyPhrases()
    {
        var expectedCount = 0;
        var phrases = new List<string>();
        var prefix = "a";
        var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
        Assert.AreEqual(expectedCount, actualCount);
    }

    [Test]
    public void CountByPrefix_ReturnsCorrectCount_WhenPrefixIsA()
    {
		var expectedCount = 2;
		var phrases = new List<string>() { "aa", "ab", "bb" };
		var prefix = "a";
		var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
        Assert.AreEqual(expectedCount, actualCount);
    }

    [Test]
    public void CountByPrefix_ReturnsZero_WhenPrefixDoesNotMatchAnyPhrase()
    {
        var expectedCount = 0;
        var phrases = new List<string>() { "aa", "ab", "bb" };
        var prefix = "c";
        var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
        Assert.AreEqual(expectedCount, actualCount);
    }

    [Test]
    public void CountByPrefix_ReturnsZero_WhenPrefixIsLongerThanAnyPhrase()
    {
        var expectedCount = 0;
        var phrases = new List<string>() { "aa", "ab", "bb" };
        var prefix = "abc";
        var actualCount = AutocompleteTask.GetCountByPrefix(phrases, prefix);
        Assert.AreEqual(expectedCount, actualCount);
    }
}