using SBC.WPF.Enums;
using SBC.WPF.Interfaces;
using System;

namespace SBC.WPF.Services
{
	public class SBCInteropCallerService : ISBCInteropService
	{
		private readonly ICInterface _interop;

		public SBCInteropCallerService(ICInterface interop)
		{
			_interop = interop ?? throw new ArgumentNullException(nameof(interop));	
		}

		public void Connect(InterfaceConnection interfaceType, string comPortOrIp, int baudRateOrPort, string protocol)
		{
			//_interop.Connect(interfaceType, comPortOrIp, baudRateOrPort, protocol);
		}

		public void Disconnect(InterfaceConnection interfaceType)
		{
			_interop.Disconnect(interfaceType);
		}

		public int GetConnectionStatus(InterfaceConnection interfaceType)
		{
			return _interop.GetConnectionStatus(interfaceType);
			//return 1;
		}

		public HWVersion GetHWVersion(InterfaceConnection interfaceType)
		{
			_interop.GetHWVersion(interfaceType, out var hwVersion);
			return hwVersion;
		}

		public string GetVersionInfo(InterfaceConnection interfaceType, VersionInfo versionType)
		{
			_interop.GetVersionInfo(interfaceType, versionType, out var version);
			return version;
		}

		public string RunTest(InterfaceConnection interfaceType, Group group, int subTest)
		{
			_interop.RunTest(interfaceType, group, subTest, out var log);
			//var log = "PCIe Gen4 x8 VPX: PASS\n";
			return log;	
		}
	}
}
