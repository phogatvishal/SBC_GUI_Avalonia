// CInterface.cpp : Implementation of CCInterface

#include "pch.h"
#include "CInterface.h"

// CCInterface

STDMETHODIMP CCInterface::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* const arr[] = 
	{
		&IID_ICInterface
	};

	for (int i=0; i < sizeof(arr) / sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i],riid))
			return S_OK;
	}
	return S_FALSE;
}

STDMETHODIMP CCInterface::Connect(InterfaceConnection interfaceType, BSTR comPortOrIp, LONG baudRateOrPort, BSTR protocol)
{
	HRESULT hr = S_OK;
	return hr;
}

STDMETHODIMP CCInterface::Disconnect(InterfaceConnection interfaceType)
{
	HRESULT hr = S_OK;
	return hr;
}

STDMETHODIMP CCInterface::GetConnectionStatus(InterfaceConnection interfaceType, BOOL* isConnected)
{
	HRESULT hr = S_OK;
	*isConnected = true;
	return hr;
}

STDMETHODIMP CCInterface::GetHWVersion(InterfaceConnection interfaceType, HWVersion* hwVersion)
{
	HRESULT hr = S_OK;
	*hwVersion = HWVersion::SBC_6U;
	return hr;
}

STDMETHODIMP CCInterface::GetVersionInfo(InterfaceConnection interfaceType, VersionInfo versionType, BSTR* version)
{
	HRESULT hr = S_OK;
	switch (versionType)
	{
	case FPGA_VERSION:
		break;
	case API_VERSION:
		break;
	case GUI_VERSION:
		break;
	case FIRMWARE_VERSION:
		break;
	default:
		hr = S_FALSE;
		break;
	}
	*version = SysAllocString(L"0x1");
	return hr;
}

STDMETHODIMP CCInterface::RunTest(InterfaceConnection interfaceType, Group group, LONG subTest, BSTR* logOutput)
{
	HRESULT hr = S_OK;
  UINT32 count = 0;
	CComBSTR selfTestLog;
	switch (group)
	{
	case Group_Interface:
    UINT32 tmpTestType;
    do
    {
      if (((subTest >> count) & 0x1))
      {
        tmpTestType = (1 << count);

        switch (tmpTestType)
        {
        case PCIE_GEN4_X8_VPX:
          selfTestLog += L"PCIe Gen4 x8 VPX: PASS\n";
          break;
        case PCIE_SWITCH_X8_GEN3:
          selfTestLog += L"PCIe Switch x8 Gen3: FAIL\n";
          break;
        case PCIE_SWITCH_VPX_X4_GEN3_CH0:
          selfTestLog += L"PCIe Switch VPX x4 Gen3 CH0: PASS\n";
          break;
        case PCIE_SWITCH_VPX_X4_GEN3_CH1:
          selfTestLog += L"PCIe Switch VPX x4 Gen3 CH1: FAIL\n";
          break;
        case PCIE_SWITCH_XMC_X8_GEN3_CH0:
          selfTestLog += L"PCIe Switch XMC x8 Gen3 CH0: PASS\n";
          break;
        case PCIE_SWITCH_XMC_X8_GEN3_CH1:
          selfTestLog += L"PCIe Switch XMC x8 Gen3 CH1: FAIL\n";
          break;
        case PCIE_FPGA_GEN3_X4:
          selfTestLog += L"PCIe FPGA Gen3 x4: PASS\n";
          break;
        case PCIE_FPGA_GEN3_X8:
          selfTestLog += L"PCIe FPGA Gen3 x8: FAIL\n";
          break;
        case PCIE_ETH_CONTROLLER_1GBPS:
          selfTestLog += L"PCIe Ethernet Controller 1Gbps: PASS\n";
          break;
        case XEON_40G_BASE:
          selfTestLog += L"Xeon 40GBase Interface: FAIL\n";
          break;
        case XEON_100G_BASE:
          selfTestLog += L"Xeon 100GBase Interface: PASS\n";
          break;
        case SATA_NAND_FLASH:
          selfTestLog += L"SATA NAND Flash: FAIL\n";
          break;
        case SATA_VPX_CH0:
          selfTestLog += L"SATA VPX CH0: PASS\n";
          break;
        case SATA_VPX_CH1:
          selfTestLog += L"SATA VPX CH1: FAIL\n";
          break;
        case SATA_VPX_CH2:
          selfTestLog += L"SATA VPX CH2: PASS\n";
          break;
        case SATA_VPX_M2:
          selfTestLog += L"SATA VPX M.2 Interface: FAIL\n";
          break;
        case LPC_TEST:
          selfTestLog += L"LPC Interface Test: PASS\n";
          break;
        case XEON_IPMC_UART:
          selfTestLog += L"UART Interface between Xeon and IPMC: FAIL\n";
          break;
        case XEON_IPMC_I2C:
          selfTestLog += L"I2C Interface between Xeon and IPMC: PASS\n";
          break;
        case XEON_EMMC_TEST:
          selfTestLog += L"Xeon eMMC Test: FAIL\n";
          break;
        case XEON_VPX_UART:
          selfTestLog += L"Xeon VPX UART: PASS\n";
          break;
        case XEON_USB:
          selfTestLog += L"Xeon USB Interface: FAIL\n";
          break;
        case XEON_VPX_USB_CH1:
          selfTestLog += L"Xeon VPX USB CH1: PASS\n";
          break;
        case XEON_VPX_USB_CH2:
          selfTestLog += L"Xeon VPX USB CH2: FAIL\n";
          break;
        default:
          selfTestLog += L"Unknown Interface Test: Not supported\n";
          break;
        }
      }
      count++;
    }
    while (count < 24);
		break;
	case Group_PBIT:
    if(subTest != 1)
    {
      selfTestLog += L"Invalid test\n";
    }
    else
    {
      selfTestLog += L"PBIT data: PASS\n";
    }
		break;
	case Group_IPMC:
    selfTestLog += L"Not supported\n";
		break;
	default:
		hr = S_FALSE;
		break;
	}
	*logOutput = selfTestLog.Copy();
	return hr;
}

