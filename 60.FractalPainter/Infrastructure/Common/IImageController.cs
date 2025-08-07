using Avalonia.Media;
using Size = Avalonia.Size;

namespace FractalPainting.Infrastructure.Common;

public interface IImageController
{
	void RecreateImage(ImageSettings imageSettings);
	Size GetImageSize();
	DrawingContext CreateDrawingContext();
	void UpdateUi();
	void SaveImage(string fileName);
	bool HasImage();
}