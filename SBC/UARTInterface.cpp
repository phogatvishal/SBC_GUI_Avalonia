#include "pch.h"
#include "UARTInterface.h"

UARTInterface::UARTInterface()
  : m_comPort(nullptr), m_baudRate(9600), m_isConnected(false),
  m_connectedInterfaceName(InterfaceConnection::INTERFACE_UNKNOWN)
{
}

UARTInterface::~UARTInterface()
{
  HRESULT hr = Disconnect();
}

HRESULT UARTInterface::Connect(
  const InterfaceConnection interfaceName,
  const BSTR comPortOrIp,
  const UINT32 baudRateOrPort,
  const BSTR protocol)
{
  if (comPortOrIp == nullptr || baudRateOrPort == 0)
  {
    return E_INVALIDARG; 
  }

  // Save connection details
  m_connectedInterfaceName = interfaceName;
  m_comPort = SysAllocString(comPortOrIp);
  m_baudRate = baudRateOrPort;
  m_isConnected = true;

  return S_OK;
}

HRESULT UARTInterface::Disconnect()
{
  if (m_isConnected)
  {
    SysFreeString(m_comPort);
    m_isConnected = false;
  }
  return S_OK;
}

//HRESULT UARTInterface::GetConnectionStatus(
//  InterfaceConnection& connectedInterfaceName,
//  BSTR* comPortOrIp,
//  UINT32* baudRateOrPort,
//  BSTR* protocol) const
//{
//  if (!m_isConnected)
//  {
//    return E_FAIL; 
//  }
//
//  connectedInterfaceName = m_connectedInterfaceName;
//  *comPortOrIp = SysAllocString(m_comPort); // Return the COM port name
//  *baudRateOrPort = m_baudRate;              // Return the baud rate
//  *protocol = SysAllocString(L"");       // Protocol (hardcoded as UART)
//  return S_OK;
//}

HRESULT UARTInterface::GetConnectionStatus(InterfaceConnection connectedInterfaceName, BOOL* connectionStatus) const
{
  *connectionStatus = m_isConnected;
  return S_OK;
}

HRESULT UARTInterface::GetHWVersion(HWVersion* hwVersion) const
{
  if (!m_isConnected)
  {
    return E_FAIL; // Not connected
  }

  *hwVersion = HWVersion::SBC_6U;
  return S_OK;
}

HRESULT UARTInterface::GetVersion(
  const VersionInfo enumVal,
  UINT32* versionInfo
) const
{
  if (!m_isConnected)
  {
    return E_FAIL;
  }

  *versionInfo = 1;
  return S_OK;
}

HRESULT UARTInterface::RunTest(
  const Group group,
  UINT32 subTest,
  BSTR* logOutput
)
{
  if (!m_isConnected)
  {
    return E_FAIL;
  }

  *logOutput = SysAllocString(L"Test completed successfully");
  return S_OK;
}
