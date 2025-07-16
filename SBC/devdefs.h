#pragma once
#include <string>

/**
 * @brief Enumerates supported hardware versions.
 */
typedef enum HWVersion
{
  SBC_6U = 0 /* 6U Single Board Computer */
} HWVersion;

/**
 * @brief Enumerates available test or functionality groups.
 */
typedef enum Group
{
  Group_Interface = 0, /* Interface-related group */
  Group_PBIT,          /* Power-on Built-In Test group */
  Group_IPMC           /* Intelligent Platform Management Controller group */
} Group;

/**
 * @enum InterfaceTests
 * @brief Defines bitmask flags for various interface tests.
 */
typedef enum InterfaceTests
{
  /* PCIe Gen4 x8 VPX interface */
  PCIE_GEN4_X8_VPX = 1 << 0,

  /* PCIe switch x8 Gen3 */
  PCIE_SWITCH_X8_GEN3 = 1 << 1,

  /* PCIe switch VPX x4 Gen3 channel 0 */
  PCIE_SWITCH_VPX_X4_GEN3_CH0 = 1 << 2,

  /* PCIe switch VPX x4 Gen3 channel 1 */
  PCIE_SWITCH_VPX_X4_GEN3_CH1 = 1 << 3,

  /* PCIe switch XMC x8 Gen3 channel 0 */
  PCIE_SWITCH_XMC_X8_GEN3_CH0 = 1 << 4,

  /* PCIe switch XMC x8 Gen3 channel 1 */
  PCIE_SWITCH_XMC_X8_GEN3_CH1 = 1 << 5,

  /* PCIe FPGA Gen3 x4 */
  PCIE_FPGA_GEN3_X4 = 1 << 6,

  /* PCIe FPGA Gen3 x8 */
  PCIE_FPGA_GEN3_X8 = 1 << 7,

  /* PCIe Ethernet controller 1 Gbps */
  PCIE_ETH_CONTROLLER_1GBPS = 1 << 8,

  /* Xeon 40GBase interface */
  XEON_40G_BASE = 1 << 9,

  /* Xeon 100GBase interface */
  XEON_100G_BASE = 1 << 10,

  /* SATA NAND flash storage */
  SATA_NAND_FLASH = 1 << 11,

  /* SATA VPX channel 0 */
  SATA_VPX_CH0 = 1 << 12,

  /* SATA VPX channel 1 */
  SATA_VPX_CH1 = 1 << 13,

  /* SATA VPX channel 2 */
  SATA_VPX_CH2 = 1 << 14,

  /* SATA M.2 VPX interface */
  SATA_VPX_M2 = 1 << 15,

  /* LPC interface test */
  LPC_TEST = 1 << 16,

  /* UART interface between Xeon and IPMC */
  XEON_IPMC_UART = 1 << 17,

  /* I2C interface between Xeon and IPMC */
  XEON_IPMC_I2C = 1 << 18,

  /* Xeon eMMC interface test */
  XEON_EMMC_TEST = 1 << 19,

  /* UART over VPX on Xeon */
  XEON_VPX_UART = 1 << 20,

  /* USB interface on Xeon */
  XEON_USB = 1 << 21,

  /* USB VPX channel 1 on Xeon */
  XEON_VPX_USB_CH1 = 1 << 22,

  /* USB VPX channel 2 on Xeon */
  XEON_VPX_USB_CH2 = 1 << 23
} InterfaceTests;

/**
 * @enum VersionInfo
 * @brief Enumerates different version types used in the system.
 */
typedef enum VersionInfo
{
  /* Version of the FPGA (Field-Programmable Gate Array) component */
  FPGA_VERSION = 0,

  /* Version of the system's Application Programming Interface (API) */
  API_VERSION,

  /* Version of the Graphical User Interface (GUI) component */
  GUI_VERSION,

  /* Version of the firmware running on the hardware */
  FIRMWARE_VERSION
} VersionInfo;

/**
 * @enum PBITTests
 * @brief Bitmask flags representing tests for PBIT (Power-on Built-In Test) operations.
 */
typedef enum PBITTests
{
  /* PBIT data test flag */
  PBIT_DATA = 1 << 0
} PBITTests;

/**
 * @brief Enum representing different communication interfaces.
 */
typedef enum InterfaceConnection
{
  /* UART communication interface */
  INTERFACE_UART = 0,

  /* Ethernet communication interface */
  INTERFACE_ETHERNET,

  /* Unknown interface */
  INTERFACE_UNKNOWN
} InterfaceConnection;