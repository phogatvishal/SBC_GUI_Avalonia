using Avalonia.Controls;
using SBC.WPF.Interfaces;

namespace SBC.WPF.Services
{
	public class WindowTrackerService : IWindowTrackerService
	{
		public Window? ActiveConnectionWindow { get; set; }
	}

}
