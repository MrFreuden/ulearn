using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using FractalPainting.UI;
using ImageController = FractalPainting.Infrastructure.Common.AvaloniaImageController;

namespace FractalPainting.App;

public class ImageSettingsAction : IUiAction
{
	public MenuCategory Category => MenuCategory.Settings;
	public event EventHandler? CanExecuteChanged;

    private readonly ImageSettings _imageSettings;
	private readonly Func<Window> _window;
	private readonly IImageController _imageController;

    public ImageSettingsAction(IImageController imageController, ImageSettings imageSettings, Func<Window> window)
    {
        _imageSettings = imageSettings;
        _window = window;
        _imageController = imageController;
    }

    public string Name => "Изображение...";
	
	public bool CanExecute(object? parameter)
	{
		return true;
	}

	public async void Execute(object? parameter)
	{
        var imageSettings = _imageSettings;
		await new SettingsForm(imageSettings).ShowDialog(_window());
		_imageController.RecreateImage(imageSettings);
	}
}

public class SaveImageAction : IUiAction
{
	public MenuCategory Category => MenuCategory.File;
	public event EventHandler? CanExecuteChanged;

    private readonly Func<Window> _window;
    private readonly IImageController _imageController;

    public SaveImageAction(IImageController imageController, Func<Window> window)
    {
        _window = window;
        _imageController = imageController;
    }

    public string Name => "Сохранить...";

	public bool CanExecute(object? parameter)
	{
		return true;
	}

	public async void Execute(object? settings)
	{
		var topLevel = TopLevel.GetTopLevel(_window());
		if (topLevel is null) return;

		var options = new FilePickerSaveOptions
		{
			Title = "Сохранить изображение",
			SuggestedFileName = "image.bmp",
		};
		var saveFile = await topLevel.StorageProvider.SaveFilePickerAsync(options);
		if (saveFile is not null)
            _imageController.SaveImage(saveFile.Path.AbsolutePath);
	}
}

public class PaletteSettingsAction : IUiAction
{
	public MenuCategory Category => MenuCategory.Settings;
	public event EventHandler? CanExecuteChanged;

    private readonly Func<Window> _window;
    private readonly Palette _palette;

    public PaletteSettingsAction(Palette palette, Func<Window> window)
    {
        _window = window;
        _palette = palette;
    }

    public string Name => "Палитра...";

	public bool CanExecute(object? parameter)
	{
		return true;
	}

	public async void Execute(object? parameter)
	{
		await new SettingsForm(_palette).ShowDialog(_window());
	}
}

public partial class MainWindow : Window
{
	private const int MenuSize = 32;
	// Контролы из авалонии
	private Menu? menu;
	private ImageControl? image;

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);

		menu = this.FindNameScope()?.Find<Menu>("Menu");
		image = this.FindNameScope()?.Find<ImageControl>("Image");
	}

	public MainWindow(IUiAction[] actions, ImageController imageController) 
	{
		InitializeComponent();
		var imageSettings = CreateSettingsManager().Load().ImageSettings;
		ClientSize = new Size(imageSettings.Width, imageSettings.Height + MenuSize);
		menu.ItemsSource = actions.ToMenuItems();
		Title = "Fractal Painter";
		
		imageController.SetControl(image);
		imageController.RecreateImage(imageSettings);
	}

	private static SettingsManager CreateSettingsManager()
	{
		return new SettingsManager(new XmlObjectSerializer(), new FileBlobStorage());
	}
}