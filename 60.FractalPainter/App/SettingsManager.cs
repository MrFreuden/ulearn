using System;
using FractalPainting.Infrastructure.Common;

namespace FractalPainting.App;

public class SettingsManager
{
	private readonly IObjectSerializer serializer;
	private readonly IBlobStorage storage;
	private readonly string settingsFilename = "app.settings";

	public SettingsManager(IObjectSerializer serializer, IBlobStorage storage)
	{
		this.serializer = serializer;
		this.storage = storage;
	}

	public AppSettings Load()
	{
		try
		{
			var data = storage.Get(settingsFilename);
			if (data == null)
			{
				var defaultSettings = CreateDefaultSettings();
				Save(defaultSettings);
				return defaultSettings;
			}

			return serializer.Deserialize<AppSettings>(data);
		}
		catch (Exception e)
		{
			MessageBox.Show(null, e.Message, "Не удалось загрузить настройки", MessageBox.MessageBoxButtons.Ok);
			return CreateDefaultSettings();
		}
	}

	private static AppSettings CreateDefaultSettings()
	{
		return new AppSettings
		{
			ImageSettings = new ImageSettings()
		};
	}

	public void Save(AppSettings settings)
	{
		storage.Set(settingsFilename, serializer.Serialize(settings));
	}
}