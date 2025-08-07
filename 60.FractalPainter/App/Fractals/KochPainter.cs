using System;
using Avalonia;
using Avalonia.Media;
using FractalPainting.Infrastructure.Common;

namespace FractalPainting.App.Fractals;

public class KochPainter
{
	private readonly IImageController imageController;
	private readonly Palette palette;

	public KochPainter(IImageController imageController, Palette palette)
	{
		this.imageController = imageController;
		this.palette = palette;
	}

	public void Paint()
	{
		using var ctx = imageController.CreateDrawingContext();
		var imageSize = imageController.GetImageSize();
		ctx.FillRectangle(new SolidColorBrush(palette.BackgroundColor),
			new Rect(0, 0, imageSize.Width, imageSize.Height));

		DrawSegment(ctx, 0, imageSize.Height * 0.9f, imageSize.Width, imageSize.Height * 0.9f, true);
		imageController.UpdateUi();
	}

	private void DrawSegment(
		DrawingContext ctx,
		double x0,
		double y0,
		double x1,
		double y1,
		bool primaryColor)
	{
		var len2 = (x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1);
		if (len2 < 4)
		{
			if (y0 < 0 || y1 < 0) return;
			var primaryPen = new Pen(new SolidColorBrush(palette.PrimaryColor));
			var secondaryPen = new Pen(new SolidColorBrush(palette.SecondaryColor));
			ctx.DrawLine(primaryColor ? primaryPen : secondaryPen, new Point(x0, y0), new Point(x1, y1));
		}
		else
		{
			var vx = (x1 - x0) / 3;
			var vy = (y1 - y0) / 3;
			DrawSegment(ctx, x0, y0, x0 + vx, y0 + vy, primaryColor);

			var k = (float)Math.Sqrt(3) / 2f;
			var px = (x0 + x1) / 2 + vy * k;
			var py = (y0 + y1) / 2 - vx * k;

			DrawSegment(ctx, x0 + vx, y0 + vy, px, py, !primaryColor);
			DrawSegment(ctx, px, py, x0 + 2 * vx, y0 + 2 * vy, !primaryColor);
			DrawSegment(ctx, x0 + 2 * vx, y0 + 2 * vy, x1, y1, primaryColor);
		}
	}
}