#pragma once
#include "CommunicationInterface.h"
#include <memory>



#define EXPORT_API __declspec(dllexport)

//class EthernetInterface :
//  public CommunicationInterface
//{
//private:
//  EthernetConfig* m_EthernetConfig;
//  bool bIsConnected = false;
//
//public:
//  EXPORT_API ~EthernetInterface();
//  [[nodiscard]] EXPORT_API HRESULT Connect(Interface interfaceName, BSTR input1, UINT32 input2, BSTR input3) override;
//  [[nodiscard]] EXPORT_API HRESULT Disconnect() override;
//  [[nodiscard]] EXPORT_API HRESULT GetConnectionStatus(Interface& interfaceName, BSTR& ip, UINT32& port, BSTR& protocol) const override;
//  [[nodiscard]] EXPORT_API HRESULT GetHWVersion(HWVersion& version) const override;
//  [[nodiscard]] EXPORT_API HRESULT GetVersion(VersionInfo enumVal, UINT32& val) const override;
//  [[nodiscard]] EXPORT_API HRESULT RunTest(Group group, UINT32 subTest, BSTR& logOutput) override;
//};

/**
 * @brief Ethernet communication interface implementation.
 */
class EthernetInterface : public CommunicationInterface
{
public:
  /**
   * @brief Default constructor for EthernetInterface.
   */
  EthernetInterface();

  /**
   * @brief Destructor for EthernetInterface.
   */
  virtual ~EthernetInterface();

  /**
   * @brief Establishes a connection with the specified Ethernet interface.
   *
   * @param interfaceName [in] The interface to connect to.
   * @param ip [in] IP address of the Ethernet device.
   * @param port [in] Port number of the Ethernet device.
   * @param protocol [in] Communication protocol (e.g., TCP, UDP).
   * @return [out] HRESULT result of the connection attempt.
   */
  [[nodiscard]] virtual HRESULT Connect(
    const InterfaceConnection interfaceName,
    const BSTR ip,                    
    const UINT32 port,                
    const BSTR protocol            
  ) override;

  /**
   * @brief Disconnects the active Ethernet communication interface.
   *
   * @return [out] HRESULT result of the disconnection attempt.
   */
  [[nodiscard]] virtual HRESULT Disconnect() override;

  /**
   * @brief Retrieves the connection status of the current Ethernet interface.
   *
   * @param connectedInterfaceName [In] Interface name.
   * @param connectionStatus [out] Returns the connection status.
   * @return [out] HRESULT result of the operation.
   */
  [[nodiscard]] virtual HRESULT GetConnectionStatus(
    InterfaceConnection connectedInterfaceName,
    BOOL* connectionStatus
  ) const override;

  /**
   * @brief Retrieves the hardware version of the Ethernet device.
   *
   * @param hwVersion [out] Returns the Hardware Version.
   * @return [out] HRESULT result of the operation.
   */
  [[nodiscard]] virtual HRESULT GetHWVersion(
    HWVersion* hwVersion                
  ) const override;

  /**
   * @brief Retrieves the version information of a specified version type.
   *
   * @param enumVal [in] The version type (enumeration value).
   * @param versionInfo [out] Returns version info.
   * @return [out] HRESULT result of the operation.
   */
  [[nodiscard]] virtual HRESULT GetVersion(
    const VersionInfo enumVal,          
    UINT32* versionInfo                
  ) const override;

  /**
   * @brief Runs a specific test on the Ethernet communication interface.
   *
   * @param group [in] The test group to which the test belongs.
   * @param subTest [in] The specific sub-test to run or we can send multiple tests of the group by ORing them.
   * @param logOutput [out] Returns the log output.
   * @return [out] HRESULT result of the test run.
   */
  [[nodiscard]] virtual HRESULT RunTest(
    const Group group,                 
    UINT32 subTest,                     
    BSTR* logOutput                     
  ) override;

private:
  BSTR m_ip;                    /**< [in] IP address of the Ethernet device */
  UINT32 m_port;                /**< [in] Port number of the Ethernet device */
  BSTR m_protocol;              /**< [in] Communication protocol (e.g., TCP, UDP) */
  bool m_isConnected;           /**< [in] Connection status flag */
  InterfaceConnection m_connectedInterfaceName; /**< [in] Connected interface name */
};


