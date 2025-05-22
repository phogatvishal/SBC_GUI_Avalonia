using SBC.WPF.Enums;

namespace SBC.WPF.Interfaces
{
	public interface ISBCInteropService
	{
		void Connect(InterfaceConnection interfaceType, string comPortOrIp, int baudRateOrPort, string protocol);
		void Disconnect(InterfaceConnection interfaceType);
		int GetConnectionStatus(InterfaceConnection interfaceType);
		HWVersion GetHWVersion(InterfaceConnection interfaceType);
		string GetVersionInfo(InterfaceConnection interfaceType, VersionInfo versionType);
		string RunTest(InterfaceConnection interfaceType, Group group, int subTest);
	}
}
