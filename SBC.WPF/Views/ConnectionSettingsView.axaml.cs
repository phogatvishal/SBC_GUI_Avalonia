using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SBC.WPF.ViewModels;

namespace SBC.WPF;

public partial class ConnectionSettingsView : Window
{
    public ConnectionSettingsView(ConnectionSettingsViewModel connectionSettingsViewModel)
    {
        InitializeComponent();
        DataContext = connectionSettingsViewModel;
    }

	private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
        Close();
	}
}