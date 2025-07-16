#include "EthernetInterface.h"
#include "FactoryRegistry.h"
#include "win_com_compat.h"
#include <iostream>

// Registry registration
bool ethernet_registered = []() {
    FactoryRegistry::Instance().RegisterFactory("ETHERNET", []() { return new EthernetInterface(); });
    return true;
}();

// Implementation
HRESULT EthernetInterface::Connect(const InterfaceConnection& conn,
                                   const std::wstring& user,
                                   uint32_t port,
                                   const std::wstring& password) {
    std::wcout << L"[ETHERNET] Connect called, user: " << user << L", port: " << port << L"\n";
    return S_OK;
}

HRESULT EthernetInterface::Disconnect() {
    std::wcout << L"[ETHERNET] Disconnect called\n";
    return S_OK;
}

HRESULT EthernetInterface::GetConnectionStatus(bool& status) const {
    status = true;
    std::wcout << L"[ETHERNET] GetConnectionStatus: Connected\n";
    return S_OK;
}

HRESULT EthernetInterface::GetHWVersion(HWVersion* version) const {
    version->major = 1; version->minor = 0;
    std::wcout << L"[ETHERNET] GetHWVersion: " << version->major << L"." << version->minor << L"\n";
    return S_OK;
}

HRESULT EthernetInterface::GetVersion(VersionInfo type, uint32_t* version) const {
    *version = 0x1;
    std::wcout << L"[ETHERNET] GetVersion returning: 0x1\n";
    return S_OK;
}

HRESULT EthernetInterface::RunTest(Group group, uint32_t subtest, wchar_t** log) {
    static const wchar_t* msg = L"Ethernet test log";
    *log = const_cast<wchar_t*>(msg);
    std::wcout << L"[ETHERNET] RunTest(" << static_cast<int>(group) << L", " << subtest << L")\n";
    return S_OK;
}
