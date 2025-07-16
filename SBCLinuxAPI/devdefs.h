#pragma once

// ==============================
// Interface Test Bitmask Flags
// ==============================

#define PCIE_GEN4_X8_VPX             (1u << 0)
#define PCIE_SWITCH_X8_GEN3          (1u << 1)
#define PCIE_SWITCH_VPX_X4_GEN3_CH0  (1u << 2)
#define PCIE_SWITCH_VPX_X4_GEN3_CH1  (1u << 3)
#define PCIE_SWITCH_XMC_X8_GEN3_CH0  (1u << 4)
#define PCIE_SWITCH_XMC_X8_GEN3_CH1  (1u << 5)
#define PCIE_FPGA_GEN3_X4            (1u << 6)
#define PCIE_FPGA_GEN3_X8            (1u << 7)
#define PCIE_ETH_CONTROLLER_1GBPS    (1u << 8)
#define XEON_40G_BASE                (1u << 9)
#define XEON_100G_BASE               (1u << 10)
#define SATA_NAND_FLASH              (1u << 11)
#define SATA_VPX_CH0                 (1u << 12)
#define SATA_VPX_CH1                 (1u << 13)
#define SATA_VPX_CH2                 (1u << 14)
#define SATA_VPX_M2                  (1u << 15)
#define LPC_TEST                     (1u << 16)
#define XEON_IPMC_UART               (1u << 17)
#define XEON_IPMC_I2C                (1u << 18)
#define XEON_EMMC_TEST               (1u << 19)
#define XEON_VPX_UART                (1u << 20)
#define XEON_USB                     (1u << 21)
#define XEON_VPX_USB_CH1             (1u << 22)
#define XEON_VPX_USB_CH2             (1u << 23)

// ==============================
// PBIT Test Bitmask Flags
// ==============================

#define PBIT_DATA                    (1u << 0)


typedef enum InterfaceConnectionType {
    INTERFACE_UART = 0,       // UART communication
    INTERFACE_ETHERNET,       // Ethernet communication
    INTERFACE_UNKNOWN         // Unknown or unsupported
} InterfaceConnectionType;

