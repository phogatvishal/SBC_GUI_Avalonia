using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SBC.WPF.Interfaces;
using SBC.WPF.Services;
using System.Net;
using SBC.WPF.Enums;
using RJCP.IO.Ports;

namespace SBC.WPF.ViewModels
{
	public partial class ConnectionSettingsViewModel : ObservableValidator, INotifyDataErrorInfo
	{
		private readonly MainWindowViewModel _mainWindowViewModel;
		private readonly ISBCInteropService _interopService;
		private readonly IExceptionHandlerService _exceptionHandler;
		private SerialPortStream serialPortStream;

		public ConnectionSettingsViewModel(MainWindowViewModel mainWindowViewModel, ISBCInteropService interopService, IExceptionHandlerService exceptionHandler)
        {
			_mainWindowViewModel = mainWindowViewModel;
			_interopService = interopService;
			_exceptionHandler = exceptionHandler;

			serialPortStream = new SerialPortStream();

			AvailableComPorts = new ObservableCollection<string>(serialPortStream.GetPortNames().OrderBy(name => name));

			Protocols = new ObservableCollection<string> { "TCP", "UDP" };
			AvailableBaudRates = new ObservableCollection<int> 
			{ 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 460800, 921600, 230400 };

			var settings = SettingsService.LoadSettings();

			if (!string.IsNullOrEmpty(settings.SelectedComPort) && AvailableComPorts.Contains(settings.SelectedComPort))
			{
				SelectedComPort = settings.SelectedComPort;
			}

			if (AvailableBaudRates.Contains(settings.SelectedBaudRate))
			{
				SelectedBaudRate = settings.SelectedBaudRate;
			}

			if (!string.IsNullOrEmpty(settings.SelectedProtocol) && Protocols.Contains(settings.SelectedProtocol))
			{
				SelectedProtocol = settings.SelectedProtocol;
			}

			if (!string.IsNullOrEmpty(settings.SelectedIP))
			{
				var ipParts = settings.SelectedIP.Split('.');
				if (ipParts.Length == 4)
				{
					IPPart1 = ipParts[0];
					IPPart2 = ipParts[1];
					IPPart3 = ipParts[2];
					IPPart4 = ipParts[3];
				}
			}

			if (settings.SelectedPort > 0 && settings.SelectedPort <= 65535)
			{
				SelectedPort = settings.SelectedPort;
			}
		}

		[ObservableProperty]
		private ObservableCollection<string> _availableComPorts = new();

		[ObservableProperty]
		private ObservableCollection<int> _availableBaudRates = new();

		[ObservableProperty]
		private IBrush _serialStatusColor = new SolidColorBrush(Colors.Gray);

		[ObservableProperty]
		private string? _selectedComPort;

		[ObservableProperty]
		private int _selectedBaudRate;

		[ObservableProperty]
		private IBrush _ethernetStatusColor = new SolidColorBrush(Colors.Gray);

		[ObservableProperty]
		private string? _iPPart1;

		[ObservableProperty]
		private string? _iPPart2;

		[ObservableProperty]
		private string? _iPPart3;

		[ObservableProperty]
		private string? _iPPart4;

		public string SelectedIP => $"{IPPart1}.{IPPart2}.{IPPart3}.{IPPart4}";

		[ObservableProperty]
		private string? _portInput;

		[ObservableProperty]
		private int? _selectedPort;

		[ObservableProperty]
		private string _selectedPortText;

		[ObservableProperty]
		private string _selectedProtocol;

		[ObservableProperty]
		private ObservableCollection<string> _protocols = new();

		partial void OnSelectedPortTextChanged(string oldValue, string newValue)
		{
			_errors[nameof(SelectedPortText)] = new();

			if (int.TryParse(newValue, out int port))
			{
				if (port is >= 1024 and <= 65535)
				{
					SelectedPort = port;
				}
				else
				{
					SelectedPort = null;
					_errors[nameof(SelectedPortText)].Add("Port must be between 1024 and 65535");
				}
			}
			else
			{
				SelectedPort = null;
				_errors[nameof(SelectedPortText)].Add("Port must be a number");
			}

			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(SelectedPortText)));
			OnPropertyChanged(nameof(HasPortError)); // <-- manually notify
			OnPropertyChanged(nameof(PortErrorMessage));
		}

		[RelayCommand]
		private void ApplySettings()
		{
			_mainWindowViewModel.IsSerialConnected = TestSerialConnection(SelectedComPort, SelectedBaudRate);
			_mainWindowViewModel.IsEthernetConnected = TestEthernetConnection(SelectedIP, SelectedPort, SelectedProtocol);

			// Save current settings
			var settings = new ConnectionSettings {
				SelectedComPort = SelectedComPort,
				SelectedBaudRate = SelectedBaudRate,
				SelectedIP = SelectedIP,
				SelectedPort = SelectedPort,
				SelectedProtocol = SelectedProtocol,
			};

			SettingsService.SaveSettings(settings);

			if (TestSerialConnection(SelectedComPort, SelectedBaudRate))
			{
				_interopService.Connect(InterfaceConnection.INTERFACE_UART, SelectedComPort, SelectedBaudRate, null);
				_mainWindowViewModel.IsSerialConnected = _interopService.GetConnectionStatus(InterfaceConnection.INTERFACE_UART) == 1;
			}
			if (TestEthernetConnection(SelectedIP, SelectedPort, SelectedProtocol))
			{
				_interopService.Connect(InterfaceConnection.INTERFACE_ETHERNET, SelectedIP, 0, SelectedProtocol);
				_mainWindowViewModel.IsEthernetConnected = _interopService.GetConnectionStatus(InterfaceConnection.INTERFACE_ETHERNET) == 1;
			}
			
		}

		[RelayCommand]
		private void TestConnection()
		{
			bool IsSerialResult = false;
			bool IsEthernetResult = false;


			if (TestSerialConnection(SelectedComPort, SelectedBaudRate))
			{
				_interopService.Connect(InterfaceConnection.INTERFACE_UART, SelectedComPort, SelectedBaudRate, null);
				IsSerialResult = _interopService.GetConnectionStatus(InterfaceConnection.INTERFACE_UART) == 1;
			}

			if (TestEthernetConnection(SelectedIP, SelectedPort, SelectedProtocol))
			{
				_interopService.Connect(InterfaceConnection.INTERFACE_UART, SelectedComPort, SelectedBaudRate, SelectedProtocol);
				IsEthernetResult = _interopService.GetConnectionStatus(InterfaceConnection.INTERFACE_ETHERNET) == 1;
			}


			SerialStatusColor = IsSerialResult ? new SolidColorBrush(Avalonia.Media.Color.Parse("#00BF10"))
			   : Brushes.Red;

			EthernetStatusColor = IsEthernetResult ? new SolidColorBrush(Avalonia.Media.Color.Parse("#00BF10"))
				: Brushes.Red;
		}

		private bool TestSerialConnection(string? port, int baudRate)
		{
			// Simulate a serial check (replace with real logic)
			return !string.IsNullOrEmpty(port) && baudRate > 0;	
		}

		private bool TestEthernetConnection(string ip, int? port, string? protocol)
		{
			bool isValidIp = IPAddress.TryParse(ip, out _);
			bool isValidPort = port >= 1024 && port <= 65535;
			bool isValidProtocol = !string.IsNullOrEmpty(protocol);

			return isValidIp && isValidPort && isValidProtocol;
		}

		public void RefreshCOMPorts()
		{
			AvailableComPorts?.Clear();

			foreach (var port in serialPortStream.GetPortNames().OrderBy(name => name))
				AvailableComPorts?.Add(port);

			if (AvailableComPorts.Any())
				SelectedComPort = AvailableComPorts[0];
		}

		private readonly Dictionary<string, List<string>> _errors = new();

		public bool HasErrors => _errors.Any();

		public bool HasPortError =>
								_errors.TryGetValue(nameof(SelectedPortText), out var list) && list?.Count > 0;

		public string? PortErrorMessage =>
								_errors.TryGetValue(nameof(SelectedPortText), out var list) ? list.FirstOrDefault() : null;

		public bool HasIpError =>
								_errors.ContainsKey(nameof(IPPart1)) ||
								_errors.ContainsKey(nameof(IPPart2)) ||
								_errors.ContainsKey(nameof(IPPart3)) ||
								_errors.ContainsKey(nameof(IPPart4));

		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

		partial void OnIPPart1Changed(string? value) => ValidateIpSegment(nameof(IPPart1), value);
		partial void OnIPPart2Changed(string? value) => ValidateIpSegment(nameof(IPPart2), value);
		partial void OnIPPart3Changed(string? value) => ValidateIpSegment(nameof(IPPart3), value);
		partial void OnIPPart4Changed(string? value) => ValidateIpSegment(nameof(IPPart4), value);

		private void ValidateIpSegment(string propertyName, string value)
		{
			if (!_errors.ContainsKey(propertyName))
				_errors[propertyName] = new List<string>();

			_errors[propertyName].Clear();

			if (!int.TryParse(value, out int number) || number < 0 || number > 255)
			{
				_errors[propertyName].Add("0–255 only");
			}

			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
		{
			if (propertyName != null && _errors.ContainsKey(propertyName))
				return _errors[propertyName];

			return Enumerable.Empty<string>();
		}
	}
}
