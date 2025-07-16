#include "pch.h"
#include "EthernetInterface.h"

EthernetInterface::EthernetInterface()
  : m_ip(SysAllocString(L"0.0.0.0")),  // Default IP address (not connected)
  m_port(0),                          // Default port (no connection)
  m_protocol(SysAllocString(L"None")), // Default protocol (none)
  m_isConnected(false),               // Not connected initially
  m_connectedInterfaceName(InterfaceConnection::INTERFACE_UNKNOWN)  // Assuming INTERFACE_UNKOWN is a valid enum for "not connected"
{
}

EthernetInterface::~EthernetInterface()
{
  SysFreeString(m_ip);      // Clean up IP string
  SysFreeString(m_protocol); // Clean up protocol string
}

HRESULT EthernetInterface::Connect(
  const InterfaceConnection interfaceName,
  const BSTR ip,
  const UINT32 port,
  const BSTR protocol)
{
  // Store connection info in member variables
  m_connectedInterfaceName = interfaceName;
  m_ip = SysAllocString(ip);       // Set IP address
  m_port = port;                   // Set port number
  m_protocol = SysAllocString(protocol); // Set protocol
  m_isConnected = true;            // Mark as connected
  return S_OK;
}

HRESULT EthernetInterface::Disconnect()
{
  // Mark the interface as disconnected
  m_isConnected = false;
  m_ip = SysAllocString(L"0.0.0.0");  // Reset IP address
  m_port = 0;                         // Reset port
  m_protocol = SysAllocString(L"None"); // Reset protocol

  return S_OK;
}

//HRESULT EthernetInterface::GetConnectionStatus(
//  InterfaceConnection& connectedInterfaceName,
//  BSTR* ip,
//  UINT32* port,
//  BSTR* protocol) const
//{
//  // Retrieve the stored connection information
//  connectedInterfaceName = m_connectedInterfaceName;
//  *ip = SysAllocString(m_ip);         // Return the stored IP address
//  *port = m_port;                     // Return the stored port number
//  *protocol = SysAllocString(m_protocol); // Return the stored protocol
//  return S_OK;
//}

HRESULT EthernetInterface::GetConnectionStatus(InterfaceConnection connectedInterfaceName, BOOL* connectionStatus) const
{
  *connectionStatus = m_isConnected;
  return S_OK;
}

HRESULT EthernetInterface::GetHWVersion(
  HWVersion* hwVersion) const
{
  *hwVersion = HWVersion::SBC_6U;
  return S_OK;
}

HRESULT EthernetInterface::GetVersion(
  const VersionInfo enumVal,
  UINT32* versionInfo) const
{
  *versionInfo = 1;
  return S_OK;
}

HRESULT EthernetInterface::RunTest(
  const Group group,
  UINT32 subTest,
  BSTR* logOutput)
{
  *logOutput = SysAllocString(L"Test completed successfully");

  return S_OK;
}
