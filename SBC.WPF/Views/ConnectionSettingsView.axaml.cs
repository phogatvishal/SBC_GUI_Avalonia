using Avalonia.Controls;
using SBC.WPF.ViewModels;

namespace SBC.WPF;

public partial class ConnectionSettingsView : Window
{
    public ConnectionSettingsView(ConnectionSettingsViewModel connectionSettingsViewModel)
    {
        InitializeComponent();
        DataContext = connectionSettingsViewModel;

		// Disable native OS borders and title bar
		SystemDecorations = SystemDecorations.None;
	}

	private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
        Close();
	}
}