using Avalonia.Controls;

namespace SBC.WPF.Interfaces
{
	public interface IWindowTrackerService
	{
		Window? ActiveConnectionWindow { get; set; }

	}
}
