using Newtonsoft.Json;
using System.IO;

namespace SBC.WPF.Services
{
	public class SettingsService
	{
		private const string SettingsFilePath = "AppSettings.json";

		public static void SaveSettings(ConnectionSettings settings)
		{
			var settingsToSave = new ConnectionSettings
			{
				SelectedComPort = settings.SelectedComPort,
				SelectedBaudRate = settings.SelectedBaudRate,
				SelectedIP = settings.SelectedIP,
				SelectedPort = settings.SelectedPort,
				SelectedProtocol = settings.SelectedProtocol,
			};

			string json = JsonConvert.SerializeObject(settingsToSave, Formatting.Indented);
			File.WriteAllText(SettingsFilePath, json);
		}

		public static ConnectionSettings LoadSettings()
		{
			if (File.Exists(SettingsFilePath))
			{
				string json = File.ReadAllText(SettingsFilePath);
				var settings = JsonConvert.DeserializeObject<ConnectionSettings>(json);
				return settings ?? new ConnectionSettings();
			}

			return new ConnectionSettings();
		}
	}

	public class ConnectionSettings
	{
		public string? SelectedComPort { get; set; }
		public int SelectedBaudRate { get; set; }
		public string? SelectedIP { get; set; }
		public int? SelectedPort { get; set; }
		public string? SelectedProtocol { get; set; }
	}
}
