#ifndef FACTORY_REGISTRY_H
#define FACTORY_REGISTRY_H

#include "CommunicationInterface.h"
#include <functional>
#include <map>
#include <memory>
#include <string>

class FactoryRegistry {
public:
    using FactoryFunc = std::function<CommunicationInterface*()>;

    static FactoryRegistry& Instance() {
        static FactoryRegistry inst;
        return inst;
    }

    void RegisterFactory(const std::string& name, FactoryFunc f) {
        factories[name] = f;
    }

    std::shared_ptr<CommunicationInterface> CreateInterface(const std::string& name) {
        auto it = factories.find(name);
        if (it != factories.end()) return std::shared_ptr<CommunicationInterface>(it->second());
        return nullptr;
    }

    FactoryFunc GetFactory(const std::string& name) {
        auto it = factories.find(name);
        if (it != factories.end()) return it->second;
        return nullptr;
    }

private:
    std::map<std::string, FactoryFunc> factories;
    FactoryRegistry() = default;
};

#endif // FACTORY_REGISTRY_H
