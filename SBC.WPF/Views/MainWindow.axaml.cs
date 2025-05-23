using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using SBC.WPF.ViewModels;
using System.Linq;
using Avalonia.Styling;
using SBC.WPF.Interfaces;
using Avalonia.Logging;
using Avalonia.Threading;

namespace SBC.WPF.Views
{
	public partial class MainWindow : Window
	{
		private WindowState _lastWindowState;
		private bool _isCustomMaximized = false;
		private PixelRect _previousBounds; // Use PixelRect instead of WPF's Rect
		private readonly ILoggerService _logger;

		public MainWindow(MainWindowViewModel mainWindowViewModel, ILoggerService logger)
		{
			InitializeComponent();
			DataContext = mainWindowViewModel;

			_lastWindowState = this.WindowState;

			this.PropertyChanged += (_, e) =>
			{
				if (e.Property == Window.WindowStateProperty)
				{
					if (this.WindowState != _lastWindowState)
					{
						_lastWindowState = this.WindowState;

						if (this.WindowState == WindowState.Maximized)
							UpdateMaximizeIcon("icons_Restore_DownDrawingImage");
						else
							UpdateMaximizeIcon("icons_MaximiseDrawingImage");
					}
				}
			};
			_logger = logger;

			Dispatcher.UIThread.Post(() =>
			{
				LogScrollViewer?.ScrollToEnd();
			});

		}

		private void MaximizeButton_Click(object? sender, RoutedEventArgs e)
		{
			if (_isCustomMaximized)
			{
				// Restore previous size
				this.Position = _previousBounds.Position;
				this.Width = _previousBounds.Width;
				this.Height = _previousBounds.Height;
				UpdateMaximizeIcon("icons_MaximiseDrawingImage");

				_isCustomMaximized = false;
			}
			else
			{
				// Save bounds
				_previousBounds = new PixelRect(Position, new PixelSize((int)Width, (int)Height));

				// Maximize manually to working area (cross-platform safe)
				var screen = Screens.ScreenFromWindow(this);
				if (screen is not null)
				{
					this.Position = screen.WorkingArea.Position;
					this.Width = screen.WorkingArea.Width;
					this.Height = screen.WorkingArea.Height;
				}
				UpdateMaximizeIcon("icons_Restore_DownDrawingImage");

				_isCustomMaximized = true;
			}
		}

		private void UpdateMaximizeIcon(string resourceKey)
		{
			var icon = MaximizeButton
				.GetVisualDescendants()
				.OfType<Image>()
				.FirstOrDefault(img => img.Name == "RestoreIcon");

			if (icon == null)
				return;

			if (Application.Current?.Resources.TryGetResource(resourceKey, ThemeVariant.Default, out var resource) == true
				&& resource is DrawingImage drawingImage)
			{
				icon.Source = drawingImage;
			}
		}

		private void MinimizeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this is Window window)
			{
				window.WindowState = WindowState.Minimized;
			}
		}

		private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (this is Window window)
			{
				window.Close();
			}
		}

		private void TitleBar_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
		{
            if (e.ClickCount == 2)
            {
				ToggleWindowState();
            }
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
			{
				BeginMoveDrag(e);
			}
		}

		private void ToggleWindowState()
		{
			WindowState = (WindowState == WindowState.Maximized)
				? WindowState.Normal
				: WindowState.Maximized;
		}
	}
}