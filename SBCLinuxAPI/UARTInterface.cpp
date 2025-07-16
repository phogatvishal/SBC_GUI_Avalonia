#include "UARTInterface.h"
#include "FactoryRegistry.h"
#include "devdefs.h"
#include "win_com_compat.h"
#include <iostream>
#include <sstream>
#include <cwchar>

// Register
bool uartRegistered = []() {
    FactoryRegistry::Instance().RegisterFactory("UART", []() { return new UARTInterface(); });
    return true;
}();

UARTInterface::UARTInterface() : m_isConnected(false) {}

HRESULT UARTInterface::Connect(const InterfaceConnection& conn,
                               const std::wstring& ip,
                               uint32_t port,
                               const std::wstring& extra) {
    std::wcout << L"[UART] Connect called, IP=" << ip << L", Port=" << port << L", Extra=" << extra << L"\n";
    m_isConnected = true;
    return S_OK;
}
HRESULT UARTInterface::Disconnect() {
    if (m_isConnected) {
        m_isConnected = false;
        std::wcout << L"[UART] Disconnect called\n";
    }
    return S_OK;
}
HRESULT UARTInterface::GetConnectionStatus(bool& status) const {
    status = m_isConnected;
    std::wcout << L"[UART] GetConnectionStatus=" << (status ? L"Connected" : L"Disconnected") << L"\n";
    return S_OK;
}
HRESULT UARTInterface::GetHWVersion(HWVersion* version) const {
    if (!version || !m_isConnected) return E_FAIL;
    version->major = 1; version->minor = 0;
    std::wcout << L"[UART] GetHWVersion=" << version->major << L"." << version->minor << L"\n";
    return S_OK;
}
HRESULT UARTInterface::GetVersion(VersionInfo type, uint32_t* version) const {
    if (!version || !m_isConnected) return E_FAIL;
    *version = 0x1;
    std::wcout << L"[UART] GetVersion returning 0x1\n";
    return S_OK;
}
HRESULT UARTInterface::RunTest(Group group, uint32_t subtest, wchar_t** log) {
    if (!log || !m_isConnected) return E_FAIL;
    std::wstringstream ss;
    if (group == Group::Group_Interface && (subtest & PCIE_GEN4_X8_VPX)) {
        ss << L"PCIe Gen4 x8 VPX: PASS\n";
    }
    std::wstring wlog = ss.str();
    *log = (wchar_t*)std::malloc((wlog.size()+1)*sizeof(wchar_t));
    std::wmemcpy(*log, wlog.c_str(), wlog.size()+1);
    std::wcout << L"[UART] RunTest returning log:\n" << wlog;
    return S_OK;
}
