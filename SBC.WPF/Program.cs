﻿using Avalonia;
using System;

namespace SBC.WPF
{
	internal sealed class Program
	{
		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				BuildAvaloniaApp()
					.StartWithClassicDesktopLifetime(args);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp()
		{
			try
			{
				return AppBuilder.Configure<App>()
					.UsePlatformDetect()
					.WithInterFont()
					.LogToTrace();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}	
	}
}
