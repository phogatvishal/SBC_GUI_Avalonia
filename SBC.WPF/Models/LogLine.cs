using Avalonia.Media;

namespace SBC.WPF.Models
{
	public class LogLine 
	{
		public string? Message { get; set; } = string.Empty;
		public IBrush Color { get; set; } = Brushes.White;
	}

}
