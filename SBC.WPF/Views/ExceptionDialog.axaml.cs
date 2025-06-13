using Avalonia.Controls;
using Avalonia.Interactivity;

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

	private void CloseButton_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}
}