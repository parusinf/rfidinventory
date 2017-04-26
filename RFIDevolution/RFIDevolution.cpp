#include <atlbase.h>
#include <rapi2.h>
#include "stdafx.h"
#include <stdio.h>
#include <wchar.h>
#include <stdlib.h>
#include <string>
#include <sstream>
#include <iostream>
#include <atlconv.h>
#include "RFIDevolution.h"
#include "RFIDTag.h"
#include "NordicIdDevice.h"
#include "DeviceFileDeployer.h"

#define TIME_LEN 34

#define BASE_ERRNO     7

static wchar_t *g_PropNames[] = {
	L"Vendor",
	L"SerialNumber",
	L"TempPath",
};

static wchar_t *g_MethodNames[] = {
	L"SendFile", 
	L"ReceiveFile",
	L"ReadNearestTag",
	L"WriteNearestTag",
	L"PrintLabel"
};

static wchar_t *g_PropNamesRu[] = {
	L"ѕроизводитель",
	L"—ерийныйЌомер",
	L"¬ременныйѕуть"
};

static wchar_t *g_MethodNamesRu[] = {
	L"ќтправить‘айл", 
	L"ѕолучить‘айл",
	L"—читатьЅлижайшуюћетку",
	L"«аписатьЅлижайшуюћетку",
	L"ѕечататьЁтикетку"
};

static const wchar_t g_kClassNames[] = L"CRFIDevolution";
static IAddInDefBase *pAsyncEvent = NULL;

//---------------------------------------------------------------------------//
uint32_t convToShortWchar(WCHAR_T** Dest, const wchar_t* Source, uint32_t len = 0);
uint32_t convFromShortWchar(wchar_t** Dest, const WCHAR_T* Source, uint32_t len = 0);
uint32_t getLenShortWcharStr(const WCHAR_T* Source);
__int64 toInt64(const std::wstring&);
char* convPWCharToPChar(const wchar_t*);
std::wstring convPCharToWS(const char*);
//---------------------------------------------------------------------------//
long GetClassObject(const WCHAR_T* wsName, IComponentBase** pInterface)
{
    if(!*pInterface)
    {
        *pInterface= new CRFIDevolution;
        return (long)*pInterface;
    }
    return 0;
}
//---------------------------------------------------------------------------//
long DestroyObject(IComponentBase** pIntf)
{
   if(!*pIntf)
      return -1;

   delete *pIntf;
   *pIntf = 0;
   return 0;
}
//---------------------------------------------------------------------------//
const WCHAR_T* GetClassNames()
{
    static WCHAR_T* names = 0;
    if (!names)
        ::convToShortWchar(&names, g_kClassNames);
    return names;
}
//---------------------------------------------------------------------------//
uint32_t __stdcall RAPISendFile(
	const char* sourceDesktopFileName, 
	const char* destinationDeviceFileName)
{
	// COM init
	::CoInitialize( NULL );
    ::OleInitialize( NULL );
	
	IFileDeployer* fd = CDeviceFileDeployer::CreateInstance();

	DWORD res = fd->SendFile(
		convPCharToWS( sourceDesktopFileName ), 
		convPCharToWS( destinationDeviceFileName ) );

	// uninit COM
	::OleUninitialize();
	::CoUninitialize();

	return res == 0;
}
//---------------------------------------------------------------------------//
uint32_t __stdcall RAPIReceiveFile(
	const char* sourceDeviceFileName, 
	const char* destinationDesktopFileName)
{
	// COM init
	::CoInitialize( NULL );
    ::OleInitialize( NULL );

	IFileDeployer* fd = CDeviceFileDeployer::CreateInstance();

	DWORD res = fd->ReceiveFile(
		convPCharToWS(sourceDeviceFileName), 
		convPCharToWS(destinationDesktopFileName) );
	
	// uninit COM
	::OleUninitialize();
	::CoUninitialize();

	return res == 0;
}
//---------------------------------------------------------------------------//

// CRFIDevolution
//---------------------------------------------------------------------------//
CRFIDevolution::CRFIDevolution()
{
    m_iMemory = 0;
    m_iConnect = 0;
}
//---------------------------------------------------------------------------//
CRFIDevolution::~CRFIDevolution()
{
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::Init(void* pConnection)
{ 
    m_iConnect = (IAddInDefBase*)pConnection;
	m_vendor = L"NordicID";

	// create DeviceFileDeployer
	m_dfd = CDeviceFileDeployer::CreateInstance();

	// create Nordic ID device
	m_device = CNordicIdDevice::CreateInstance();

	// create DatamaxPrinter
	m_printer = new DatamaxPrinter();

	return m_iConnect != NULL;
}
//---------------------------------------------------------------------------//
long CRFIDevolution::GetInfo()
{ 
    // Component should put supported component technology version 
    // This component supports 2.0 version
    return 2000; 
}
//---------------------------------------------------------------------------//
void CRFIDevolution::Done()
{
	delete m_device;
	delete m_printer;
}
/////////////////////////////////////////////////////////////////////////////
// ILanguageExtenderBase
//---------------------------------------------------------------------------//
bool CRFIDevolution::RegisterExtensionAs(WCHAR_T** wsExtensionName)
{ 
    wchar_t *wsExtension = L"RFIDevolution";
    int iActualSize = ::wcslen(wsExtension) + 1;
    WCHAR_T* dest = 0;

    if (m_iMemory)
    {
        if(m_iMemory->AllocMemory((void**)wsExtensionName, iActualSize * sizeof(WCHAR_T)))
            ::convToShortWchar(wsExtensionName, wsExtension, iActualSize);
        return true;
    }

    return false; 
}
//---------------------------------------------------------------------------//
long CRFIDevolution::GetNProps()
{ 
     return ePropLast;
}
//---------------------------------------------------------------------------//
long CRFIDevolution::FindProp(const WCHAR_T* wsPropName)
{ 
    long plPropNum = -1;
    wchar_t* propName = 0;

    ::convFromShortWchar(&propName, wsPropName);
    plPropNum = findName(g_PropNames, propName, ePropLast);

    if (plPropNum == -1)
        plPropNum = findName(g_PropNamesRu, propName, ePropLast);

    delete[] propName;

    return plPropNum;
}
//---------------------------------------------------------------------------//
const WCHAR_T* CRFIDevolution::GetPropName(long lPropNum, long lPropAlias)
{ 
    if (lPropNum >= ePropLast)
        return NULL;

    wchar_t *wsCurrentName = NULL;
    WCHAR_T *wsPropName = NULL;
    int iActualSize = 0;

    switch(lPropAlias)
    {
    case 0: // First language
        wsCurrentName = g_PropNames[lPropNum];
        break;
    case 1: // Second language
        wsCurrentName = g_PropNamesRu[lPropNum];
        break;
    default:
        return 0;
    }
    
    iActualSize = wcslen(wsCurrentName)+1;

    if (m_iMemory && wsCurrentName)
    {
        if (m_iMemory->AllocMemory((void**)&wsPropName, iActualSize * sizeof(WCHAR_T)))
            ::convToShortWchar(&wsPropName, wsCurrentName, iActualSize);
    }

    return wsPropName;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::GetPropVal(const long lPropNum, tVariant* pvarPropVal)
{ 
	DWORD dwRetVal = 0;

	switch(lPropNum)
    {
	case ePropVendor:
		wstringToVar(m_dfd->GetDeviceName(), pvarPropVal);
		break;
	
	case ePropTempPath:
		WCHAR lpTempPathBuffer[MAX_PATH];
		dwRetVal = GetTempPath(MAX_PATH, lpTempPathBuffer);				
		wstringToVar((WCHAR_T*)&lpTempPathBuffer, pvarPropVal);
		break;
	
	case ePropSerialNumber:
		if (!m_device->getConnected())
			m_device->Connect();

		if (m_device->getConnected())
			wstringToVar(m_device->getSerial(), pvarPropVal);
		else
			wstringToVar(L"N/A", pvarPropVal);

		break;

	default:
		return false;
    }

    return true;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::SetPropVal(const long lPropNum, tVariant *varPropVal)
{ 
	last_type = (char)TV_VT(varPropVal);
    return false;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::IsPropReadable(const long lPropNum)
{ 
    switch(lPropNum)
    { 
	case ePropVendor:
		return true;
	case ePropTempPath:
		return true;
	case ePropSerialNumber:
		return true;
    }

    return false;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::IsPropWritable(const long lPropNum)
{
    return false;
}
//---------------------------------------------------------------------------//
long CRFIDevolution::GetNMethods()
{ 
    return eMethLast;
}
//---------------------------------------------------------------------------//
long CRFIDevolution::FindMethod(const WCHAR_T* wsMethodName)
{ 
    long plMethodNum = -1;
    wchar_t* name = 0;

    ::convFromShortWchar(&name, wsMethodName);

    plMethodNum = findName(g_MethodNames, name, eMethLast);

    if (plMethodNum == -1)
        plMethodNum = findName(g_MethodNamesRu, name, eMethLast);

    return plMethodNum;
}
//---------------------------------------------------------------------------//
const WCHAR_T* CRFIDevolution::GetMethodName(const long lMethodNum, const long lMethodAlias)
{ 
    if (lMethodNum >= eMethLast)
        return NULL;

    wchar_t *wsCurrentName = NULL;
    WCHAR_T *wsMethodName = NULL;
    int iActualSize = 0;

    switch(lMethodAlias)
    {
		case 0: // First language
			wsCurrentName = g_MethodNames[lMethodNum];
			break;
		case 1: // Second language
			wsCurrentName = g_MethodNamesRu[lMethodNum];
			break;
		default: 
			return 0;
    }

    iActualSize = wcslen(wsCurrentName)+1;

    if (m_iMemory && wsCurrentName)
    {
        if(m_iMemory->AllocMemory((void**)&wsMethodName, iActualSize * sizeof(WCHAR_T)))
            ::convToShortWchar(&wsMethodName, wsCurrentName, iActualSize);
    }

    return wsMethodName;
}
//---------------------------------------------------------------------------//
long CRFIDevolution::GetNParams(const long lMethodNum)
{ 
    switch(lMethodNum)
    { 
	case eMethSendFile:
		return 2;
	case eMethReceiveFile:
		return 2;
	case eMethReadNearestTag:
		return 3;
	case eMethWriteNearestTag:
		return 3;
	case eMethPrintLabel:
		return 7;
	default:
		return 0;
    }
    
    return 0;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::GetParamDefValue(const long lMethodNum, const long lParamNum,
	tVariant *pvarParamDefValue)
{ 
    TV_VT(pvarParamDefValue)= VTYPE_EMPTY;

    switch(lMethodNum)
    { 
	case eMethSendFile:
	case eMethReceiveFile:
		// There are no parameter values by default 
		break;
	default:
		return false;
    }

    return false;
} 
//---------------------------------------------------------------------------//
bool CRFIDevolution::HasRetVal(const long lMethodNum)
{ 
    switch(lMethodNum)
    { 
	case eMethSendFile:
	case eMethReceiveFile:
	case eMethReadNearestTag:
	case eMethWriteNearestTag:
	case eMethPrintLabel:
		return true;
	default:
		return false;
    }

    return false;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::CallAsProc(const long lMethodNum,
	tVariant* paParams, const long lSizeArray)
{ 
    return false;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::CallAsFunc(const long lMethodNum,
	tVariant* pvarRetValue, tVariant* paParams, const long lSizeArray)
{ 
    bool ret = false;

    switch(lMethodNum)
    {
	// send file from desktop to device
	case eMethSendFile:
		{
			if (!paParams || lSizeArray != 2)
				return false;

			TV_VT(pvarRetValue) = VTYPE_BOOL;
			
			TV_BOOL(pvarRetValue) = m_dfd->SendFile(
				(paParams) -> pwstrVal, 
				(paParams + 1) -> pwstrVal ) == 0;

			ret = true;
			break;
		}

	// receive file from device to desktop
	case eMethReceiveFile:
		{
			if (!paParams || lSizeArray != 2)
				return false;
			
			TV_VT(pvarRetValue) = VTYPE_BOOL;			
			
			TV_BOOL(pvarRetValue) = m_dfd->ReceiveFile(
				(paParams) -> pwstrVal, 
				(paParams+1) -> pwstrVal ) == 0;

			ret = true;
			break;
		}

	// read nearest tag
	case eMethReadNearestTag:
		{
			if (!paParams || lSizeArray != 3)
				return false;
			
			std::wstring wsTagType;
			std::wstring wsCompanyId;
			std::wstring wsId;

			TV_VT(pvarRetValue) = VTYPE_BOOL;
			TV_BOOL(pvarRetValue) = 
				ReadNearestTag(
					wsTagType, 
					wsCompanyId,
					wsId
				);

			::convFromShortWchar(&((paParams) -> pwstrVal), wsTagType.c_str());
			::convFromShortWchar(&((paParams+1) -> pwstrVal), wsCompanyId.c_str());
			::convFromShortWchar(&((paParams+2) -> pwstrVal), wsId.c_str());

			ret = true;
			break;
		}

	// write nearest tag
	case eMethWriteNearestTag:
		{
			if (!paParams || lSizeArray != 3)
				return false;
			
			TV_VT(pvarRetValue) = VTYPE_BOOL;
			TV_BOOL(pvarRetValue) = 
				WriteNearestTag(
					((paParams) -> pwstrVal), 
					((paParams+1) -> pwstrVal),
					((paParams+2) -> pwstrVal)
				);

			ret = true;
			break;
		}

	// print label on Datamax printer
	case eMethPrintLabel:
		{
			/*
			ѕечататьЁтикетку(
				"02001", 
				"02510001", 
				"000000014993", 
				"ќјќ ќ–√ЁЌ≈–√ќ√ј«",
				"—ервер базы данных HP DL380G5 DC X5160 3 0/1333/4M 2G 1P P400/256M RF", 
				"»нв.N 0140-01.0006325",
				"—ер.N CZC6403TWV");
			*/

			if (!paParams || lSizeArray != 7)
				return false;

			std::wstring wsParam1 = std::wstring( ((paParams)   -> pwstrVal) );
			std::wstring wsParam2 = std::wstring( ((paParams+1) -> pwstrVal) );
			std::wstring wsParam3 = std::wstring( ((paParams+2) -> pwstrVal) );
			std::wstring wsParam4 = std::wstring( ((paParams+3) -> pwstrVal) );
			std::wstring wsParam5 = std::wstring( ((paParams+4) -> pwstrVal) );
			std::wstring wsParam6 = std::wstring( ((paParams+5) -> pwstrVal) );
			std::wstring wsParam7 = std::wstring( ((paParams+6) -> pwstrVal) );

			TV_VT(pvarRetValue) = VTYPE_BOOL;
			TV_BOOL(pvarRetValue) = 
				m_printer->PrintLabel(
					wsParam1, 
					wsParam2,
					wsParam3,
					wsParam4,
					wsParam5,
					wsParam6,
					wsParam7
				) == 0;

			/*TV_VT(pvarRetValue) = VTYPE_BOOL;
			TV_BOOL(pvarRetValue) = 
				m_printer->PrintLabel(
					(paParams)   -> pwstrVal, 
					(paParams+1) -> pwstrVal,
					(paParams+2) -> pwstrVal,
					(paParams+3) -> pwstrVal,
					(paParams+4) -> pwstrVal,
					(paParams+5) -> pwstrVal,
					(paParams+6) -> pwstrVal
				) == 0;*/

			ret = true;
			break;
		}
	}

	return ret; 
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::ReadNearestTag(
	std::wstring& aTagType, // L"1" - Object, L"2" - Location
	std::wstring& aCompanyId,
	std::wstring& aId
)
{
	if (m_device->Connect())
	{
		wchar_t buffer[65];
		ITag* tag = m_device->GetNearestTag();
		
		// tag type
		switch (tag->getType())
		{
		case ITag::EType::Object:
			aTagType = L"1";
			break;

		case ITag::EType::Location:
			aTagType = L"2";
			break;

		default:
			aTagType = L"0";
		}
		
		// tag company
		_i64tow(tag->getCompanyId(), buffer, 10);
		aCompanyId = buffer;
		
		// tag identificator
		_i64tow(tag->getId(), buffer, 10);
		aId = buffer;
		
		return true;
	}
	else
		return false;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::WriteNearestTag(
	std::wstring aTagType, // L"1" - Object, L"2" - Location
	std::wstring aCompanyId,
	std::wstring aId
)
{
	bool success = false;

	if (m_device->Connect())
	{
		ITag* tag = new CRFIDTag();

		tag->setType( (ITag::EType)toInt64(aTagType) );
		tag->setCompanyId( toInt64(aCompanyId) );
		tag->setId( toInt64(aId) );

		if (m_device->WriteTag(tag) == IDevice::EResponse::Successful)	
			success = true;

		delete tag;
	}
	
	return success;
}
//---------------------------------------------------------------------------//
void CRFIDevolution::SetLocale(const WCHAR_T* loc)
{
    _wsetlocale(LC_ALL, loc);
}
/////////////////////////////////////////////////////////////////////////////
// LocaleBase
//---------------------------------------------------------------------------//
bool CRFIDevolution::setMemManager(void* mem)
{
    m_iMemory = (IMemoryManager*)mem;
    return m_iMemory != 0;
}
//---------------------------------------------------------------------------//
void CRFIDevolution::addError(uint32_t wcode, const wchar_t* source, 
	const wchar_t* descriptor, long code)
{
    if (m_iConnect)
    {
        WCHAR_T *err = 0;
        WCHAR_T *descr = 0;
        
        ::convToShortWchar(&err, source);
        ::convToShortWchar(&descr, descriptor);

        m_iConnect->AddError(wcode, err, descr, code);
        delete[] err;
        delete[] descr;
    }
}
//---------------------------------------------------------------------------//
long CRFIDevolution::findName(wchar_t* names[], const wchar_t* name, 
	const uint32_t size) const
{
    long ret = -1;
    for (uint32_t i = 0; i < size; i++)
    {
        if (!wcscmp(names[i], name))
        {
            ret = i;
            break;
        }
    }
    return ret;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::save_string_copy(tVariant* val, wchar_t** dest, int* size) 
{
	wchar_t* t1 = *dest;
	if (t1)
		delete [] t1;
	int iActualSize = val -> strLen +1;
	t1 = new wchar_t[iActualSize*sizeof(wchar_t)];
	memcpy(t1, val -> pwstrVal, iActualSize*sizeof(wchar_t));
	*size = iActualSize;
	*dest = t1;
	return true;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::load_string_copy(tVariant** val, wchar_t* dest, int size) 
{
    wchar_t* t1 = NULL;
	m_iMemory->AllocMemory((void**)&t1, size * sizeof(WCHAR_T));
	memcpy(t1, dest, size * sizeof(WCHAR_T));
	TV_VT(*val) = VTYPE_PWSTR;
	(*val) -> pwstrVal = t1;
	(*val) -> strLen = size-1;
	return true;
}
//---------------------------------------------------------------------------//
bool CRFIDevolution::wstringToVar(std::wstring str, tVariant* val) {
	char* t1;
	TV_VT(val) = VTYPE_PWSTR;
	m_iMemory->AllocMemory((void**)&t1, (str.length()+1) * sizeof(WCHAR_T));
	memcpy(t1, str.c_str(), (str.length()+1) * sizeof(WCHAR_T));
	val -> pstrVal = t1;
	val -> strLen = str.length();
	return true;
}
//---------------------------------------------------------------------------//
uint32_t convToShortWchar(WCHAR_T** Dest, const wchar_t* Source, uint32_t len)
{
    if (!len)
        len = ::wcslen(Source)+1;

    if (!*Dest)
        *Dest = new WCHAR_T[len];

    WCHAR_T* tmpShort = *Dest;
    wchar_t* tmpWChar = (wchar_t*) Source;
    uint32_t res = 0;

    ::memset(*Dest, 0, len*sizeof(WCHAR_T));
    do
    {
        *tmpShort++ = (WCHAR_T)*tmpWChar++;
        ++res;
    }
    while (len-- && *tmpWChar);

    return res;
}
//---------------------------------------------------------------------------//
uint32_t convFromShortWchar(wchar_t** Dest, const WCHAR_T* Source, uint32_t len)
{
    if (!len)
        len = getLenShortWcharStr(Source)+1;

    if (!*Dest)
        *Dest = new wchar_t[len];

    wchar_t* tmpWChar = *Dest;
    WCHAR_T* tmpShort = (WCHAR_T*)Source;
    uint32_t res = 0;

    ::memset(*Dest, 0, len*sizeof(wchar_t));
    do
    {
        *tmpWChar++ = (wchar_t)*tmpShort++;
        ++res;
    }
    while (len-- && *tmpShort);

    return res;
}
//---------------------------------------------------------------------------//
uint32_t getLenShortWcharStr(const WCHAR_T* Source)
{
    uint32_t res = 0;
    WCHAR_T *tmpShort = (WCHAR_T*)Source;

    while (*tmpShort++)
        ++res;

    return res;
}
//---------------------------------------------------------------------------//
__int64 toInt64(const std::wstring& strbuf)
{
    std::wstringstream converter;
    __int64 value = 0;
    converter << strbuf;
    converter >> value;
    return value;
}
//---------------------------------------------------------------------------//
char* convPWCharToPChar(const wchar_t* pwcSource)
{
	std::wstring wsSource( pwcSource );
	std::string sSource( wsSource.begin(), wsSource.end() );
	
	char* pcSource = new char[sSource.length() + 1];
	memcpy(pcSource, sSource.c_str(), sSource.length());
	pcSource[sSource.length()] = 0;
	
	return pcSource;
}
//---------------------------------------------------------------------------//
std::wstring convPCharToWS(const char* source)
{
	std::string sSource( source );
	std::wstring wsSource;
	wsSource.assign( sSource.begin(), sSource.end() );
	return wsSource;
}
//---------------------------------------------------------------------------//
