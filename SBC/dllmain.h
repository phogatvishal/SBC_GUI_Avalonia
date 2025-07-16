// dllmain.h : Declaration of module class.

class CSBCModule : public ATL::CAtlDllModuleT< CSBCModule >
{
public :
	DECLARE_LIBID(LIBID_SBCLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_SBC, "{0de9549c-be2e-4d13-9a56-31bf12508400}")
};

extern class CSBCModule _AtlModule;
