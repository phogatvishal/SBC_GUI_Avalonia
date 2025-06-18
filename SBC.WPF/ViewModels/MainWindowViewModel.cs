using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SBC.WPF.Interfaces;
using SBC.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using SBC.WPF.Enums;
using SBC.WPF.Services;
using Avalonia.Controls.Platform;

namespace SBC.WPF.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
		private readonly ITestLoaderService _testLoaderService;
		private readonly ISBCInteropService _interopService;
		private readonly IServiceProvider _serviceProvider;
		private readonly IExceptionHandlerService _exceptionHandler;
		private readonly ILoggerService _logger;
		private APILogView apiLogView;

		[ObservableProperty]
		private bool _isNavCollapsed;

		[ObservableProperty]
		private string? _name;

		[ObservableProperty]
		private TestGroup? _selectedTestGroup;

		[ObservableProperty]
		private ObservableCollection<TestGroup>? _testGroups = new();

		public ObservableCollection<LogLine> LogLines { get; } = new();

		[ObservableProperty]
		private int? _selectedIteration = 1;

		[ObservableProperty]
		private bool _isSerialConnected;

		[ObservableProperty]
		private bool _isEthernetConnected;

		[ObservableProperty]
		private string _logDocument = string.Empty;

		[ObservableProperty]
		private bool _isAllSelected;

		public MainWindowViewModel(ITestLoaderService testLoaderService, ISBCInteropService interopService, IServiceProvider serviceProvider,
			ILoggerService logger, IExceptionHandlerService exceptionHandler)
        {
			_testLoaderService = testLoaderService;
			_interopService = interopService;
			_serviceProvider = serviceProvider;
			_exceptionHandler = exceptionHandler;

			_logger = logger;
			
			_ = Initialize();

			_interopService.Connect(
			InterfaceConnection.INTERFACE_ETHERNET, "COM3", 115200, "MyProtocol");
		}

		private async Task Initialize()
		{
			await LoadTestsUI();
			await CheckConnectionStatus();
		}

		private async Task LoadTestsUI()
		{
			string basePath = AppDomain.CurrentDomain.BaseDirectory;
			string jsonPath = Path.Combine(basePath, "Resources", "Data", "Tests.json");

			var groups = await _testLoaderService.LoadTestGroupsAsync(jsonPath);

			TestGroups = new ObservableCollection<TestGroup>(groups);

			foreach (var group in TestGroups)
			{
				foreach (var testCase in group.Testcases)
				{
					testCase.ParentGroup = group;
					testCase.IsSelected = false; // force unchecked
				}
				group.IsAllSelected = false; // force parent unchecked

			}
			SelectedTestGroup = TestGroups.FirstOrDefault();
		}

		[RelayCommand]
		private async Task RunTestAsync()
		{
			try
			{
				if (SelectedTestGroup == null)
				{
					Log("No tab selected.");
					return;
				}

				var selectedTests = SelectedTestGroup.Testcases.Where(t => t.IsSelected).ToList();
				if (!selectedTests.Any())
				{
					Log($"No test cases selected in '{SelectedTestGroup.Name}'");
					return;
				}

				var bitMask = GetTestBitmask(SelectedTestGroup.Name);
				Log($"Bit Mask: {bitMask}{Environment.NewLine}");

				bool retryAttempted = false;

				for (int i = 1; i <= SelectedIteration; i++)
				{
					Log($"[Iteration {i}] Running {selectedTests.Count} test(s) from '{SelectedTestGroup.Name}'...{Environment.NewLine}");
					await Task.Delay(10); // Let UI update

					foreach (var test in selectedTests)
					{
						Log($"  - Executing: {test.Name}{Environment.NewLine}");
						await Task.Delay(1); // Simulate small delay
					}

					Log("Test execution finished.");

					try
					{
						var result = _interopService.RunTest(
							GetConnectionFromType(SelectedTestGroup.Type),
							GetGroupFromTestType(SelectedTestGroup.Type),
							bitMask);

						Log(result.ToString());
						UpdateTestResults(result.ToString(), SelectedTestGroup.Testcases);
					}
					catch (Exception ex)
					{
						var userChoice = await _exceptionHandler.ShowExceptionDialogAsync("Test Execution Error",
							"An error occurred while running the test.", ex.ToString(), canRetry: !retryAttempted); // only show Retry button on first failure

						if (userChoice == ExceptionDialogResult.Retry && !retryAttempted)
						{
							retryAttempted = true;
							i--; // retry current iteration
							continue;
						}

						break; // exit loop on failure or after retry used
					}
				}
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowExceptionDialogAsync("Critical Error",
					"An unexpected error occurred while starting the test.", ex.ToString(), canRetry: false);
			}
		}

		private void UpdateTestResults(string resultData, IEnumerable<TestCase> testCases)
		{
			var lines = resultData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				var parts = line.Split(new[] { ':' }, 2);
				if (parts.Length != 2)
					continue;

				string testName = parts[0].Trim();
				string status = parts[1].Trim().ToUpper();

				var testCase = testCases.FirstOrDefault(tc =>
							   tc.Name?.Replace("_", "").Replace(" ", "").Equals(
							   testName.Replace("_", "").Replace(" ", ""),
							   StringComparison.OrdinalIgnoreCase) == true);

				if (testCase != null)
				{
					testCase.Status = status; // This auto-updates color too
				}
			}
		}

		[RelayCommand]
		public async Task ExportLogsAsync(Window? parentWindow)
		{
			try
			{
				if (parentWindow == null)
				{
					Log("Export failed: Window not available.");
					return;
				}

				var storage = parentWindow.StorageProvider;
				if (storage == null || !storage.CanSave)
				{
					Log("Export failed: File save not supported.");
					return;
				}

				var file = await storage.SaveFilePickerAsync(new FilePickerSaveOptions
				{
					Title = "Save Logs As",
					SuggestedFileName = "Logs.txt",
					DefaultExtension = "txt",
					FileTypeChoices = new[]
					{
				new FilePickerFileType("Text Files") { Patterns = new[] { "*.txt" } },
				new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
			}
				});

				if (file != null)
				{
					await using var stream = await file.OpenWriteAsync();
					using var writer = new StreamWriter(stream);
					await writer.WriteAsync(GetLogText());
					Log($"✅ Logs saved to {file.Name}{Environment.NewLine}{Environment.NewLine}");
				}
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error","An unexpected error occurred while exporting the logs.",ex.ToString(),canRetry: false);
			}
		}

		private string GetLogText()
		{
			return string.Join(Environment.NewLine, LogLines.Select(line => line.Message));
		}

		[RelayCommand]
		private void ClearLogs()
		{
			LogLines.Clear();
			_logger.ClearCurrentLogs();
		}

		private InterfaceConnection GetConnectionFromType(string type)
		{
			if (Enum.TryParse<InterfaceConnection>(type, out var result))
			{
				return result;
			}

			return InterfaceConnection.INTERFACE_UNKNOWN;
		}
		private Group GetGroupFromTestType(string type)
		{
			if (Enum.TryParse<Group>(type, out var result))
			{
				return result;
			}
			return Group.Group_Interface;
		}

		public int GetTestBitmask(string groupType)
		{
			var group = TestGroups.FirstOrDefault(g => g.Name == groupType);
			if (group == null) return 0;

			int bitmask = 0;
			foreach (var test in group.Testcases)
			{
				if (test.IsSelected)
				{
					bitmask |= (1 << test.Bit);
				}
			}

			return bitmask;
		}

		public async Task CheckConnectionStatus()
		{
			try
			{
				//UARTRESULT
				var UARTresult = _interopService.GetConnectionStatus(InterfaceConnection.INTERFACE_UART);

				await Task.Delay(10);

				if (UARTresult == 1)
					IsSerialConnected = true;

				//ETHERNET
				var Ethernetresult = _interopService.GetConnectionStatus(InterfaceConnection.INTERFACE_ETHERNET);

				await Task.Delay(10);

				if (Ethernetresult == 1)
					IsEthernetConnected = true;
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error", "An unexpected error occurred while trying to retrieve the connection status.", ex.ToString(), canRetry: false);
			}
		}

		private void Log(string message)
		{
			_logger.Log(message);

			IBrush brush = Brushes.White;


			LogLines.Add(new LogLine
			{
				Message = message,
				Color = brush
			});
		}

		[RelayCommand]
		private async Task APIHelp()
		{
			try
			{
				string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Docs");
				string chmPath = Path.Combine(basePath, "SBC.chm");
				string pdfPath = Path.Combine(basePath, "SBC.pdf");

				// Windows: Try to open .chm if it exists
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					if (File.Exists(chmPath))
					{
						try
						{
							Process.Start(new ProcessStartInfo
							{
								FileName = chmPath,
								UseShellExecute = true
							});
							return;
						}
						catch (Exception ex)
						{
							await _exceptionHandler.ShowExceptionDialogAsync(
								"Error", "Failed to open CHM help file.", ex.ToString(), canRetry: false);
							return;
						}
					}
				}

				// Fallback to PDF (cross-platform)
				if (File.Exists(pdfPath))
				{
					try
					{
						Process.Start(new ProcessStartInfo
						{
							FileName = pdfPath,
							UseShellExecute = true
						});
					}
					catch (Exception ex)
					{
						await _exceptionHandler.ShowExceptionDialogAsync(
							"Error", "Failed to open PDF help file.", ex.ToString(), canRetry: false);
					}
				}
				else
				{
					await _exceptionHandler.ShowExceptionDialogAsync(
						"Error", "Help file not found (CHM/PDF).", canRetry: false);
				}
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error", "An unexpected error occurred while trying to open API help file.", ex.ToString(), canRetry: false);
			}
		}

		[RelayCommand]
		private async Task OpenConnectionSettings()
		{
			try
			{
				if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
					return;

				var mainWindow = desktop.MainWindow;
				if (mainWindow is null)
					return;

				mainWindow.Effect = new BlurEffect { Radius = 0.5 };

				var connectionSettingsView = _serviceProvider.GetRequiredService<ConnectionSettingsView>();

				if (connectionSettingsView.DataContext is ConnectionSettingsViewModel vm)
				{
					vm.RefreshCOMPorts();
				}
				await connectionSettingsView.ShowDialog(mainWindow);

				mainWindow.Effect = null;
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error", "An unexpected error occurred while trying to open Connection-Settings page.", ex.ToString(), canRetry: false);
			}
		}

		[RelayCommand]
		private void Exit()
		{
			if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
			{
				lifetime.Shutdown();
			}
		}

		[RelayCommand]
		private async Task OpenExportApiLogsAsync()
		{
			try
			{
				if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
					return;

				var mainWindow = desktop.MainWindow;

				mainWindow.Effect = new BlurEffect { Radius = 0.5 };

				var connectionSettingsView = _serviceProvider.GetRequiredService<APILogView>();
				await connectionSettingsView.ShowDialog(mainWindow);

				mainWindow.Effect = null;
			}
			catch (Exception ex)
			{
				await _exceptionHandler.ShowExceptionDialogAsync(
					"Error", "An unexpected error occurred while trying to open API-Logs page.", ex.ToString(), canRetry: false);
			}
		}
	}
}
