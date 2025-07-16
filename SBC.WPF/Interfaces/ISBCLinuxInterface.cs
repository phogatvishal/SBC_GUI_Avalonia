namespace SBC.WPF.Interfaces;

public interface ISBCLinuxInterface
{
	void Connect(Enums.InterfaceConnection interfaceType, string comPortOrIp, int baudRateOrPort, string protocol);
	void Disconnect(Enums.InterfaceConnection interfaceType);
	int GetConnectionStatus(Enums.InterfaceConnection interfaceType);
	void GetHWVersion(Enums.InterfaceConnection interfaceType, out Enums.HWVersion HWVersion);
	string GetVersionInfo(Enums.InterfaceConnection interfaceType, Enums.VersionInfo versionType, out string VersionInfo);
	string RunTest(Enums.InterfaceConnection interfaceType, Enums.Group group, int subTest, out string logOutput);
}
