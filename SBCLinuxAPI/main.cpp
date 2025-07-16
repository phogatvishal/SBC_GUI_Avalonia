#include <iostream>
#include <string>
#include <dlfcn.h>
#include <memory>
#include "CommunicationInterface.h"
#include "devdefs.h"


typedef CommunicationInterface* (*DllGetClassObjectFunc)(const std::string&);
typedef bool (*DllInstallFunc)(bool, const std::string&);

void TestInterface(CommunicationInterface* iface, const std::string& name) {
    if (!iface) {
        std::cerr << "Failed to load interface: " << name << std::endl;
        return;
    }
    std::wcout << L"=== Testing Interface: " << std::wstring(name.begin(), name.end()) << L" ===" << std::endl;
    InterfaceConnection conn{L"/dev/ttyUSB0", L"9600-8N1", 9600};
    iface->Connect(conn, L"user", 1234, L"pass");
    bool status; iface->GetConnectionStatus(status);
    std::cout << "Connection Status: " << (status ? "Connected" : "Disconnected") << std::endl;
    HWVersion ver; iface->GetHWVersion(&ver);
    std::cout << "HW Version: " << ver.major << "." << ver.minor << std::endl;
    uint32_t v; iface->GetVersion(VersionInfo::FIRMWARE_VERSION, &v);
    std::cout << "Firmware Version: " << v << std::endl;
    wchar_t* log = nullptr;
    iface->RunTest(Group::Group_Interface, PCIE_GEN4_X8_VPX, &log);
    std::wcout << L"RunTest Log: " << (log?log:L"<null>") << std::endl;
    iface->Disconnect();
}

int main() {
    std::cout << "=== Interface Test Application ===" << std::endl;
    void* lib = dlopen("./libsbc.so", RTLD_LAZY);
    if(!lib){ std::cerr<<dlerror()<<std::endl;return 1;}
    auto dllInstall = (DllInstallFunc)dlsym(lib, "DllInstall");
    if(dllInstall) dllInstall(true, "Init");
    auto dllGet = (DllGetClassObjectFunc)dlsym(lib, "DllGetClassObject");
    auto uart = std::unique_ptr<CommunicationInterface>(dllGet("UART"));
    TestInterface(uart.get(), "UART");
    auto eth = std::unique_ptr<CommunicationInterface>(dllGet("ETHERNET"));
    TestInterface(eth.get(), "ETHERNET");
    dlclose(lib);
    return 0;
}
