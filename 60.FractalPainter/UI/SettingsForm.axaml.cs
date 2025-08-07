using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.PropertyGrid.Controls;

namespace FractalPainting.UI;

public partial class SettingsForm : Window
{
	private PropertyGrid? settingsGrid;
	private Button? closeButton;

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);

		settingsGrid = this.FindNameScope()?.Find<PropertyGrid>("Settings");
		closeButton = this.FindNameScope()?.Find<Button>("CloseButton");
	}

	public SettingsForm(object settings)
	{
		InitializeComponent();
		Title = "Настройки";
		if (settingsGrid != null)
			settingsGrid.SelectedObject = settings;
		if (closeButton != null)
			closeButton.Click += (_, __) => CloseButtonClicked();
	}

	public void CloseButtonClicked()
	{
		Close();
	}
}