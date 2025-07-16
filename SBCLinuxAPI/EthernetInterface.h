#ifndef ETHERNET_INTERFACE_H
#define ETHERNET_INTERFACE_H

#include "CommunicationInterface.h"

class EthernetInterface : public CommunicationInterface {
public:
    EthernetInterface() = default;
    ~EthernetInterface() override = default;

    HRESULT Connect(const InterfaceConnection& conn,
                    const std::wstring& user,
                    uint32_t port,
                    const std::wstring& password) override;

    HRESULT Disconnect() override;
    HRESULT GetConnectionStatus(bool& status) const override;
    HRESULT GetHWVersion(HWVersion* version) const override;
    HRESULT GetVersion(VersionInfo type, uint32_t* version) const override;
    HRESULT RunTest(Group group, uint32_t subtest, wchar_t** log) override;
};

#endif // ETHERNET_INTERFACE_H
