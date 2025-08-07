using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace FractalPainting.Infrastructure.Common;

public class AvaloniaImageController : IImageController
{
	private ImageControl? control;

	public Size GetImageSize()
	{
		FailIfNotInitialized();
		return control!.Bounds.Size;
	}

	public void UpdateUi()
	{
		FailIfNotInitialized();
		control!.InvalidateVisual();
	}

	public DrawingContext CreateDrawingContext()
	{
		FailIfNotInitialized();
		if (control!.Source != null) return control.Source.CreateDrawingContext();

		var imageSize = GetImageSize();
		var pixelSize = new PixelSize((int)imageSize.Width, (int)imageSize.Height);
		control.Source = new RenderTargetBitmap(pixelSize);

		return control.Source.CreateDrawingContext();
	}

	private void FailIfNotInitialized()
	{
		if (control == null)
			throw new InvalidOperationException(
				$"You must call {nameof(SetControl)} before other method call!");
	}

	public void SetControl(ImageControl? control)
	{
		this.control = control;
	}

	public void RecreateImage(ImageSettings imageSettings)
	{
		FailIfNotInitialized();
		control!.Width = imageSettings.Width;
		control.Height = imageSettings.Height;

		control.Source?.Dispose();
		control.Source = null;
		UpdateUi();
	}

	public void SaveImage(string fileName)
	{
		FailIfNotInitialized();
		control!.Source?.Save(fileName);
	}
	
	public bool HasImage()
	{
		FailIfNotInitialized();
		return control!.Source is not null;
	}
}