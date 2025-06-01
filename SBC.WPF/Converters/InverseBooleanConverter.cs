using Avalonia.Data.Converters;
using Avalonia;
using System;
using System.Globalization;

namespace SBC.WPF.Converters
{
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is bool b ? !b : AvaloniaProperty.UnsetValue;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is bool b ? !b : AvaloniaProperty.UnsetValue;
	}
}
