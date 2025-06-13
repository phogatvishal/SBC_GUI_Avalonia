using SBC.WPF.Enums;

namespace SBC.WPF.Interfaces
{
	public interface ICInterface
	{
		void Connect(InterfaceConnection interfaceType, string? comPortOrIp, int baudRateOrPort, string? protocol);

		void Disconnect( InterfaceConnection interfaceType);

		int GetConnectionStatus(InterfaceConnection interfaceType);

		void GetHWVersion(InterfaceConnection interfaceType, out HWVersion HWVersion);

		void GetVersionInfo(InterfaceConnection interfaceType, VersionInfo versionType, out string VersionInfo);

		void RunTest(InterfaceConnection interfaceType, Group Group, int subTest, out string logOutput);
	}
}
