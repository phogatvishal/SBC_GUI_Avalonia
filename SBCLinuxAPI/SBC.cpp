#include <iostream>
#include <string>
#include "CommunicationInterface.h"
#include "FactoryRegistry.h"
#include "UARTInterface.h"
#include "EthernetInterface.h"

// -------------------------------
// COM-style Entry Points
// -------------------------------
extern "C" bool DllRegisterServer() {
    std::cout << "[SBC] DllRegisterServer called\n";
    return true;
}

extern "C" bool DllUnregisterServer() {
    std::cout << "[SBC] DllUnregisterServer called\n";
    return true;
}

extern "C" bool DllInstall(bool install, const std::string& userFlag) {
    std::cout << "[SBC] DllInstall(" << install << ") userFlag=" << userFlag << "\n";
    return install ? DllRegisterServer() : DllUnregisterServer();
}

extern "C" bool DllCanUnloadNow() {
    std::cout << "[SBC] DllCanUnloadNow called\n";
    return true;
}

extern "C" CommunicationInterface* DllGetClassObject(const std::string& clsid) {
    std::cout << "[SBC] DllGetClassObject='" << clsid << "'\n";
    auto f = FactoryRegistry::Instance().GetFactory(clsid);
    return f ? f() : nullptr;
}

extern "C" __attribute__((visibility("default"))) CommunicationInterface* CreateUARTInterface() {
    std::cout << "[SBC] CreateUARTInterface called — Build: 2025-07-14\n";
    return new UARTInterface();
}

extern "C" HRESULT UART_Connect(CommunicationInterface* obj,
                                const wchar_t* ip, uint32_t port, const wchar_t* extra) {
    InterfaceConnection conn{ip, L"9600-8N1", port};
    return obj->Connect(conn, ip, port, extra);
}

extern "C" HRESULT UART_Disconnect(CommunicationInterface* obj) {
    return obj->Disconnect();
}

extern "C" HRESULT UART_GetConnectionStatus(CommunicationInterface* obj, bool* status) {
    return obj->GetConnectionStatus(*status);
}

extern "C" HRESULT UART_GetHWVersion(CommunicationInterface* obj, HWVersion* ver) {
    return obj->GetHWVersion(ver);
}

extern "C" HRESULT UART_GetVersion(CommunicationInterface* obj, VersionInfo type, uint32_t* v) {
    return obj->GetVersion(type, v);
}

extern "C" HRESULT UART_RunTest(CommunicationInterface* obj,
                                Group group, uint32_t subtest, wchar_t** log) {
    return obj->RunTest(group, subtest, log);
}

extern "C" __attribute__((visibility("default"))) CommunicationInterface* CreateEthernetInterface() {
    std::cout << "[SBC] CreateEthernetInterface called\n";
    return new EthernetInterface();
}

extern "C" HRESULT Ethernet_Connect(CommunicationInterface* obj,
                                    const wchar_t* ip, uint32_t port, const wchar_t* extra) {
    InterfaceConnection conn{ip, L"eth", port};
    return obj->Connect(conn, ip, port, extra);
}

extern "C" HRESULT Ethernet_Disconnect(CommunicationInterface* obj) {
    return obj->Disconnect();
}

extern "C" HRESULT Ethernet_GetConnectionStatus(CommunicationInterface* obj, bool* status) {
    return obj->GetConnectionStatus(*status);
}

extern "C" HRESULT Ethernet_GetHWVersion(CommunicationInterface* obj, HWVersion* ver) {
    return obj->GetHWVersion(ver);
}

extern "C" HRESULT Ethernet_GetVersion(CommunicationInterface* obj, VersionInfo type, uint32_t* v) {
    return obj->GetVersion(type, v);
}

extern "C" HRESULT Ethernet_RunTest(CommunicationInterface* obj,
                                    Group group, uint32_t subtest, wchar_t** log) {
    return obj->RunTest(group, subtest, log);
}

extern "C" void DestroyInterface(CommunicationInterface* obj) {
    delete obj;
}

