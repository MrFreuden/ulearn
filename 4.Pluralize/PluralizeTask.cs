namespace Pluralize;

public static class PluralizeTask
{
	public static string PluralizeRubles(int count)
	{
		string one = "рубль", two = "рубля", five = "рублей";
		var n = count % 100;
		if (n >= 5 && n <= 20)
			return five;
		n %= 10;
		if (n == 1)
			return one;
		if (n >= 2 && n <= 4)
			return two;
		return five;
	}
}