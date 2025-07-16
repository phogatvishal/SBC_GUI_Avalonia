#pragma once
#include "CommunicationInterface.h"
#include <memory>

#define EXPORT_API __declspec(dllexport)

//class UARTInterface : public CommunicationInterface
//{
//private:
//  std::unique_ptr<UARTConfig> m_uartConfig;
//  bool bIsConnected = false;
//
//public:
//  EXPORT_API ~UARTInterface();
//  [[nodiscard]] EXPORT_API HRESULT Connect(Interface interfaceName, BSTR input1, UINT32 input2, BSTR input3) override;
//  [[nodiscard]] EXPORT_API HRESULT Disconnect() override;
//  [[nodiscard]] EXPORT_API HRESULT GetConnectionStatus(Interface& connectedInterfaceName, BSTR& output1, UINT32& output2, BSTR& output3) const override;
//  [[nodiscard]] EXPORT_API HRESULT GetHWVersion(HWVersion& hwVersion) const override;
//  [[nodiscard]] EXPORT_API HRESULT GetVersion(VersionInfo enumVal, UINT32& versionInfo) const override;
//  [[nodiscard]] EXPORT_API HRESULT RunTest(Group group, UINT32 subTest, BSTR& logOutput) override;
//};


/**
 * @brief UART communication interface implementation.
 *
 * This class provides the methods for connecting, disconnecting, and querying the status
 * of a UART interface. The connection uses COM ports and baud rates to establish communication.
 */
class UARTInterface : public CommunicationInterface
{
public:
  /**
   * @brief Default constructor for the UART interface.
   */
  UARTInterface();

  /**
   * @brief Destructor for the UART interface. Cleans up allocated resources.
   */
  ~UARTInterface();

  /**
   * @brief Establishes a connection with the specified UART interface.
   *
   * @param interfaceName [in] The interface to connect to.
   * @param comPortOrIp [in] The COM port for UART communication (e.g., "COM1").
   * @param baudRateOrPort [in] The baud rate for UART communication (e.g., 9600, 115200).
   * @param protocol [in] Communication protocol, which may not be relevant for UART communication.
   * @return HRESULT indicating the result of the connection attempt (S_OK for success).
   */
  [[nodiscard]] virtual HRESULT Connect(
    const InterfaceConnection interfaceName,
    const BSTR comPortOrIp,
    const UINT32 baudRateOrPort,
    const BSTR protocol
  ) override;

  /**
   * @brief Disconnects the active UART communication interface.
   *
   * Resets all connection details and frees resources.
   *
   * @return HRESULT indicating the result of the disconnection attempt (S_OK for success).
   */
  [[nodiscard]] virtual HRESULT Disconnect() override;

  /**
   * @brief Retrieves the connection status of the current UART interface.
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
   * @brief Retrieves the hardware version of the UART device.
   *
   * @param hwVersion [out] A Returns the Hardware Version.
   * @return HRESULT indicating the result of the operation (S_OK for success).
   */
  [[nodiscard]] virtual HRESULT GetHWVersion(
    HWVersion* hwVersion
  ) const override;

  /**
   * @brief Retrieves version information of the UART interface.
   *
   * @param enumVal [in] The version type.
   * @param versionInfo [out] Returns version info.
   * @return HRESULT indicating the result of the operation (S_OK for success).
   */
  [[nodiscard]] virtual HRESULT GetVersion(
    const VersionInfo enumVal,
    UINT32* versionInfo
  ) const override;

  /**
   * @brief Runs a specific test on the UART communication interface.
   *
   * @param group [in] The test group to which the test belongs.
   * @param subTest [in] The specific sub-test to run or we can send multiple tests of the group by ORing them.
   * @param logOutput [out] Returns the log output.
   * @return HRESULT indicating the result of the test run (S_OK for success).
   */
  [[nodiscard]] virtual HRESULT RunTest(
    const Group group,
    UINT32 subTest,
    BSTR* logOutput
  ) override;

private:
  BSTR m_comPort;           /**< COM port name (e.g., "COM1") */
  UINT32 m_baudRate;        /**< Baud rate for UART communication (e.g., 9600, 115200) */
  bool m_isConnected;       /**< Connection status flag (true if connected, false otherwise) */
  InterfaceConnection m_connectedInterfaceName; /**< Connected interface name */
};
