using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace SBC.WPF.ViewModels
{
	public partial class ConnectionSettingsViewModel : ViewModelBase, INotifyDataErrorInfo
	{
        [ObservableProperty]
		private ObservableCollection<string> _availableComPorts = new();

		[ObservableProperty]
		private ObservableCollection<int> _availableBaudRates = new();

		[ObservableProperty]
		private Brush? _serialStatusColor = new SolidColorBrush(Colors.Gray); 

		[ObservableProperty]
		private string? _selectedComPort;

		[ObservableProperty]
		private int? _selectedBaudRate;

		[ObservableProperty]
		private Brush? _ethernetStatusColor;

		[ObservableProperty]
		private string? _iPPart1;

		[ObservableProperty]
		private string? _iPPart2;

		[ObservableProperty]
		private string? _iPPart3;

		[ObservableProperty]
		private string? _iPPart4;

		public string SelectedIP => $"{IPPart1}.{IPPart2}.{IPPart3}.{IPPart4}";

		private readonly Dictionary<string, List<string>> _errors = new();

		public bool HasErrors => _errors.Any();

		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

		partial void OnIPPart1Changed(string value) => ValidateIpSegment(nameof(IPPart1), value);
		partial void OnIPPart2Changed(string value) => ValidateIpSegment(nameof(IPPart2), value);
		partial void OnIPPart3Changed(string value) => ValidateIpSegment(nameof(IPPart3), value);
		partial void OnIPPart4Changed(string value) => ValidateIpSegment(nameof(IPPart4), value);

		private void ValidateIpSegment(string propertyName, string value)
		{
			if (!_errors.ContainsKey(propertyName))
				_errors[propertyName] = new List<string>();

			_errors[propertyName].Clear();

			if (!int.TryParse(value, out int number) || number < 0 || number > 255)
			{
				_errors[propertyName].Add("Must be a number between 0 and 255");
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
