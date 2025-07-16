using SBC.WPF.Interfaces;
using System;
using System.Text;
using E = SBC.WPF.Enums;


namespace SBC.WPF.Services
{
	public class LinuxInteropService : ISBCInteropService
	{
		private readonly ISBCInteropService _uart;
		private readonly ISBCInteropService _ethernet;
		private readonly ILoggerService _logger;

		public LinuxInteropService(ISBCInteropService uart, ISBCInteropService ethernet, ILoggerService logger)
		{
			_uart = uart ?? throw new ArgumentNullException(nameof(uart));
			_ethernet = ethernet ?? throw new ArgumentNullException(nameof(ethernet));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		private ISBCInteropService GetDelegate(E.InterfaceConnection type) => type switch
		{
			E.InterfaceConnection.INTERFACE_UART => _uart,
			E.InterfaceConnection.INTERFACE_ETHERNET => _ethernet,
			_ => throw new NotSupportedException($"Unsupported interface: {type}")
		};
		
		public void Connect(E.InterfaceConnection interfaceType, string? comPortOrIp, int baudRateOrPort, string? protocol)
		{
			var impl = GetDelegate((E.InterfaceConnection)interfaceType);
			LogCall(nameof(Connect), $"{interfaceType}, {comPortOrIp}, {baudRateOrPort}, {protocol}");
			impl.Connect(interfaceType, comPortOrIp, baudRateOrPort, protocol);
		}

		public void Disconnect(E.InterfaceConnection interfaceType)
		{
			var impl = GetDelegate((E.InterfaceConnection)interfaceType);
			LogCall(nameof(Disconnect), $"{interfaceType}");
			impl.Disconnect(interfaceType);
		}

		public int GetConnectionStatus(E.InterfaceConnection interfaceType)
		{
			var impl = GetDelegate((E.InterfaceConnection)interfaceType);
			LogCall(nameof(GetConnectionStatus), $"{interfaceType}");
			var result = impl.GetConnectionStatus(interfaceType);
			LogCall(nameof(GetConnectionStatus), $"{interfaceType}", result.ToString());
			return result;
		}

		public E.HWVersion GetHWVersion(E.InterfaceConnection interfaceType)
		{
			var impl = GetDelegate((E.InterfaceConnection)interfaceType);
			LogCall(nameof(GetHWVersion), $"{interfaceType}");
			var result = impl.GetHWVersion(interfaceType);
			LogCall(nameof(GetHWVersion), $"{interfaceType}", result.ToString());
			return result;
		}

		public string GetVersionInfo(E.InterfaceConnection interfaceType, E.VersionInfo versionType)
		{
			var impl = GetDelegate((E.InterfaceConnection)interfaceType);
			LogCall(nameof(GetVersionInfo), $"{interfaceType}, {versionType}");
			var result = impl.GetVersionInfo(interfaceType, versionType);
			LogCall(nameof(GetVersionInfo), $"{interfaceType}, {versionType}", result);
			return result;
		}

		public string RunTest(E.InterfaceConnection interfaceType, E.Group group, int subTest)
		{
			var impl = GetDelegate((E.InterfaceConnection)interfaceType);
			string hex = $"0x{subTest:X}";
			LogCall(nameof(RunTest), $"{interfaceType}, {group}, {hex}");
			var result = impl.RunTest(interfaceType, group, subTest);
			LogCall(nameof(RunTest), $"{interfaceType}, {group}, {hex}", result);
			return result;
		}


		private void LogCall(string methodName, string parameters, string? result = null, Exception? ex = null)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"{methodName}({parameters})");
			if (ex != null)
			{
				sb.AppendLine($"  [ERROR] {ex.Message}");
				sb.AppendLine($"  [STACK] {ex.StackTrace}");
			}
			else if (result != null)
			{
				sb.AppendLine($"Return Value ({methodName}): {result}");
			}
			_logger.APILog(sb.ToString());
		}
	}

	//public class LinuxInteropService : ISBCInteropService
	//{
	//	private readonly ISBCInteropService _interop;
	//	private readonly ILoggerService _logger;

	//	public LinuxInteropService(ISBCInteropService interop, ILoggerService logger) 
	//	{
	//		_interop = interop ?? throw new ArgumentNullException(nameof(interop));
	//		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	//	}

	//	public void Connect(Enums.InterfaceConnection interfaceType, string comPortOrIp, int baudRateOrPort, string protocol)
	//	{
	//		try
	//		{
	//			LogCall(nameof(Connect), $"{interfaceType}, {comPortOrIp}, {baudRateOrPort}, {protocol}");
	//			_interop.Connect(interfaceType, comPortOrIp, baudRateOrPort, protocol);
	//		}
	//		catch (Exception ex)
	//		{
	//			LogCall(nameof(Connect), $"{interfaceType}, {comPortOrIp}, {baudRateOrPort}, {protocol}", ex: ex);
	//			throw;
	//		}
	//	}

	//	public void Disconnect(Enums.InterfaceConnection interfaceType)
	//	{
	//		try
	//		{
	//			LogCall(nameof(Disconnect), $"{interfaceType}");
	//			_interop.Disconnect(interfaceType);
	//		}
	//		catch (Exception ex)
	//		{
	//			LogCall(nameof(Disconnect), $"{interfaceType}", ex: ex);
	//			throw;
	//		}
	//	}

	//	public int GetConnectionStatus(Enums.InterfaceConnection interfaceType)
	//	{
	//		try
	//		{
	//			LogCall(nameof(GetConnectionStatus), $"{interfaceType}");
	//			var result = _interop.GetConnectionStatus(interfaceType);
	//			LogCall(nameof(GetConnectionStatus), $"{interfaceType}", result.ToString());

	//			return result;
	//		}
	//		catch (Exception ex)
	//		{
	//			LogCall(nameof(GetConnectionStatus), $"{interfaceType}", ex: ex);
	//			throw;
	//		}
	//	}
	//	public Enums.HWVersion GetHWVersion(Enums.InterfaceConnection interfaceType)
	//	{
	//		try
	//		{
	//			LogCall(nameof(GetHWVersion), $"{interfaceType}");
	//			var result = _interop.GetHWVersion(interfaceType);
	//			LogCall(nameof(GetHWVersion), $"{interfaceType}", result.ToString());
	//			return result;
	//		}
	//		catch (Exception ex)
	//		{
	//			LogCall(nameof(GetHWVersion), $"{interfaceType}", ex: ex);
	//			throw;
	//		}
	//	}

	//	public string GetVersionInfo(Enums.InterfaceConnection interfaceType, Enums.VersionInfo versionType)
	//	{
	//		try
	//		{
	//			LogCall(nameof(GetVersionInfo), $"{interfaceType}, {versionType}");
	//			var version = _interop.GetVersionInfo(interfaceType, versionType);
	//			LogCall(nameof(GetVersionInfo), $"{interfaceType}, {versionType}", version);
	//			return version;
	//		}
	//		catch (Exception ex)
	//		{
	//			LogCall(nameof(GetVersionInfo), $"{interfaceType}, {versionType}", ex: ex);
	//			throw;
	//		}
	//	}

	//	public string RunTest(Enums.InterfaceConnection interfaceType, Enums.Group group, int subTest)
	//	{
	//		try
	//		{
	//			string hexValue = $"0x{subTest:X}";
	//			LogCall(nameof(RunTest), $"{group}, {hexValue}");
	//			var result = _interop.RunTest(interfaceType, group, subTest);
	//			LogCall(nameof(RunTest), $"{group}, {hexValue}", result);
	//			return result;
	//		}
	//		catch (Exception ex)
	//		{
	//			string hexValue = $"0x{subTest:X}";
	//			LogCall(nameof(RunTest), $"{group}, {hexValue}", ex: ex);
	//			throw;
	//		}
	//	}

	//	private void LogCall(string methodName, string parameters, string? result = null, Exception? ex = null)
	//	{
	//		var logBuilder = new StringBuilder();

	//		if (result == null)
	//		{
	//			logBuilder.AppendLine($"{methodName}({parameters})");
	//		}
	//		if (ex != null)
	//		{
	//			logBuilder.AppendLine($"  [ERROR] {ex.Message}");
	//			logBuilder.AppendLine($"  [STACK] {ex.StackTrace}");
	//		}
	//		else if (result != null)
	//		{
	//			logBuilder.AppendLine($"Return Value ({methodName}): {result}");
	//		}

	//		_logger.APILog(logBuilder.ToString());
	//	}
	//}
}
