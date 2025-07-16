using SBC.WPF.Enums;
using SBC.WPF.Interfaces;
using System;
using System.Text;

namespace SBC.WPF.Services
{
	public class SBCInteropCallerService : ISBCInteropService
	{
		private readonly ICInterface _interop;
		private readonly ILoggerService _logger;

		public SBCInteropCallerService(ICInterface interop, ILoggerService logger)
		{
			_interop = interop ?? throw new ArgumentNullException(nameof(interop));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		private void LogCall(string methodName, string parameters, string? result = null, Exception? ex = null)
		{
			var logBuilder = new StringBuilder();

			if (result == null)
			{
				logBuilder.AppendLine($"{methodName}({parameters})");
			}
			if (ex != null)
			{
				logBuilder.AppendLine($"  [ERROR] {ex.Message}");
				logBuilder.AppendLine($"  [STACK] {ex.StackTrace}");
			}
			else if (result != null)
			{
				logBuilder.AppendLine($"Return Value ({methodName}): {result}");
			}

			_logger.APILog(logBuilder.ToString());
		}

		public void Connect(Enums.InterfaceConnection interfaceType, string? comPortOrIp, int baudRateOrPort, string? protocol)
		{
			try
			{
				LogCall(nameof(Connect), $"{interfaceType}, {comPortOrIp}, {baudRateOrPort}, {protocol}");
				_interop.Connect((InterfaceConnection)interfaceType, comPortOrIp, baudRateOrPort, protocol);
			}
			catch (Exception ex)
			{
				LogCall(nameof(Connect), $"{interfaceType}, {comPortOrIp}, {baudRateOrPort}, {protocol}", ex: ex);
				throw;
			}
		}

		public void Disconnect(Enums.InterfaceConnection interfaceType)
		{
			try
			{
				LogCall(nameof(Disconnect), $"{interfaceType}");
				_interop.Disconnect((InterfaceConnection)interfaceType);
			}
			catch (Exception ex)
			{
				LogCall(nameof(Disconnect), $"{interfaceType}", ex: ex);
				throw;
			}
		}

		public int GetConnectionStatus(Enums.InterfaceConnection interfaceType)
		{
		    try
            {
                LogCall(nameof(GetConnectionStatus), $"{interfaceType}");
				var result = _interop.GetConnectionStatus((InterfaceConnection)interfaceType);
                LogCall(nameof(GetConnectionStatus), $"{interfaceType}", result.ToString());
                return result;
            }
            catch (Exception ex)
            {
                LogCall(nameof(GetConnectionStatus), $"{interfaceType}", ex: ex);
                throw;
            }
		}

		public Enums.HWVersion GetHWVersion(Enums.InterfaceConnection interfaceType)
		{
			try
			{
				LogCall(nameof(GetHWVersion), $",{interfaceType}");
				_interop.GetHWVersion((InterfaceConnection)interfaceType, out var hwVersion);
				LogCall(nameof(GetHWVersion), $",{interfaceType}", hwVersion.ToString());
				return (Enums.HWVersion)hwVersion;
			}
			catch (Exception ex)
			{
				LogCall(nameof(GetHWVersion), $"{interfaceType}", ex: ex);
				throw;
			}
		}

		public string GetVersionInfo(Enums.InterfaceConnection interfaceType, Enums.VersionInfo versionType)
		{
			try
			{
				LogCall(nameof(GetVersionInfo), $"{interfaceType}, {versionType}");
				_interop.GetVersionInfo((InterfaceConnection)interfaceType, (VersionInfo)versionType, out var version);
				LogCall(nameof(GetVersionInfo), $"{interfaceType}, {versionType}", version);
				return version;
			}
			catch (Exception ex)
			{
				LogCall(nameof(GetVersionInfo), $"{interfaceType}", ex: ex);
				throw;
			}
		}

	    public string RunTest(Enums.InterfaceConnection interfaceType, Enums.Group group, int subTest)
        {
            try
            {
                string hexValue = $"0x{subTest:X}";
                LogCall(nameof(RunTest), $"{group}, {hexValue}");
                _interop.RunTest((InterfaceConnection)interfaceType, (Group)group, subTest, out var log);
                LogCall(nameof(RunTest), $"{group}, {hexValue}", log);
                return log;
            }
            catch (Exception ex)
            {
                string hexValue = $"0x{subTest:X}";
                LogCall(nameof(RunTest), $"{group}, {hexValue}", ex: ex);
                throw;
            }
        }
	}
}
