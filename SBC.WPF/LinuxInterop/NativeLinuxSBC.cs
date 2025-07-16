using System;
using System.Runtime.InteropServices;

namespace SBC.WPF.LinuxInterop
{
	/// <summary>
	/// used as P/Invoke Layer
	/// </summary>
	public static class NativeLinuxSBC
	{
		private const string LIB_NAME = "libsbc.so";

		// UART
		[DllImport(LIB_NAME, EntryPoint = "CreateUARTInterface", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr CreateUARTInterface();

		[DllImport(LIB_NAME, EntryPoint = "UART_Connect", CharSet = CharSet.Unicode)]
		public static extern int UART_Connect(IntPtr obj, string ip, uint port, string extra);

		[DllImport(LIB_NAME, EntryPoint = "UART_Disconnect")]
		public static extern int UART_Disconnect(IntPtr obj);

		[DllImport(LIB_NAME, EntryPoint = "UART_GetConnectionStatus")]
		public static extern int UART_GetConnectionStatus(IntPtr obj, out bool status);

		[DllImport(LIB_NAME, EntryPoint = "UART_GetHWVersion")]
		public static extern int UART_GetHWVersion(IntPtr obj, out HWVersionNative ver);

		[DllImport(LIB_NAME, EntryPoint = "UART_GetVersion")]
		public static extern int UART_GetVersion(IntPtr obj, Enums.VersionInfo type, out uint version);

		[DllImport(LIB_NAME, EntryPoint = "UART_RunTest")]
		public static extern int UART_RunTest(IntPtr obj, Enums.Group group, uint subtest, out IntPtr log);


		// Ethernet
		[DllImport(LIB_NAME, EntryPoint = "CreateEthernetInterface")]
		public static extern IntPtr CreateEthernetInterface();

		[DllImport(LIB_NAME, EntryPoint = "Ethernet_Connect", CharSet = CharSet.Unicode)]
		public static extern int Ethernet_Connect(IntPtr obj, string ip, uint port, string extra);

		[DllImport(LIB_NAME, EntryPoint = "Ethernet_Disconnect")]
		public static extern int Ethernet_Disconnect(IntPtr obj);

		[DllImport(LIB_NAME, EntryPoint = "Ethernet_GetConnectionStatus")]
		public static extern int Ethernet_GetConnectionStatus(IntPtr obj, out bool status);

		[DllImport(LIB_NAME, EntryPoint = "Ethernet_GetHWVersion")]
		public static extern int Ethernet_GetHWVersion(IntPtr obj, out HWVersionNative ver);

		[DllImport(LIB_NAME, EntryPoint = "Ethernet_GetVersion")]
		public static extern int Ethernet_GetVersion(IntPtr obj, Enums.VersionInfo type, out uint version);

		[DllImport(LIB_NAME, EntryPoint = "Ethernet_RunTest")]
		public static extern int Ethernet_RunTest(IntPtr obj, Enums.Group group, uint subtest, out IntPtr log);


		[DllImport(LIB_NAME, EntryPoint = "DestroyInterface")]
		public static extern void DestroyInterface(IntPtr obj);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct HWVersionNative
	{
		public ushort major;
		public ushort minor;
		public ushort patch;
	}

	//public enum HWVersion : int
	//{
	//	SBC_6U = 0
	//}

	//public enum InterfaceConnection : int
	//{
	//	INTERFACE_UART = 0,
	//	INTERFACE_ETHERNET = 1,
	//	INTERFACE_UNKNOWN = 2
	//}

	//public enum Group : int
	//{
	//	Group_Interface = 0,
	//	Group_PBIT = 1,
	//	Group_IPMC = 2
	//}

	//public enum VersionInfo : int
	//{
	//	FPGA_VERSION = 0,
	//	API_VERSION = 1,
	//	GUI_VERSION = 2,
	//	FIRMWARE_VERSION = 3
	//}
}
