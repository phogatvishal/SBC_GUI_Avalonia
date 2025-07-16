#ifndef UART_INTERFACE_H
#define UART_INTERFACE_H

#include "CommunicationInterface.h"

class UARTInterface : public CommunicationInterface {
public:
    UARTInterface();
    ~UARTInterface() override = default;

    HRESULT Connect(const InterfaceConnection& conn,
                    const std::wstring& ip,
                    uint32_t port,
                    const std::wstring& extra) override;

    HRESULT Disconnect() override;
    HRESULT GetConnectionStatus(bool& status) const override;
    HRESULT GetHWVersion(HWVersion* version) const override;
    HRESULT GetVersion(VersionInfo type, uint32_t* version) const override;
    HRESULT RunTest(Group group, uint32_t subtest, wchar_t** log) override;

private:
    bool m_isConnected;
};

#endif // UART_INTERFACE_H
