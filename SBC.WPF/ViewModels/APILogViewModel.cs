using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SBC.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace SBC.WPF.ViewModels
{
	public partial class APILogViewModel : ObservableValidator
	{
		private readonly ILoggerService _logger;
		private readonly IExceptionHandlerService _exceptionHandler;

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(CombinedLogs))]
		private ObservableCollection<string> _logs = new();

		public string CombinedLogs => string.Join(Environment.NewLine, Logs);

		public APILogViewModel(ILoggerService logger, IExceptionHandlerService exceptionHandler)
        {
			_logger = logger;
			_exceptionHandler = exceptionHandler;

			Logs.CollectionChanged += (_, _) => OnPropertyChanged(nameof(CombinedLogs));
			_ = LoadLogsAsync();
		}

		private async Task LoadLogsAsync()
		{
			try
			{
				Logs.Clear();
				var content = await _logger.ExportCurrentAPILogsAsync();

				foreach (var line in content.Split('\n'))
				{
					if (!string.IsNullOrWhiteSpace(line))
						Logs.Add(line.Trim());
				}
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowMessageAsync(null, $"Failed to load Api-Logs");
			}
		}
		
		public async Task ExportLogsAsync(Window parent)
		{
			try
			{
				var fileType = new FilePickerFileType("Text Files")
				{
					Patterns = new[] { "*.txt" }
				};

				var file = await parent.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
				{
					Title = "Save API Logs As",
					SuggestedFileName = "APILogs.txt",
					FileTypeChoices = new List<FilePickerFileType>
					{
						fileType,
						new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
					},
					DefaultExtension = "txt"
				});

				if (file is not null)
				{
					var logs = CombinedLogs; // Or however you access log text
					await using var stream = await file.OpenWriteAsync();
					using var writer = new StreamWriter(stream);
					await writer.WriteAsync(logs);
				}
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowMessageAsync(null, $"Failed to export Api-Logs");
			}
		}


		[RelayCommand]
		private async Task ClearLogs()
		{
			try
			{
				Logs.Clear();
				_logger.ClearCurrentAPILogs();
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowMessageAsync(null, $"Failed to clear Api-Logs, please try again");
			}
		}
	}
}
