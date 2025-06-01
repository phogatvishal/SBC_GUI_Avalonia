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

		public async Task ShowMessageAsync(string? title, string message)
		{
			Window dialog = null;
			dialog = new Window
			{
				Title = title,
				Width = 250,
				SizeToContent = SizeToContent.Height,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				BorderBrush = new SolidColorBrush(Color.Parse("#999999")),
				BorderThickness = new Thickness(1),
				Content = new StackPanel
				{
					Margin = new Thickness(10),
					Children =
				{
					new TextBlock
					{
						Text = message,
						HorizontalAlignment = HorizontalAlignment.Center,
						TextWrapping = TextWrapping.Wrap,
						Margin = new Thickness(0, 0, 0, 20)
					},
					new Grid
					{
						HorizontalAlignment = HorizontalAlignment.Center,
						ColumnDefinitions = { new ColumnDefinition() },
						Children =
						{
							new Button
							{
								Content = "OK",
								Width = 70,
								Height = 30,
								HorizontalContentAlignment = HorizontalAlignment.Center,
								HorizontalAlignment = HorizontalAlignment.Center,
								Command = new RelayCommand(() => dialog.Close())
							}
						}
					}
				}
				}
			};

			dialog.MinWidth = 300;
			dialog.MaxWidth = 600;

			// Try to copy theme from MainWindow explicitly
			if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
				desktop.MainWindow is { } mainWindow)
			{
				dialog.RequestedThemeVariant = mainWindow.ActualThemeVariant ?? ThemeVariant.Light;
				dialog.Styles.AddRange(mainWindow.Styles);
			}

			var owner = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
			await dialog.ShowDialog(owner);
		}
	}
}
