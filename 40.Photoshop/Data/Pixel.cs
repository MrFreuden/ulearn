using System;

namespace MyPhotoshop
{
    public readonly struct Pixel
	{
        public Pixel(double r, double g, double b)
        {
            R = CheckCorrectValue(r);
            G = CheckCorrectValue(g);
            B = CheckCorrectValue(b);
        }

        public double R { get; }
        public double G { get; }
        public double B { get; }

        private static double CheckCorrectValue(double value)
        {
            if (value < 0 || value > 1) throw new ArgumentException();
            return value;
        }

        public static double Trim(double value)
		{
			if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
		}

        public static Pixel operator *(Pixel p1, Pixel p2)
        {
            return new Pixel(
                Trim(p1.R * p2.R), 
                Trim(p1.G * p2.R), 
                Trim(p1.B * p2.R));
        }

        public static Pixel operator *(Pixel p1, double value)
        {
            return new Pixel(
                Trim(p1.R * value), 
                Trim(p1.G * value), 
                Trim(p1.B * value));
        }

        public static Pixel operator *(double value, Pixel p1)
        {
            return p1 * value;
        }

        public static Pixel operator /(Pixel p1, double value)
        {
            return new Pixel(
                Trim(p1.R / value),
                Trim(p1.G / value),
                Trim(p1.B / value));
        }

        public static Pixel operator /(double value, Pixel p1)
        {
            return p1 / value;
        }
    }
}

