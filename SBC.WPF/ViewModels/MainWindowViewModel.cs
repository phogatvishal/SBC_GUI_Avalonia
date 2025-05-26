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

namespace SBC.WPF.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
		private readonly ITestLoaderService _testLoaderService;
		private readonly ISBCInteropService _interopService;
		private readonly IServiceProvider _serviceProvider;
		public ILoggerService _logger;

		[ObservableProperty]
		private bool _isNavCollapsed;

		[ObservableProperty]
		private string? _name;

		[ObservableProperty]
		private TestGroup? _selectedTestGroup;

		[ObservableProperty]
		private ObservableCollection<TestGroup> _testGroups = new();

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

		public MainWindowViewModel(ITestLoaderService testLoaderService, ISBCInteropService interopService, IServiceProvider serviceProvider, ILoggerService logger)
        {
			_testLoaderService = testLoaderService;
			_interopService = interopService;
			_serviceProvider = serviceProvider;

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

				SelectedTestGroup = TestGroups.FirstOrDefault();
			}
		}

		[RelayCommand]
		private async Task RunTestAsync()
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
			Log($"Bit Mask: {bitMask}");

			for (int i = 1; i <= SelectedIteration; i++)
			{
				Log($"[Iteration {i}] Running {selectedTests.Count} test(s) from '{SelectedTestGroup.Name}'...");
				await Task.Delay(10); // Let UI update

				foreach (var test in selectedTests)
				{
					Log($"  - Executing: {test.Name}");
					await Task.Delay(1); // Simulate small delay
				}
			}

			Log("Test execution finished.");

			var result = _interopService.RunTest(
				GetConnectionFromType(SelectedTestGroup.Type),
				GetGroupFromTestType(SelectedTestGroup.Type),
				bitMask);

			Log(result.ToString());
			UpdateTestResults(result.ToString(),  SelectedTestGroup.Testcases);
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

		private string GetLogText()
		{
			return string.Join(Environment.NewLine, LogLines.Select(line => line.Message));
		}

		[RelayCommand]
		private void ClearLogs()
		{
			LogLines.Clear();
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

		private void Log(string message)
		{
			_logger.Log(message);

			IBrush brush = Brushes.White;

			if (message.Contains("FAIL", StringComparison.OrdinalIgnoreCase))
				brush = Brushes.Red;
			else if (message.Contains("PASS", StringComparison.OrdinalIgnoreCase))
				brush = Brushes.LimeGreen;

			LogLines.Add(new LogLine
			{
				Message = message,
				Color = brush
			});
		}
	}
}
