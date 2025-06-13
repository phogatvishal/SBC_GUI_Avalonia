using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
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
	public partial class ConnectionSettingsViewModel : ObservableObject, IDataErrorInfo
	{
		private readonly MainWindowViewModel _mainWindowViewModel;
		private readonly ISBCInteropService _interopService;
		private readonly IExceptionHandlerService _exceptionHandler;
		private readonly SerialPortStream serialPortStream;

		public ObservableCollection<string> AvailableComPorts { get; }
		public ObservableCollection<int> AvailableBaudRates { get; }
		public ObservableCollection<string> Protocols { get; }

		[ObservableProperty] private string? selectedComPort;
	    [ObservableProperty] private int selectedBaudRate;
		[ObservableProperty] private string? selectedProtocol;
		[ObservableProperty] private IBrush serialStatusColor = new SolidColorBrush(Colors.Gray);
		[ObservableProperty] private IBrush ethernetStatusColor = new SolidColorBrush(Colors.Gray);

		[ObservableProperty] private string? iPPart1;
		[ObservableProperty] private string? iPPart2;
		[ObservableProperty] private string? iPPart3;
		[ObservableProperty] private string? iPPart4;
		[ObservableProperty] private string? selectedPortText;

		public int? SelectedPort { get; private set; }
		public string SelectedIP => $"{IPPart1}.{IPPart2}.{IPPart3}.{IPPart4}";

		public string Error => null!;

		public string PortError => this[nameof(SelectedPortText)];
		public bool IsPortTextNotEmpty => !string.IsNullOrWhiteSpace(PortError);

		public string IPPart1Error => this[nameof(IPPart1)];
		public string IPPart2Error => this[nameof(IPPart2)];
		public string IPPart3Error => this[nameof(IPPart3)];
		public string IPPart4Error => this[nameof(IPPart4)];

		public bool HasIPPart1Error => !string.IsNullOrWhiteSpace(IPPart1Error);
		public bool HasIPPart2Error => !string.IsNullOrWhiteSpace(IPPart2Error);
		public bool HasIPPart3Error => !string.IsNullOrWhiteSpace(IPPart3Error);
		public bool HasIPPart4Error => !string.IsNullOrWhiteSpace(IPPart4Error);

		public ConnectionSettingsViewModel(MainWindowViewModel mainWindowViewModel, ISBCInteropService interopService, IExceptionHandlerService exceptionHandler)
		{
			_mainWindowViewModel = mainWindowViewModel;
			_interopService = interopService;
			_exceptionHandler = exceptionHandler;

			serialPortStream = new SerialPortStream();
			AvailableComPorts = new ObservableCollection<string>(serialPortStream.GetPortNames().OrderBy(name => name));
			AvailableBaudRates = new ObservableCollection<int> { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 460800, 921600, 230400 };
			Protocols = new ObservableCollection<string> { "TCP", "UDP" };

			var settings = SettingsService.LoadSettings();
			SelectedComPort = AvailableComPorts.Contains(settings.SelectedComPort) ? settings.SelectedComPort : null;
			SelectedBaudRate = AvailableBaudRates.Contains(settings.SelectedBaudRate) ? settings.SelectedBaudRate : 9600;
			SelectedProtocol = Protocols.Contains(settings.SelectedProtocol) ? settings.SelectedProtocol : null;

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

			if (settings.SelectedPort is >= 1024 and <= 65535)
			{
				SelectedPortText = settings.SelectedPort.ToString();
			}
		}

		partial void OnSelectedPortTextChanged(string? value)
		{
			OnPropertyChanged(nameof(PortError));
			OnPropertyChanged(nameof(IsPortTextNotEmpty));

			if (int.TryParse(value, out var port) && port is >= 1024 and <= 65535)
				SelectedPort = port;
			else
				SelectedPort = null;
		}

		partial void OnIPPart1Changed(string? value) => NotifyIPValidation(nameof(IPPart1));
		partial void OnIPPart2Changed(string? value) => NotifyIPValidation(nameof(IPPart2));
		partial void OnIPPart3Changed(string? value) => NotifyIPValidation(nameof(IPPart3));
		partial void OnIPPart4Changed(string? value) => NotifyIPValidation(nameof(IPPart4));

		private void NotifyIPValidation(string propertyName)
		{
			OnPropertyChanged(propertyName + "Error");
			OnPropertyChanged("Has" + propertyName + "Error");
		}

		public string this[string columnName]
		{
			get
			{
				return columnName switch
				{
					nameof(SelectedPortText) =>
						!int.TryParse(SelectedPortText, out var port) ? "Port must be a number" :
						port is < 1024 or > 65535 ? "Port must be between 1024 and 65535" : string.Empty,

					nameof(IPPart1) => ValidateIpSegment(IPPart1),
					nameof(IPPart2) => ValidateIpSegment(IPPart2),
					nameof(IPPart3) => ValidateIpSegment(IPPart3),
					nameof(IPPart4) => ValidateIpSegment(IPPart4),
					_ => string.Empty
				};
			}
		}

		private string ValidateIpSegment(string? part) =>
			!int.TryParse(part, out int num) || num < 0 || num > 255 ? "0–255 only" : string.Empty;

		[RelayCommand]
		private void ApplySettings()
		{
			if (int.TryParse(SelectedPortText, out var parsedPort))
				SelectedPort = parsedPort;

			_mainWindowViewModel.IsSerialConnected = TestSerialConnection(SelectedComPort, SelectedBaudRate);
			_mainWindowViewModel.IsEthernetConnected = TestEthernetConnection(SelectedIP, SelectedPort, SelectedProtocol);

			var settings = new ConnectionSettings
			{
				SelectedComPort = SelectedComPort,
				SelectedBaudRate = SelectedBaudRate,
				SelectedIP = SelectedIP,
				SelectedPort = SelectedPort,
				SelectedProtocol = SelectedProtocol
			};

			SettingsService.SaveSettings(settings);

			if (_mainWindowViewModel.IsSerialConnected)
				_interopService.Connect(InterfaceConnection.INTERFACE_UART, SelectedComPort, SelectedBaudRate, null);

			if (_mainWindowViewModel.IsEthernetConnected)
				_interopService.Connect(InterfaceConnection.INTERFACE_ETHERNET, SelectedIP, 0, SelectedProtocol);
		}

		[RelayCommand]
		private void TestConnection()
		{
			bool serial = TestSerialConnection(SelectedComPort, SelectedBaudRate);
			bool ethernet = TestEthernetConnection(SelectedIP, SelectedPort, SelectedProtocol);

			if (serial)
				_interopService.Connect(InterfaceConnection.INTERFACE_UART, SelectedComPort, SelectedBaudRate, null);

			if (ethernet)
				_interopService.Connect(InterfaceConnection.INTERFACE_ETHERNET, SelectedIP, 0, SelectedProtocol);

			SerialStatusColor = serial ? new SolidColorBrush(Color.Parse("#00BF10")) : Brushes.Red;
			EthernetStatusColor = ethernet ? new SolidColorBrush(Color.Parse("#00BF10")) : Brushes.Red;
		}

		private bool TestSerialConnection(string? port, int baudRate) => !string.IsNullOrEmpty(port) && baudRate > 0;

		private bool TestEthernetConnection(string ip, int? port, string? protocol)
		{
			return IPAddress.TryParse(ip, out _) && port is >= 1024 and <= 65535 && !string.IsNullOrEmpty(protocol);
		}

		public void RefreshCOMPorts()
		{
			AvailableComPorts.Clear();
			foreach (var port in serialPortStream.GetPortNames().OrderBy(name => name))
				AvailableComPorts.Add(port);

			if (AvailableComPorts.Any())
				SelectedComPort = AvailableComPorts.First();
		}
	}
}
