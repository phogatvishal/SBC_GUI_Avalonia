cmake_minimum_required(VERSION 3.10)
project(SBC_Project LANGUAGES CXX)

# Use C++17
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Export all symbols unless explicitly hidden
set(CMAKE_CXX_VISIBILITY_PRESET hidden)
set(CMAKE_VISIBILITY_INLINES_HIDDEN 1)

# Source files
set(SOURCE_FILES
    SBC.cpp
    COMRuntime.cpp
    UARTInterface.cpp
    EthernetInterface.cpp
    InterfaceRegistrar.cpp
)

# Include headers from current directory
include_directories(${CMAKE_CURRENT_SOURCE_DIR})

# Build shared library (libsbc.so)
add_library(sbc SHARED ${SOURCE_FILES})

# Override hidden visibility for this target so exports like CreateUARTInterface work
target_compile_options(sbc PRIVATE -fvisibility=default)

# Set name of shared lib as "libsbc.so"
set_target_properties(sbc PROPERTIES OUTPUT_NAME "sbc")

# Optional test app
add_executable(test_app main.cpp)
target_link_libraries(test_app PRIVATE sbc dl)

