using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Avalonia;
using SBC.WPF.Interfaces;
using System;
using System.Threading.Tasks;

namespace SBC.WPF.Services
{
	public class ExceptionHandlerService : IExceptionHandlerService
	{
		public void RegisterGlobalHandlers()
		{
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
			{
				if (e.ExceptionObject is Exception ex)
				{
					HandleException("Unhandled Exception", ex);
				}
				Environment.Exit(1);
			};

			TaskScheduler.UnobservedTaskException += (s, e) =>
			{
				HandleException("Unobserved Task Exception", e.Exception);
				e.SetObserved();
			};
		}

		public void HandleException(string title, Exception exception)
		{
			Console.WriteLine($"[{title}] {exception.Message}");
			Console.WriteLine(exception.StackTrace);

			// Show a UI dialog using Avalonia dispatcher
			Dispatcher.UIThread.Post(async () =>
			{
				var dialog = new ExceptionDialog(title, exception.Message);
				await dialog.ShowDialog(GetMainWindow());
			});
		}

		private Window? GetMainWindow()
		{
			if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				return desktop.MainWindow;
			}

			return null;
		}
	}
}
