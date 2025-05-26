using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBC.WPF.Models
{
	public class LogLine
	{
		public string Message { get; set; }
		public IBrush Color { get; set; } = Brushes.White;
	}

}
