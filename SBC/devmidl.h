#pragma once


#ifdef __midl
#define DEV_MIDL_ENUM [v1_enum]  // For MIDL, use [v1_enum] attribute for versioned enum.
#else
#define DEV_MIDL_ENUM enum  // For C++, use enum class for type safety.
#endif

