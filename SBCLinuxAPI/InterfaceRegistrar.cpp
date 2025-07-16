#include "FactoryRegistry.h"
#include "UARTInterface.h"
#include "EthernetInterface.h"

void RegisterAllInterfaces() {
    FactoryRegistry::Instance().RegisterFactory("UART", []() { return new UARTInterface(); });
    FactoryRegistry::Instance().RegisterFactory("ETHERNET", []() { return new EthernetInterface(); });
}
