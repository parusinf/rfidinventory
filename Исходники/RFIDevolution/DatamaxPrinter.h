// DataMax E-4203 printer implementation
// Author: Pavel Nikitin (c) 2013
#pragma once
#include <atlbase.h>
#include <sstream>

typedef DWORD (__stdcall *MYPROC)(DWORD, DWORD, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR);

class DatamaxPrinter
{
public:
	DatamaxPrinter(void);
	~DatamaxPrinter(void);
	
	// Print label
	DWORD PrintLabel(
		std::wstring prefix,      // 5 digits: 02001 - объект, 04001 - помещение
		std::wstring companyCode, // 8 digits
		std::wstring barcode,     // 12 digits
		std::wstring companyName,
		std::wstring nomenclature,
		std::wstring inventoryNumber,
		std::wstring serialNumber);

private:
	HINSTANCE    hInstanceLib; 
    MYPROC	     fPrintProcAddr;
	BOOL		 fRunTimeLinkSuccess;
	std::wstring sTempPathBuffer;

	// Create label template file in temporary directory
	DWORD CreateSLB();
	std::string ws2s(std::wstring);

};
