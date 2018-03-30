#pragma once
#ifndef _WINDOWS
#define _WINDOWS
#endif

#include "ComponentBase.h"
#include "AddInDefBase.h"
#include "IMemoryManager.h"
#include "IDevice.h"
#include "IFileDeployer.h"
#include "DatamaxPrinter.h"
#include <string>

class CRFIDevolution : public IComponentBase
{
public:
    enum Props
    {
        ePropVendor = 0,
		ePropSerialNumber,
		ePropTempPath,
        ePropLast      // Always last
    };

    enum Methods
    {
        eMethSendFile = 0,
        eMethReceiveFile,
        eMethReadNearestTag,
        eMethWriteNearestTag,
		eMethPrintLabel,
        eMethLast      // Always last
    };

    CRFIDevolution(void);
    virtual ~CRFIDevolution();
    // IInitDoneBase
    virtual bool ADDIN_API Init(void*);
    virtual bool ADDIN_API setMemManager(void* mem);
    virtual long ADDIN_API GetInfo();
    virtual void ADDIN_API Done();
    // ILanguageExtenderBase
    virtual bool ADDIN_API RegisterExtensionAs(WCHAR_T**);
    virtual long ADDIN_API GetNProps();
    virtual long ADDIN_API FindProp(const WCHAR_T* wsPropName);
    virtual const WCHAR_T* ADDIN_API GetPropName(long lPropNum, long lPropAlias);
    virtual bool ADDIN_API GetPropVal(const long lPropNum, tVariant* pvarPropVal);
    virtual bool ADDIN_API SetPropVal(const long lPropNum, tVariant* varPropVal);
    virtual bool ADDIN_API IsPropReadable(const long lPropNum);
    virtual bool ADDIN_API IsPropWritable(const long lPropNum);
    virtual long ADDIN_API GetNMethods();
    virtual long ADDIN_API FindMethod(const WCHAR_T* wsMethodName);
    virtual const WCHAR_T* ADDIN_API GetMethodName(const long lMethodNum, 
                            const long lMethodAlias);
    virtual long ADDIN_API GetNParams(const long lMethodNum);
    virtual bool ADDIN_API GetParamDefValue(const long lMethodNum, const long lParamNum,
                            tVariant *pvarParamDefValue);   
    virtual bool ADDIN_API HasRetVal(const long lMethodNum);
    virtual bool ADDIN_API CallAsProc(const long lMethodNum,
                    tVariant* paParams, const long lSizeArray);
    virtual bool ADDIN_API CallAsFunc(const long lMethodNum,
                tVariant* pvarRetValue, tVariant* paParams, const long lSizeArray);
    // LocaleBase
    virtual void ADDIN_API SetLocale(const WCHAR_T* loc);
    
private:
    long findName(wchar_t* names[], const wchar_t* name, const uint32_t size) const;
    void addError(uint32_t wcode, const wchar_t* source, 
                    const wchar_t* descriptor, long code);

	bool save_string_copy(tVariant* val, wchar_t** dest, int* size) ;
	bool load_string_copy(tVariant** val, wchar_t* dest, int size) ;
	bool wstringToVar(std::wstring s, tVariant* val) ;
	
	bool ReadNearestTag(
		std::wstring& aTagType, // L"1" - Object, L"2" - Location
		std::wstring& aCompanyId,
		std::wstring& aId );

	bool WriteNearestTag(
		std::wstring aTagType, 
		std::wstring aCompanyId, 
		std::wstring aObjectId );

    // Attributes
    IAddInDefBase      *m_iConnect;
    IMemoryManager     *m_iMemory;

    bool                m_boolEnabled;
    uint32_t            m_uiTimer;
	uint8_t				last_type;
	std::wstring		m_vendor;
    IDevice*			m_device;
	IFileDeployer*		m_dfd;
	DatamaxPrinter*		m_printer;
};
