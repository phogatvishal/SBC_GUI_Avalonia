#pragma once
#include <atomic>

// Global COM runtime ref count
extern std::atomic<int> g_globalRefCount;

inline void IncrementGlobalRef() { g_globalRefCount.fetch_add(1); }
inline void DecrementGlobalRef() { g_globalRefCount.fetch_sub(1); }
