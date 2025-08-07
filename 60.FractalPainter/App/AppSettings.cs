using FractalPainting.Infrastructure.Common;

namespace FractalPainting.App;

public class AppSettings : IImageSettingsProvider
{
	public ImageSettings ImageSettings { get; set; }
}