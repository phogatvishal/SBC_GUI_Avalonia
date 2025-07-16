#pragma once
#include <string>

std::string LookupCLSIDDLL(const std::string& clsid);
void AddRegistryEntry(const std::string& clsid, const std::string& soPath);
void RemoveRegistryEntry(const std::string& clsid);
