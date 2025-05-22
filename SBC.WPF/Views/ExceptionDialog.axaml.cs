using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SBC.WPF;

public partial class ExceptionDialog : Window
{
	public string TitleText { get; set; }
	public string Message { get; set; }

	public ExceptionDialog(string title, string message)
	{
		InitializeComponent();
		TitleText = title;
		Message = message;
		DataContext = this;
	}

	private void OnOkClick(object? sender, RoutedEventArgs e)
	{
		this.Close();
	}
}