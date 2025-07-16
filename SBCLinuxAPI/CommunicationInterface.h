#ifndef COMMUNICATION_INTERFACE_H
#define COMMUNICATION_INTERFACE_H

#include <cstdint>
#include <string>

// HRESULT type
typedef int32_t HRESULT;

// Enum for version types
enum class VersionInfo {
    FIRMWARE_VERSION,
    HARDWARE_VERSION,
    BSP_VERSION,
    LIBRARY_VERSION,
    APPLICATION_VERSION
};

// Enum for test groups
enum class Group {
    Group_Interface = 0,
    Group_PBIT,
    Group_IPMC
};

// Hardware version structure
struct HWVersion {
    uint32_t major;
    uint32_t minor;
};

// Interface connection parameters
struct InterfaceConnection {
    std::wstring devicePath;
    std::wstring config;
    uint32_t baudRate;
};

// Abstract base interface
class CommunicationInterface {
public:
    virtual ~CommunicationInterface() = default;

    virtual HRESULT Connect(const InterfaceConnection& conn,
                            const std::wstring& ip,
                            uint32_t port,
                            const std::wstring& extra) = 0;

    virtual HRESULT Disconnect() = 0;
    virtual HRESULT GetConnectionStatus(bool& status) const = 0;
    virtual HRESULT GetHWVersion(HWVersion* version) const = 0;
    virtual HRESULT GetVersion(VersionInfo type, uint32_t* version) const = 0;
    virtual HRESULT RunTest(Group group, uint32_t subtest, wchar_t** log) = 0;
};

#endif // COMMUNICATION_INTERFACE_H
