using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SBC.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using System.Linq;

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
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error", "An unexpected error occurred while trying to load logs.", ex.ToString(), canRetry: false);
			}
		}

		[RelayCommand]
		public async Task ExportLogsAsync()
		{
			try
			{
				var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
				var mainWindow = lifetime?.MainWindow;
				var activeWindow = lifetime?.Windows.FirstOrDefault(w => w.IsActive);

				if (mainWindow is null)
				{
					await _exceptionHandler.ShowExceptionDialogAsync(
						"Error",
						"An unexpected error occurred while trying to access the window.",
						canRetry: false);
					return;
				}

				var fileType = new FilePickerFileType("Text Files")
				{
					Patterns = new[] { "*.txt" }
				};

				var file = await mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
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
					var logs = CombinedLogs;
					await using var stream = await file.OpenWriteAsync();
					using var writer = new StreamWriter(stream);
					await writer.WriteAsync(logs);
				}
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error", "An unexpected error occurred while exporting the logs.", ex.ToString(), canRetry: false);
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
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error", "An unexpected error occurred.", ex.ToString(), canRetry: false);
			}
		}
	}
}
