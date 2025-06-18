using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using SBC.WPF.Interfaces;
using SBC.WPF.Services;
using SBC.WPF.Views;
using System;

namespace SBC.WPF
{
	public partial class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				var splashScreen = new SplashScreenWindow();
				splashScreen.Show();

				var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(2000) };
				timer.Tick += (sender, e) =>
				{
					timer.Stop();
					InitializeMainApp(desktop);
					splashScreen.Close();
				};
				timer.Start();
			}
			base.OnFrameworkInitializationCompleted();
		}

		private void InitializeMainApp(IClassicDesktopStyleApplicationLifetime desktop)
		{
			var bootstrapper = new Bootstrapper();
			bootstrapper.Build();

			var exceptionHandler = bootstrapper.ServiceProvider.GetRequiredService<IExceptionHandlerService>();
			exceptionHandler.RegisterGlobalHandlers();

			desktop.MainWindow = bootstrapper.ServiceProvider.GetRequiredService<MainWindow>();
			desktop.MainWindow.Show();
		}
	}
}