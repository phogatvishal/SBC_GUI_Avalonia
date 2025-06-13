using Avalonia.Controls;
using Avalonia.Interactivity;
using SBC.WPF.ViewModels;

namespace SBC.WPF;

public partial class ConfirmDialog : Window
{
    private readonly ConfirmDialogViewModel _confirmDialogViewModel;
	private bool _confirmedExit = false;

	public ConfirmDialog(ConfirmDialogViewModel confirmDialogViewModel)
    {
        InitializeComponent();
        _confirmDialogViewModel = confirmDialogViewModel;
		DataContext = _confirmDialogViewModel;
    }

	private void Yes_Click(object? sender, RoutedEventArgs e)
	{
		Close(true);
	}

	private void No_Click(object? sender, RoutedEventArgs e)
	{
		Close(false);
	}

	private void CloseButton_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}
}