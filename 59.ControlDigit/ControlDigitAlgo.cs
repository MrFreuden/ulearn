namespace SRP.ControlDigit;

public static class Extensions
{
    public static IEnumerable<int> GetDigitsRightToLeft(this long number)
    {
        while (number > 0)
        {
            yield return (int)(number % 10);
            number /= 10;
        }
    }

    public static IEnumerable<int> GetDigitsLeftToRight(this long number) =>
        number.GetDigitsRightToLeft().Reverse();

    public static int MultiplyByPosition(this int digit, int position) =>
        digit * position;

    public static int ProcessDoubledDigit(this int digit)
    {
        int doubled = digit * 2;
        return doubled > 9 ? doubled - 9 : doubled;
    }

    public static int GetControlDigit(this int sum, int modulus)
    {
        int remainder = sum % modulus;
        return remainder == 0 ? 0 : modulus - remainder;
    }

    public static char GetIsbnControlDigit(this int sum)
    {
        int controlDigit = sum % 11;
        return controlDigit == 10 ? 'X' : (char)('0' + controlDigit);
    }

    public static int SumWithAlternatingFactors(this IEnumerable<int> digits, int factor1, int factor2)
    {
        return digits.Select((d, i) => d * (i % 2 == 0 ? factor1 : factor2)).Sum();
    }
}

public static class LuhnExtensions
{
    public static int ApplyLuhnMultiplier(this int digit, bool isEvenPosition)
    {
        return isEvenPosition ? digit.ProcessDoubledDigit() : digit;
    }

    public static int CalculateLuhnSum(this IEnumerable<int> digits)
    {
        return digits.Select((d, i) => d.ApplyLuhnMultiplier(i % 2 == 0)).Sum();
    }
}

public static class IsbnExtensions
{
    public static int CalculateIsbnSum(this IEnumerable<int> digits)
    {
        var digitList = digits.ToList();

        while (digitList.Count < 9)
            digitList.Insert(0, 0);

        return digitList
            .Take(9)
            .Select((d, i) => d.MultiplyByPosition(i + 1))
            .Sum();
    }
}

public static class UpcExtensions
{
    public static int CalculateUpcSum(this IEnumerable<int> digits)
    {
        return digits.SumWithAlternatingFactors(3, 1);
    }
}

public static class ControlDigitAlgo
{
    public static int Luhn(long number)
    {
        int sum = number.GetDigitsRightToLeft().CalculateLuhnSum();
        return sum.GetControlDigit(10);
    }

    public static char Isbn10(long number)
    {
        int sum = number.GetDigitsLeftToRight().CalculateIsbnSum();
        return sum.GetIsbnControlDigit();
    }

    public static int Upc(long number)
    {
        int sum = number.GetDigitsRightToLeft().CalculateUpcSum();
        return sum.GetControlDigit(10);
    }
}