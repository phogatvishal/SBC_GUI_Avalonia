using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using Avalonia.Threading;
using SBC.WPF.Interfaces;
using SBC.WPF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SBC.WPF.Services
{
	public class ExceptionHandlerService : IExceptionHandlerService
	{
		public void RegisterGlobalHandlers()
		{
			AppDomain.CurrentDomain.UnhandledException += (_, e) =>
			{
				if (e.ExceptionObject is Exception ex)
					ShowExceptionDialog("Unhandled Exception", ex, false);

				Environment.Exit(1);
			};

			TaskScheduler.UnobservedTaskException += (_, e) =>
			{
				ShowExceptionDialog("Unobserved Task Exception", e.Exception, false);
				e.SetObserved();
			};
		}

		public void HandleException(string title, Exception ex)
		{
			Console.WriteLine($"[{title}] {ex.Message}\n{ex.StackTrace}");
			ShowExceptionDialog(title, ex, false);
		}

		public Task<ExceptionDialogResult> ShowExceptionDialogAsync(
		string title,
		string message,
		string? details = null,
		bool canRetry = false,
		Window? parentWindow = null) 
		{
			return InternalShowDialog(title, message, details, canRetry, parentWindow);
		}


		private void ShowExceptionDialog(string title, Exception ex, bool canRetry)
		{
			Dispatcher.UIThread.Post(async () =>
			{
				await InternalShowDialog(title, ex.Message, ex.ToString(), canRetry);
			});
		}
		private async Task<ExceptionDialogResult> InternalShowDialog(string title, string message, string? details, bool canRetry, Window? parentWindow = null)
		{
			var dialog = new ExceptionDialog(title, message, details, canRetry);

			var targetWindow = parentWindow
				?? (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
					?.Windows.FirstOrDefault(w => w.IsActive)
				?? (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
					?.MainWindow;

			if (targetWindow is not null)
			{
				dialog.RequestedThemeVariant = targetWindow.ActualThemeVariant ?? ThemeVariant.Light;
				dialog.Styles.AddRange(targetWindow.Styles);
				return await dialog.ShowDialogAsync(targetWindow);
			}
			else
			{
				dialog.Show(); // fallback, not modal
				return ExceptionDialogResult.Ok;
			}
		}


	}
}