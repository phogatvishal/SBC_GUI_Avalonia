#ifndef WIN_COM_COMPAT_H
#define WIN_COM_COMPAT_H

#include <cstdint>
typedef int32_t HRESULT;
#define S_OK     ((HRESULT)0)
#define S_FALSE  ((HRESULT)1)
#define E_FAIL   ((HRESULT)0x80004005)

#endif // WIN_COM_COMPAT_H
