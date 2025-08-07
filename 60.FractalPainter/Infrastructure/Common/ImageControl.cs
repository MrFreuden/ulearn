using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace FractalPainting.Infrastructure.Common;

public class ImageControl : Control
{
	public RenderTargetBitmap? Source;

	public override void Render(DrawingContext context)
	{
		if (Source is not null)
			context.DrawImage(Source, new Rect(0, 0, Width, Height));
		else
			context.FillRectangle(Brushes.Black,  new Rect(0, 0, Width, Height));
	}
}