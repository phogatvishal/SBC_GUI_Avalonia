namespace SBC.WPF.Interfaces
{
	public interface ISBCInteropService
	{
		void Connect(Enums.InterfaceConnection interfaceType, string? comPortOrIp, int baudRateOrPort, string? protocol);
		void Disconnect(Enums.InterfaceConnection interfaceType);
		int GetConnectionStatus(Enums.InterfaceConnection interfaceType);
		Enums.HWVersion GetHWVersion(Enums.InterfaceConnection interfaceType);
		string GetVersionInfo(Enums.InterfaceConnection interfaceType, Enums.VersionInfo versionType);
		string RunTest(Enums.InterfaceConnection interfaceType, Enums.Group group, int subTest);
	}
}
