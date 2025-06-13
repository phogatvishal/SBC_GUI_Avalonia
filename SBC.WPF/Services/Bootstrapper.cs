using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using SBC.WPF.Interfaces;
using SBC.WPF.ViewModels;
using SBC.WPF.Views;
using System;

namespace SBC.WPF.Services
{
	public class Bootstrapper
	{
		private readonly IServiceCollection _services;
		public IServiceProvider ServiceProvider { get; private set; }

		public Bootstrapper()
		{
			_services = new ServiceCollection();
			ConfigureServices(_services);
			ServiceProvider = _services.BuildServiceProvider(); // <- Build early and expose
		}

		public void Build()
		{
			ServiceProvider = _services.BuildServiceProvider();
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<MainWindow>();
			services.AddSingleton<MainWindowViewModel>();
			services.AddTransient<ConnectionSettingsView>();
			services.AddTransient<ConnectionSettingsViewModel>();
			services.AddTransient<APILogView>();
			services.AddTransient<APILogViewModel>();
			services.AddSingleton<ITestLoaderService, TestLoaderService>();
			services.AddSingleton<ISBCInteropService>(provider =>
			{
				var logger = provider.GetRequiredService<ILoggerService>();
				return new SBCInteropCallerService(new CInterface(), logger);
			});
			services.AddTransient<ExceptionDialog>();
			services.AddSingleton<IExceptionHandlerService, ExceptionHandlerService>();
			services.AddSingleton<ILoggerService>(provider => new LoggerService("logs.log", "apilogs.log"));

			services.AddTransient<ConfirmDialog>();
			services.AddTransient<ConfirmDialogViewModel>();
		}

		public void Run(IClassicDesktopStyleApplicationLifetime desktop)
		{
			// App startup
			desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();


#if DEBUG
            desktop.MainWindow.AttachDevTools(); // 🔍 Enables Avalonia DevTools
#endif

            // Register exception handlers (optional, since it can now be done in App.axaml.cs)
            var exceptionHandler = ServiceProvider.GetRequiredService<IExceptionHandlerService>();
			exceptionHandler.RegisterGlobalHandlers();
		}
	}
}
