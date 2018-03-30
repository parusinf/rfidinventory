#ifndef NUROS_WIN32_H_
#define NUROS_WIN32_H_

#include <winsock2.h>
#include <Ws2tcpip.h>
#include <windows.h>
#include <tchar.h>

#ifndef NUR_WIN32
	#define NUR_WIN32 1
#endif

#ifdef UNDER_CE
	// WIN CE
	#define NUR_WINCE 1
	#define NUR_MODULECONTROL 1
	#undef NUR_USB_AUTOCONNECT
	#undef NUR_USB_TRANSPORT
#else
	// PC
	#define NUR_USB_AUTOCONNECT 1
	#define NUR_USB_TRANSPORT 1
	#undef NUR_MODULECONTROL
#endif

#define NUR_SOCKET_TRANSPORT 1
#define NUR_SERVER_TRANSPORT 1
//#define NUR_BLUETOOTH_SOCKET 1
#define NUR_SERIAL_TRANSPORT 1

#ifndef NUR_WINCE
	#include <shlobj.h>    // for SHGetFolderPath
#endif

#endif /* NUROS_WIN32_H_ */
