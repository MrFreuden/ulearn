namespace Passwords;

public class CaseAlternatorTask
{
    public static List<string> AlternateCharCases(string lowercaseWord)
    {
        var result = new List<string>();
        AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
        return result.ToList();
    }

    static void AlternateCharCases(char[] word, int startIndex, List<string> result)
    {
        if (startIndex == word.Length)
        {
            if (!result.Contains(new string(word)))
            {
                result.Add(new string(word));
            }
            return;
        }
        else
        {
            if (char.IsLetter(word[startIndex]))
            {
                AlternateCharCases(word, startIndex + 1, result);
                word[startIndex] = char.ToUpper(word[startIndex]);
                AlternateCharCases(word, startIndex + 1, result);
                word[startIndex] = char.ToLower(word[startIndex]);
            }
            else
            {
                AlternateCharCases(word, startIndex + 1, result);
            }
        }
    }
}