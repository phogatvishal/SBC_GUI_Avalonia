using Avalonia.Controls;
using Avalonia.Interactivity;
using SBC.WPF.ViewModels;
using System;

namespace SBC.WPF;

public partial class APILogView : Window
{
	public readonly APILogViewModel _apiLogViewModel;

	public APILogView(APILogViewModel apiLogViewModel)
    {
        InitializeComponent();
        DataContext = apiLogViewModel;
		_apiLogViewModel = apiLogViewModel;

		// Disable native OS borders and title bar
		SystemDecorations = SystemDecorations.None;
	}

	private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		Close();
	}

	protected override void OnClosed(EventArgs e)
	{
		base.OnClosed(e);
	}
}