using Avalonia.Controls;
using System.Reflection;
using System;

namespace SBC.WPF;

public partial class SplashScreenWindow : Window
{
	public string VersionText { get; set; }
	public string CopyrightText { get; set; }

	public SplashScreenWindow()
    {
        InitializeComponent();

		SystemDecorations = SystemDecorations.None;

		var version = Assembly.GetExecutingAssembly().GetName().Version;
		if (version != null)
		{
			VersionText = $"v{version.Major}.{version.Minor}.{version.Build}";
		}
		else
		{
			VersionText = "v1.0.0";
		}

		var year = DateTime.Now.Year;
		CopyrightText = $"copyright @ {year} Logic Fruit Technologies Private Limited";

		DataContext = this;
	}
}