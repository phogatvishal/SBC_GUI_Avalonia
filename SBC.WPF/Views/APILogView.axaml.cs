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
	}

	private async void ExportLogsButton_Click(object? sender, RoutedEventArgs e)
	{
		await _apiLogViewModel.ExportLogsAsync(this); 
	}

	private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		Close();
	}

	protected override void OnClosed(EventArgs e)
	{
		base.OnClosed(e);
	}

	private void OnHeaderPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
	{
		if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
		{
			if (this is Window window)
			{
				window.BeginMoveDrag(e);
			}
		}
	}
}