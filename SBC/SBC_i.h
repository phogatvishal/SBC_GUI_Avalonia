

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0628 */
/* at Tue Jan 19 08:44:07 2038
 */
/* Compiler settings for SBC.idl:
    Oicf, W1, Zp8, env=Win64 (32b run), target_arch=AMD64 8.01.0628 
    protocol : all , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */



/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __SBC_i_h__
#define __SBC_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

#ifndef DECLSPEC_XFGVIRT
#if defined(_CONTROL_FLOW_GUARD_XFG)
#define DECLSPEC_XFGVIRT(base, func) __declspec(xfg_virtual(base, func))
#else
#define DECLSPEC_XFGVIRT(base, func)
#endif
#endif

/* Forward Declarations */ 

#ifndef __ICInterface_FWD_DEFINED__
#define __ICInterface_FWD_DEFINED__
typedef interface ICInterface ICInterface;

#endif 	/* __ICInterface_FWD_DEFINED__ */


#ifndef __CInterface_FWD_DEFINED__
#define __CInterface_FWD_DEFINED__

#ifdef __cplusplus
typedef class CInterface CInterface;
#else
typedef struct CInterface CInterface;
#endif /* __cplusplus */

#endif 	/* __CInterface_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"
#include "shobjidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_SBC_0000_0000 */
/* [local] */ 

#pragma once
typedef 
enum HWVersion
    {
        SBC_6U	= 0
    } 	HWVersion;

typedef 
enum Group
    {
        Group_Interface	= 0,
        Group_PBIT	= ( Group_Interface + 1 ) ,
        Group_IPMC	= ( Group_PBIT + 1 ) 
    } 	Group;

typedef 
enum InterfaceTests
    {
        PCIE_GEN4_X8_VPX	= ( 1 << 0 ) ,
        PCIE_SWITCH_X8_GEN3	= ( 1 << 1 ) ,
        PCIE_SWITCH_VPX_X4_GEN3_CH0	= ( 1 << 2 ) ,
        PCIE_SWITCH_VPX_X4_GEN3_CH1	= ( 1 << 3 ) ,
        PCIE_SWITCH_XMC_X8_GEN3_CH0	= ( 1 << 4 ) ,
        PCIE_SWITCH_XMC_X8_GEN3_CH1	= ( 1 << 5 ) ,
        PCIE_FPGA_GEN3_X4	= ( 1 << 6 ) ,
        PCIE_FPGA_GEN3_X8	= ( 1 << 7 ) ,
        PCIE_ETH_CONTROLLER_1GBPS	= ( 1 << 8 ) ,
        XEON_40G_BASE	= ( 1 << 9 ) ,
        XEON_100G_BASE	= ( 1 << 10 ) ,
        SATA_NAND_FLASH	= ( 1 << 11 ) ,
        SATA_VPX_CH0	= ( 1 << 12 ) ,
        SATA_VPX_CH1	= ( 1 << 13 ) ,
        SATA_VPX_CH2	= ( 1 << 14 ) ,
        SATA_VPX_M2	= ( 1 << 15 ) ,
        LPC_TEST	= ( 1 << 16 ) ,
        XEON_IPMC_UART	= ( 1 << 17 ) ,
        XEON_IPMC_I2C	= ( 1 << 18 ) ,
        XEON_EMMC_TEST	= ( 1 << 19 ) ,
        XEON_VPX_UART	= ( 1 << 20 ) ,
        XEON_USB	= ( 1 << 21 ) ,
        XEON_VPX_USB_CH1	= ( 1 << 22 ) ,
        XEON_VPX_USB_CH2	= ( 1 << 23 ) 
    } 	InterfaceTests;

typedef 
enum VersionInfo
    {
        FPGA_VERSION	= 0,
        API_VERSION	= ( FPGA_VERSION + 1 ) ,
        GUI_VERSION	= ( API_VERSION + 1 ) ,
        FIRMWARE_VERSION	= ( GUI_VERSION + 1 ) 
    } 	VersionInfo;

typedef 
enum PBITTests
    {
        PBIT_DATA	= ( 1 << 0 ) 
    } 	PBITTests;

typedef 
enum InterfaceConnection
    {
        INTERFACE_UART	= 0,
        INTERFACE_ETHERNET	= ( INTERFACE_UART + 1 ) ,
        INTERFACE_UNKNOWN	= ( INTERFACE_ETHERNET + 1 ) 
    } 	InterfaceConnection;



extern RPC_IF_HANDLE __MIDL_itf_SBC_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_SBC_0000_0000_v0_0_s_ifspec;

#ifndef __ICInterface_INTERFACE_DEFINED__
#define __ICInterface_INTERFACE_DEFINED__

/* interface ICInterface */
/* [unique][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ICInterface;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("91e7c13d-e281-4797-b816-a25819cb1941")
    ICInterface : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Connect( 
            /* [in] */ InterfaceConnection interfaceType,
            /* [in] */ BSTR comPortOrIp,
            /* [in] */ LONG baudRateOrPort,
            /* [in] */ BSTR protocol) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Disconnect( 
            /* [in] */ InterfaceConnection interfaceType) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetConnectionStatus( 
            /* [in] */ InterfaceConnection interfaceType,
            /* [retval][out] */ BOOL *isConnected) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetHWVersion( 
            /* [in] */ InterfaceConnection interfaceType,
            /* [out] */ HWVersion *hwVersion) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetVersionInfo( 
            /* [in] */ InterfaceConnection interfaceType,
            /* [in] */ VersionInfo versionType,
            /* [out] */ BSTR *versionInfo) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE RunTest( 
            /* [in] */ InterfaceConnection interfaceType,
            /* [in] */ Group group,
            /* [in] */ LONG subTest,
            /* [out] */ BSTR *logOutput) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct ICInterfaceVtbl
    {
        BEGIN_INTERFACE
        
        DECLSPEC_XFGVIRT(IUnknown, QueryInterface)
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICInterface * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        DECLSPEC_XFGVIRT(IUnknown, AddRef)
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICInterface * This);
        
        DECLSPEC_XFGVIRT(IUnknown, Release)
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICInterface * This);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfoCount)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ICInterface * This,
            /* [out] */ UINT *pctinfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetTypeInfo)
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ICInterface * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        DECLSPEC_XFGVIRT(IDispatch, GetIDsOfNames)
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ICInterface * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        DECLSPEC_XFGVIRT(IDispatch, Invoke)
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ICInterface * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        DECLSPEC_XFGVIRT(ICInterface, Connect)
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Connect )( 
            ICInterface * This,
            /* [in] */ InterfaceConnection interfaceType,
            /* [in] */ BSTR comPortOrIp,
            /* [in] */ LONG baudRateOrPort,
            /* [in] */ BSTR protocol);
        
        DECLSPEC_XFGVIRT(ICInterface, Disconnect)
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Disconnect )( 
            ICInterface * This,
            /* [in] */ InterfaceConnection interfaceType);
        
        DECLSPEC_XFGVIRT(ICInterface, GetConnectionStatus)
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetConnectionStatus )( 
            ICInterface * This,
            /* [in] */ InterfaceConnection interfaceType,
            /* [retval][out] */ BOOL *isConnected);
        
        DECLSPEC_XFGVIRT(ICInterface, GetHWVersion)
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetHWVersion )( 
            ICInterface * This,
            /* [in] */ InterfaceConnection interfaceType,
            /* [out] */ HWVersion *hwVersion);
        
        DECLSPEC_XFGVIRT(ICInterface, GetVersionInfo)
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetVersionInfo )( 
            ICInterface * This,
            /* [in] */ InterfaceConnection interfaceType,
            /* [in] */ VersionInfo versionType,
            /* [out] */ BSTR *versionInfo);
        
        DECLSPEC_XFGVIRT(ICInterface, RunTest)
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *RunTest )( 
            ICInterface * This,
            /* [in] */ InterfaceConnection interfaceType,
            /* [in] */ Group group,
            /* [in] */ LONG subTest,
            /* [out] */ BSTR *logOutput);
        
        END_INTERFACE
    } ICInterfaceVtbl;

    interface ICInterface
    {
        CONST_VTBL struct ICInterfaceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICInterface_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICInterface_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICInterface_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ICInterface_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ICInterface_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ICInterface_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ICInterface_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ICInterface_Connect(This,interfaceType,comPortOrIp,baudRateOrPort,protocol)	\
    ( (This)->lpVtbl -> Connect(This,interfaceType,comPortOrIp,baudRateOrPort,protocol) ) 

#define ICInterface_Disconnect(This,interfaceType)	\
    ( (This)->lpVtbl -> Disconnect(This,interfaceType) ) 

#define ICInterface_GetConnectionStatus(This,interfaceType,isConnected)	\
    ( (This)->lpVtbl -> GetConnectionStatus(This,interfaceType,isConnected) ) 

#define ICInterface_GetHWVersion(This,interfaceType,hwVersion)	\
    ( (This)->lpVtbl -> GetHWVersion(This,interfaceType,hwVersion) ) 

#define ICInterface_GetVersionInfo(This,interfaceType,versionType,versionInfo)	\
    ( (This)->lpVtbl -> GetVersionInfo(This,interfaceType,versionType,versionInfo) ) 

#define ICInterface_RunTest(This,interfaceType,group,subTest,logOutput)	\
    ( (This)->lpVtbl -> RunTest(This,interfaceType,group,subTest,logOutput) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICInterface_INTERFACE_DEFINED__ */



#ifndef __SBCLib_LIBRARY_DEFINED__
#define __SBCLib_LIBRARY_DEFINED__

/* library SBCLib */
/* [version][uuid] */ 


EXTERN_C const IID LIBID_SBCLib;

EXTERN_C const CLSID CLSID_CInterface;

#ifdef __cplusplus

class DECLSPEC_UUID("a9f691c5-ac1c-4206-a250-3a11c5782955")
CInterface;
#endif
#endif /* __SBCLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  BSTR_UserSize64(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal64(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal64(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree64(     unsigned long *, BSTR * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


