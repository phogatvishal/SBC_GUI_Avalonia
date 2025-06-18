
using Avalonia.Controls;
using Avalonia.Interactivity;
using SBC.WPF.Models;
using System;
using System.Threading.Tasks;

namespace SBC.WPF
{
	public partial class ExceptionDialog : Window
	{
		private TaskCompletionSource<ExceptionDialogResult>? _tcs;

		public ExceptionDialog(string title, string message, string? details = null, bool canRetry = false)
		{
			InitializeComponent();

			TitleText.Text = title;
			MessageText.Text = message;

			if (!string.IsNullOrWhiteSpace(details))
			{
				DetailsBlock.Text = details;
				DetailsButton.IsVisible = true;
			}

			RetryButton.IsVisible = canRetry;
			DetailsBlock.IsVisible = false;

			SystemDecorations = SystemDecorations.None;
		}

		public Task<ExceptionDialogResult> ShowDialogAsync(Window parent)
		{
			_tcs = new TaskCompletionSource<ExceptionDialogResult>();
			ShowDialog(parent);
			return _tcs.Task;
		}

		private void RetryButton_Click(object? sender, RoutedEventArgs e) => CloseWithResult(ExceptionDialogResult.Retry);
		private void OkButton_Click(object? sender, RoutedEventArgs e) => CloseWithResult(ExceptionDialogResult.Ok);
		private void DetailsButton_Click(object? sender, RoutedEventArgs e) => DetailsBlock.IsVisible = !DetailsBlock.IsVisible;
		private void CloseButton_Click(object? sender, RoutedEventArgs e) => CloseWithResult(ExceptionDialogResult.Ok);

		private void CloseWithResult(ExceptionDialogResult result)
		{
			_tcs?.TrySetResult(result);
			Close();
		}
	}
}