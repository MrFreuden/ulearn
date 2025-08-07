using System;

namespace MyPhotoshop
{
    public class Photo
	{
		public readonly int Width;
		public readonly int Height;
		private readonly Pixel[,] _data;
		
		public Photo(int width, int height)
		{
			Width = width;
			Height = height;
			_data = new Pixel[width, height];
		}

		public Pixel this[int x, int y]
		{
			get { return _data[x, y]; }
			set { _data[x, y] = value; }
		}
	}
}