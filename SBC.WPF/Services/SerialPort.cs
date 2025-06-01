using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBC.WPF.Services
{
	public class SerialPort
	{
		public static string[] GetPortNames()
		{
			try
			{
				using var serialCommKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM");
				if (serialCommKey == null)
					return Array.Empty<string>();

				var portList = new List<string>();
				foreach (var valueName in serialCommKey.GetValueNames())
				{
					var port = serialCommKey.GetValue(valueName) as string;
					if (!string.IsNullOrWhiteSpace(port))
					{
						portList.Add(port);
					}
				}

				return portList.ToArray();
			}
			catch (Exception ex)
			{
				// Log if needed
				Console.WriteLine("Error reading serial ports from registry: " + ex.Message);
				return Array.Empty<string>();
			}
		}
	}
}
