using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using SBC.WPF.ViewModels;
using System.Linq;
using Avalonia.Styling;
using SBC.WPF.Interfaces;
using SBC.WPF.Models;
using Avalonia.Input;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace SBC.WPF.Views
{
	public partial class MainWindow : Window
	{
		private Avalonia.Controls.WindowState _lastWindowState;
		private bool _isCustomMaximized = false;
		private PixelRect _previousBounds; // Use PixelRect instead of WPF's Rect
		private readonly ILoggerService _logger;
		private readonly MainWindowViewModel _mainWindowViewModel;
		private const int ResizeBorderThickness = 6;
		private WindowEdge? _resizeEdge;
		private readonly IServiceProvider _serviceProvider;

		public MainWindow(MainWindowViewModel mainWindowViewModel, ILoggerService logger, IServiceProvider serviceProvider)
		{
			InitializeComponent();
			_mainWindowViewModel = mainWindowViewModel;
			DataContext = _mainWindowViewModel;

			_lastWindowState = this.WindowState;

			this.PropertyChanged += (_, e) =>
			{
				if (e.Property == Window.WindowStateProperty)
				{
					if (this.WindowState != _lastWindowState)
					{
						_lastWindowState = this.WindowState;

						if (this.WindowState == Avalonia.Controls.WindowState.Maximized)
							UpdateMaximizeIcon("icons_Restore_DownDrawingImage");
						else
							UpdateMaximizeIcon("icons_MaximiseDrawingImage");
					}
				}
			};
			_logger = logger;

			_logger.OnNewLogLine += (line) =>
			{
				LogScrollViewer?.ScrollToEnd();
			};

			var screen = Screens.ScreenFromWindow(this) ?? Screens.Primary;
			Width = screen.WorkingArea.Width * 0.8;
			Height = screen.WorkingArea.Height * 0.8;

			Position = new PixelPoint(
			screen.WorkingArea.X + (int)((screen.WorkingArea.Width - Width) / 2),
			screen.WorkingArea.Y + (int)((screen.WorkingArea.Height - Height) / 2));

			_lastWindowState = this.WindowState;

			// Disable native OS borders and title bar
			SystemDecorations = SystemDecorations.None;
			_serviceProvider = serviceProvider;

			//Closing += OnClosing;
		}

		private void MaximizeButton_Click(object? sender, RoutedEventArgs e)
		{
			WindowState = WindowState == Avalonia.Controls.WindowState.Maximized ? Avalonia.Controls.WindowState.Normal : Avalonia.Controls.WindowState.Maximized;
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

		private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
		{
			if (this is Window window)
			{
				window.WindowState = Avalonia.Controls.WindowState.Minimized;
			}
		}

		private void CloseButton_Click(object? sender, RoutedEventArgs e)
		{
			if (this is Window window)
			{
				window.Close();
			}
		}

		private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
		{
			var point = e.GetPosition(this);
			var edge = GetWindowEdge(point);

			if (e.ClickCount == 2)
			{
				WindowState = WindowState == Avalonia.Controls.WindowState.Maximized ? Avalonia.Controls.WindowState.Normal : Avalonia.Controls.WindowState.Maximized;
				return;
			}

			if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
			{
				if (edge.HasValue)
				{
					BeginResizeDrag(edge.Value, e);
				}
				else
				{
					BeginMoveDrag(e);
				}
			}
		}

		private void TitleBar_PointerMoved(object? sender, PointerEventArgs e)
		{
			var point = e.GetPosition(this);
			var edge = GetWindowEdge(point);
			_resizeEdge = edge;

			Cursor = edge switch
			{
				WindowEdge.North => new Cursor(StandardCursorType.TopSide),
				WindowEdge.South => new Cursor(StandardCursorType.BottomSide),
				WindowEdge.West => new Cursor(StandardCursorType.LeftSide),
				WindowEdge.East => new Cursor(StandardCursorType.RightSide),
				WindowEdge.NorthWest => new Cursor(StandardCursorType.TopLeftCorner),
				WindowEdge.NorthEast => new Cursor(StandardCursorType.TopRightCorner),
				WindowEdge.SouthWest => new Cursor(StandardCursorType.BottomLeftCorner),
				WindowEdge.SouthEast => new Cursor(StandardCursorType.BottomRightCorner),
				_ => new Cursor(StandardCursorType.Arrow)
			};
		}

		private void IsAllSelected_Click(object? sender, RoutedEventArgs e)
		{
			if (sender is CheckBox checkBox && checkBox.DataContext is TestGroup group)
			{
				// Force it to toggle between true and false manually
				group.IsAllSelected = group.IsAllSelected == null ? false : true;

				// prevent Avalonia from toggling to null on click
				e.Handled = true;
			}
		}

		private void Window_PointerMoved(object? sender, Avalonia.Input.PointerPressedEventArgs e)
		{
			if (_resizeEdge.HasValue)
			{
				BeginResizeDrag(_resizeEdge.Value, e);
			}
		}

		private void Window_PointerMoved(object? sender, PointerEventArgs e)
		{
			var point = e.GetPosition(this);
			var edge = GetWindowEdge(point);
			_resizeEdge = edge;

			Cursor = edge switch
			{
				WindowEdge.North => new Cursor(StandardCursorType.TopSide),
				WindowEdge.South => new Cursor(StandardCursorType.BottomSide),
				WindowEdge.West => new Cursor(StandardCursorType.LeftSide),
				WindowEdge.East => new Cursor(StandardCursorType.RightSide),
				WindowEdge.NorthWest => new Cursor(StandardCursorType.TopLeftCorner),
				WindowEdge.NorthEast => new Cursor(StandardCursorType.TopRightCorner),
				WindowEdge.SouthWest => new Cursor(StandardCursorType.BottomLeftCorner),
				WindowEdge.SouthEast => new Cursor(StandardCursorType.BottomRightCorner),
				_ => new Cursor(StandardCursorType.Arrow)
			};
		}

		private WindowEdge? GetWindowEdge(Point point)
		{
			const int thickness = 6;
			var width = Bounds.Width;
			var height = Bounds.Height;

			bool left = point.X <= thickness;
			bool right = point.X >= width - thickness;
			bool top = point.Y <= thickness;
			bool bottom = point.Y >= height - thickness;

			return (top, bottom, left, right) switch
			{
				(true, false, true, false) => WindowEdge.NorthWest,
				(true, false, false, true) => WindowEdge.NorthEast,
				(false, true, true, false) => WindowEdge.SouthWest,
				(false, true, false, true) => WindowEdge.SouthEast,
				(true, false, false, false) => WindowEdge.North,
				(false, true, false, false) => WindowEdge.South,
				(false, false, true, false) => WindowEdge.West,
				(false, false, false, true) => WindowEdge.East,
				_ => null
			};
		}

		private void Window_PointerPressed(object? sender, PointerPressedEventArgs e)
		{
			if (_resizeEdge.HasValue && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
			{
				BeginResizeDrag(_resizeEdge.Value, e);
				e.Handled = true;
			}
		}

		private void HamburgerButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
		}

		private bool _confirmedExit = false;

		private async void Window_Closing(object? sender, Avalonia.Controls.WindowClosingEventArgs e)
		{
			if (_confirmedExit) return;

			//Optional: skip dialog if stored setting says so
				if (AppSettings.DontAskExitConfirm)
				return;

			e.Cancel = true;

			var dialog = _serviceProvider.GetRequiredService<ConfirmDialog>();
			var vm = _serviceProvider.GetRequiredService<ConfirmDialogViewModel>();
			var confirmDialog = new ConfirmDialog(vm);
			var result = await confirmDialog.ShowDialog<bool>(this);

			if (result)
			{
				_confirmedExit = true;
				Close();
			}
		}

		private async void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			if (_confirmedExit) return;

			//Optional: skip dialog if stored setting says so
			if (AppSettings.DontAskExitConfirm)
				return;


			var dialog = _serviceProvider.GetRequiredService<ConfirmDialog>();
			var vm = _serviceProvider.GetRequiredService<ConfirmDialogViewModel>();
			var confirmDialog = new ConfirmDialog(vm);
			var result = await confirmDialog.ShowDialog<bool>(this);

			if (result)
			{
				_confirmedExit = true;
				Close();
			}
		}
	}
}