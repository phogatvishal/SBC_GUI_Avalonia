#pragma once
#include "devdefs.h"
#include <Windows.h>

#define EXPORT_API __declspec(dllexport)

/**
 * @brief Interface for communication operations.
 */
class CommunicationInterface
{
public:
  virtual ~CommunicationInterface() = default;

  /**
   * @brief Establishes a connection with the specified interface.
   *
   * @param interfaceName [in] The interface to connect to.
   * @param comPortOrIp [in] Additional input parameter (e.g., COM port or IP address).
   * @param baudRateOrPort [in] Baud rate (if serial communication) or port number.
   * @param protocol [in] Communication protocol (e.g., TCP, UDP, etc.).
   * @return [out] HRESULT result of the connection attempt.
   */
  [[nodiscard]] virtual HRESULT Connect(
    const InterfaceConnection interfaceName, 
    const BSTR comPortOrIp,          
    const UINT32 baudRateOrPort,     
    const BSTR protocol              
  ) = 0;

  /**
   * @brief Disconnects the active communication interface.
   *
   * @return [out] HRESULT result of the disconnection attempt.
   */
  [[nodiscard]] virtual HRESULT Disconnect() = 0;

  /**
   * @brief Retrieves the connection status of the current interface.
   *
   * @param connectedInterfaceName [In] Interface name.
   * @param connectionStatus [out] Returns the connection status.
   * @return [out] HRESULT result of the operation.
   */
  [[nodiscard]] virtual HRESULT GetConnectionStatus(
    InterfaceConnection connectedInterfaceName, 
    BOOL* connectionStatus                      
  ) const = 0;

  /**
   * @brief Retrieves the hardware version of the communication device.
   *
   * @param hwVersion [out] Returns the Hardware Version.
   * @return [out] HRESULT result of the operation.
   */
  [[nodiscard]] virtual HRESULT GetHWVersion(
    HWVersion* hwVersion               
  ) const = 0;

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
  ) const = 0;

  /**
   * @brief Runs a specific test on the communication interface.
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
  ) = 0;
};


