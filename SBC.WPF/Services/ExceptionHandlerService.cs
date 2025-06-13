using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Avalonia;
using SBC.WPF.Interfaces;
using System;
using System.Threading.Tasks;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;

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
				var mainWindow = GetMainWindow();

				if (mainWindow != null)
				{
					await dialog.ShowDialog(mainWindow);
				}
				else
				{
					dialog.Show();
				}
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

		public async Task<bool> ShowMessageAsync(string? title, string message)
		{
			var tcs = new TaskCompletionSource<bool>();

			// Create buttons
			var okButton = new Button
			{
				Content = "OK",
				Width = 80,
				Height = 32,
				Margin = new Thickness(5),
				Background = Brushes.White,
				Foreground = Brushes.Black
			};

			var cancelButton = new Button
			{
				Content = "Cancel",
				Width = 80,
				Height = 32,
				Margin = new Thickness(5),
				Background = Brushes.White,
				Foreground = Brushes.Black
			};

			// Layout for buttons
			var buttonPanel = new StackPanel
			{
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Center,
				Children = { okButton, cancelButton }
			};

			// Main content panel
			var contentPanel = new StackPanel
			{
				Margin = new Thickness(20),
				Spacing = 10,
				Children =
		{
			new TextBlock
			{
				Text = message,
				TextWrapping = TextWrapping.Wrap,
				Foreground = Brushes.Black
			},
			buttonPanel
		}
			};

			// Create dialog window
			var dialog = new Window
			{
				Title = title ?? "Message",
				Width = 350,
				SizeToContent = SizeToContent.Height,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				Background = Brushes.White,
				Foreground = Brushes.Black,
				BorderBrush = new SolidColorBrush(Color.Parse("#999999")),
				BorderThickness = new Thickness(1),
				Content = contentPanel
			};

			// Button click logic
			okButton.Click += (_, _) =>
			{
				tcs.TrySetResult(true);
				dialog.Close();
			};

			cancelButton.Click += (_, _) =>
			{
				tcs.TrySetResult(false);
				dialog.Close();
			};

			// Apply theme from MainWindow if available
			if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
				desktop.MainWindow is { } mainWindow)
			{
				dialog.RequestedThemeVariant = mainWindow.ActualThemeVariant ?? ThemeVariant.Light;
				dialog.Styles.AddRange(mainWindow.Styles);
				await dialog.ShowDialog(mainWindow);
			}
			else
			{
				dialog.Show();
			}

			return await tcs.Task;
		}
	}
}
