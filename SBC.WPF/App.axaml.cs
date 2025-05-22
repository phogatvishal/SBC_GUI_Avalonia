using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SBC.WPF.Interfaces;
using SBC.WPF.Services;

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
				var bootstrapper = new Bootstrapper();
				bootstrapper.Build();

				var exceptionHandler = bootstrapper.ServiceProvider.GetRequiredService<IExceptionHandlerService>();
				exceptionHandler.RegisterGlobalHandlers();

				bootstrapper.Run(desktop);
			}
			base.OnFrameworkInitializationCompleted();
		}
	}
}