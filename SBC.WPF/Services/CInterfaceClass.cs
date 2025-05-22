using SBC.WPF.Enums;
using SBC.WPF.Interfaces;

namespace SBC.WPF.Services
{
	public class CInterfaceClass : ICInterface
	{
		public virtual extern void Connect(InterfaceConnection interfaceType, string comPortOrIp, int baudRateOrPort, string protocol);

		public virtual extern void Disconnect(InterfaceConnection interfaceType);

		public new int GetConnectionStatus(InterfaceConnection interfaceType)
		{
			return 1;
		}

		public virtual extern void GetHWVersion(InterfaceConnection interfaceType, out HWVersion HWVersion);

		public virtual extern void GetVersionInfo(InterfaceConnection interfaceType, VersionInfo versionType, out string VersionInfo);

		public void RunTest(InterfaceConnection interfaceType, Group Group, int subTest, out string logOutput)
		{
			logOutput = "Hard coded test result: PASS";
		}
	}
}
