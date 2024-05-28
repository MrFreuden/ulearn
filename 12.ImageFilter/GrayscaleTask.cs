namespace Recognizer;

public static class GrayscaleTask
{
	public static double[,] ToGrayscale(Pixel[,] original)
	{
        var width = original.GetLength(0);
        var height = original.GetLength(1);
        var grayscale = new double[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grayscale[x, y] = ConvertRGBToGrayscale(original[x, y].R, original[x, y].G, original[x, y].B);
            }
        }
        return grayscale;
	}

    private static double ConvertRGBToGrayscale(double R, double G, double B)
    {
        return (0.299 * R + 0.587 * G + 0.114 * B) / 255;
    }
}