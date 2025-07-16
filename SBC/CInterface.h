// CInterface.h : Declaration of the CCInterface

#pragma once
#include "resource.h"       // main symbols
#include "SBC_i.h"
//#include "CommunicationInterface.h"
//#include "EthernetInterface.h"
//#include "UARTInterface.h"
#include <memory>


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CCInterface

class ATL_NO_VTABLE CCInterface :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CCInterface, &CLSID_CInterface>,
	public ISupportErrorInfo,
	public IDispatchImpl<ICInterface, &IID_ICInterface, &LIBID_SBCLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CCInterface()
	{
	}

DECLARE_REGISTRY_RESOURCEID(106)

DECLARE_NOT_AGGREGATABLE(CCInterface)

BEGIN_COM_MAP(CCInterface)
	COM_INTERFACE_ENTRY(ICInterface)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
END_COM_MAP()

// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

public:
	// Method declarations
	STDMETHOD(Connect)(InterfaceConnection interfaceType, BSTR comPortOrIp, LONG baudRateOrPort, BSTR protocol);
	STDMETHOD(Disconnect)(InterfaceConnection interfaceType);
	STDMETHOD(GetConnectionStatus)(InterfaceConnection interfaceType, BOOL* isConnected);
	STDMETHOD(GetHWVersion)(InterfaceConnection interfaceType, HWVersion* hwVersion);
	STDMETHOD(GetVersionInfo)(InterfaceConnection interfaceType, VersionInfo versionType, BSTR* version);
	STDMETHOD(RunTest)(InterfaceConnection interfaceType, Group group, LONG subTest, BSTR* logOutput);
};

OBJECT_ENTRY_AUTO(__uuidof(CInterface), CCInterface)
