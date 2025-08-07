namespace Incapsulation.RationalNumbers;

public class Rational
{
    public readonly int Numerator;
    public readonly int Denominator;

    public Rational(int numerator, int denominator = 1)
    {
        if (denominator == 0)
        {
            Numerator = 0;
            Denominator = 0;
            return;
        }

        var gcd = GCD(Math.Abs(numerator), Math.Abs(denominator));

        Numerator = denominator < 0 ? -numerator / gcd : numerator / gcd;
        Denominator = Math.Abs(denominator) / gcd;
    }

    public bool IsNan => Denominator == 0;

    private int GCD(int a, int b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static Rational operator +(Rational r1, Rational r2)
    {
        if (r1.Denominator == r2.Denominator)
        {
            return new Rational(r1.Numerator + r2.Numerator, r1.Denominator);
        }
        var commonDenominator = r1.Denominator * r2.Denominator;
        var newNumerator = r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator;
        return new Rational(newNumerator, commonDenominator);
    }

    public static Rational operator -(Rational r1, Rational r2)
    {
        if (r1.Denominator == r2.Denominator)
        {
            return new Rational(r1.Numerator - r2.Numerator, r1.Denominator);
        }
        var commonDenominator = r1.Denominator * r2.Denominator;
        var newNumerator = r1.Numerator * r2.Denominator - r2.Numerator * r1.Denominator;
        return new Rational(newNumerator, commonDenominator);
    }

    public static Rational operator *(Rational r1, Rational r2)
    {
        return new Rational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);
    }

    public static Rational operator /(Rational r1, Rational r2)
    {
        return r1 * new Rational(r2.Denominator, r2.Numerator);
    }

    public static implicit operator double(Rational r) => r.IsNan ? double.NaN : (double)r.Numerator / r.Denominator;

    public static explicit operator int(Rational r)
    {
        if (r.Numerator % r.Denominator != 0)
            throw new InvalidCastException("Cannot convert to int.");
        return r.Numerator / r.Denominator;
    }

    public static implicit operator Rational(int number) => new(number);
}