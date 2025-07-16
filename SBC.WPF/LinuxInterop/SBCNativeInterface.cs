using System;
using System.Runtime.InteropServices;
using SBC.WPF.Interfaces;

namespace SBC.WPF.LinuxInterop
{
	public class SBCNativeInterface : ISBCInteropService, IDisposable
	{
		private readonly IntPtr _nativePtr;
		private readonly Enums.InterfaceConnection _type;
		private bool _disposed;

		public SBCNativeInterface(Enums.InterfaceConnection type)
		{
			_type = type;

			_nativePtr = type switch
			{
				Enums.InterfaceConnection.INTERFACE_UART => NativeLinuxSBC.CreateUARTInterface(),
				Enums.InterfaceConnection.INTERFACE_ETHERNET => NativeLinuxSBC.CreateEthernetInterface(),
				_ => throw new NotSupportedException($"Unsupported interface: {type}")
			};

			if (_nativePtr == IntPtr.Zero)
				throw new InvalidOperationException($"Failed to create interface of type: {type}");
		}

		public void Connect(Enums.InterfaceConnection interfaceType, string comPortOrIp, int baudRateOrPort, string protocol)
		{
			int hr = _type switch
			{
				Enums.InterfaceConnection.INTERFACE_UART =>
					NativeLinuxSBC.UART_Connect(_nativePtr, comPortOrIp, (uint)baudRateOrPort, protocol),

				Enums.InterfaceConnection.INTERFACE_ETHERNET =>
					NativeLinuxSBC.Ethernet_Connect(_nativePtr, comPortOrIp, (uint)baudRateOrPort, protocol),

				_ => throw new NotSupportedException()
			};

			if (hr != 0)
				throw new InvalidOperationException($"Connect failed. HRESULT={hr}");
		}

		public void Disconnect(Enums.InterfaceConnection interfaceType)
		{
			int hr = _type switch
			{
				Enums.InterfaceConnection.INTERFACE_UART => NativeLinuxSBC.UART_Disconnect(_nativePtr),
				Enums.InterfaceConnection.INTERFACE_ETHERNET => NativeLinuxSBC.Ethernet_Disconnect(_nativePtr),
				_ => throw new NotSupportedException()
			};

			if (hr != 0)
				throw new InvalidOperationException($"Disconnect failed. HRESULT={hr}");
		}

		public int GetConnectionStatus(Enums.InterfaceConnection interfaceType)
		{
			bool status;

			int hr = _type switch
			{
				Enums.InterfaceConnection.INTERFACE_UART => NativeLinuxSBC.UART_GetConnectionStatus(_nativePtr, out status),
				Enums.InterfaceConnection.INTERFACE_ETHERNET => NativeLinuxSBC.Ethernet_GetConnectionStatus(_nativePtr, out status),
				_ => throw new NotSupportedException()
			};

			if (hr != 0)
				throw new InvalidOperationException($"GetConnectionStatus failed. HRESULT={hr}");

			return status ? 1 : 0;
		}

		public Enums.HWVersion GetHWVersion(Enums.InterfaceConnection interfaceType)
		{
			HWVersionNative native;

			int hr = _type switch
			{
				Enums.InterfaceConnection.INTERFACE_UART => NativeLinuxSBC.UART_GetHWVersion(_nativePtr, out native),
				Enums.InterfaceConnection.INTERFACE_ETHERNET => NativeLinuxSBC.Ethernet_GetHWVersion(_nativePtr, out native),
				_ => throw new NotSupportedException()
			};

			if (hr != 0)
				throw new InvalidOperationException($"GetHWVersion failed. HRESULT={hr}");

			// Assuming mapping is based on major
			return (Enums.HWVersion)native.major;
		}

		public string GetVersionInfo(Enums.InterfaceConnection interfaceType, Enums.VersionInfo versionType)
		{
			uint ver;

			int hr = _type switch
			{
				Enums.InterfaceConnection.INTERFACE_UART => NativeLinuxSBC.UART_GetVersion(_nativePtr, versionType, out ver),
				Enums.InterfaceConnection.INTERFACE_ETHERNET => NativeLinuxSBC.Ethernet_GetVersion(_nativePtr, versionType, out ver),
				_ => throw new NotSupportedException()
			};

			if (hr != 0)
				throw new InvalidOperationException($"GetVersionInfo failed. HRESULT={hr}");

			return ver.ToString();
		}

		public string RunTest(Enums.InterfaceConnection interfaceType, Enums.Group group, int subTest)
		{
			IntPtr logPtr;

			int hr = _type switch
			{
				Enums.InterfaceConnection.INTERFACE_UART => NativeLinuxSBC.UART_RunTest(_nativePtr, group, (uint)subTest, out logPtr),
				Enums.InterfaceConnection.INTERFACE_ETHERNET => NativeLinuxSBC.Ethernet_RunTest(_nativePtr, group, (uint)subTest, out logPtr),
				_ => throw new NotSupportedException()
			};

			if (hr != 0)
				throw new InvalidOperationException($"RunTest failed. HRESULT={hr}");

			string logOutput = Marshal.PtrToStringUni(logPtr)!;

			// Marshal.FreeCoTaskMem(logPtr); // Uncomment if C++ uses CoTaskMemAlloc

			return logOutput;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				NativeLinuxSBC.DestroyInterface(_nativePtr);
				_disposed = true;
			}
		}
	}
}
