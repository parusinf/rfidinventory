// File: NordicIdDevice.cpp
// Program: RFIDevolution
// Version 1.0
// Copyright © Pavel Nikitin 2013
#include "NordicIdDevice.h"
#include "RFIDTag.h"
#include <string>

#define USE_USB_AUTO_CONNECT 1
void NURAPICALLBACK MyNotificationFunc(HANDLE m_hNur, DWORD timestamp, int type, LPVOID data, int dataLen);

// Init Singleton
CNordicIdDevice* CNordicIdDevice::m_self = NULL;

// CONSTRUCTORS & DESTRUCTORS
//---------------------------------------------------------------------------//
// Create and init device
CNordicIdDevice::CNordicIdDevice()
{
	m_connected = false;
	m_scaning = false;
	m_error = 0;
	    
	m_hNur = INVALID_HANDLE_VALUE;
	m_tag_storage_size = 0;
	
	// Create new API object    
	m_hNur = NurApiCreate();    
}

CNordicIdDevice::~CNordicIdDevice()
{
	// Free API object    
	if (m_hNur != INVALID_HANDLE_VALUE)
		NurApiFree(m_hNur);
}

// PROPERTIES
//---------------------------------------------------------------------------//
// Device init successful
//---------------------------------------------------------------------------//
bool CNordicIdDevice::getConnected()
{
	return m_connected;
}
    
// Scaning in process
//---------------------------------------------------------------------------//
bool CNordicIdDevice::getScaning()
{
	return m_scaning;
}

// Convert EPC byte array to string
void CNordicIdDevice::EpcToString(BYTE *epc, DWORD epcLen, TCHAR *epcStr)
{    
	DWORD n;    
	int pos = 0;    
	for (n = 0; n < epcLen; n++) 
	{        
		pos += _stprintf_s(&epcStr[pos], NUR_MAX_EPC_LENGTH-pos, _T("%02x"), epc[n]);    
	}    
	epcStr[pos] = 0;
}

// Perform inventory
int CNordicIdDevice::PerformInventory()
{
	int rounds = 3;    
	int error = 0;    
	int tagsFound = 0, tagsMem = 0;    
	int tagCount = 0;    
	int idx = 0;    
	HANDLE hStorage = NULL;    
	HANDLE hTag = NULL;    
	struct NUR_INVENTORY_RESPONSE invResp;    
	struct NUR_TAG_DATA tagData;    
	TCHAR epcStr[128];    
	
	// clear previously inventoried tags from memory
	Clear();  
	
	_tprintf(_T("Perform inventory ."));    
	while (rounds-- > 0)    
	{               
		_tprintf(_T("."));                
		
		// perform inventory with 500ms timeout, Q 3, session 0        
		//error = NurApiInventory(m_hNur, 500, 3, 0, &invResp);        
		
		// perform simple inventory        
		error = NurApiSimpleInventory(m_hNur, &invResp);        
		
		if (error != NUR_NO_ERROR && error != NUR_ERROR_NO_TAG)        
		{            
			// failed            
			return error;        
		}    
	}    
	_tprintf(_T(". %d tags found\r\n"), invResp.numTagsMem);    
	
	// fetch tags from module, including tag meta    
	error = NurApiFetchTags(m_hNur, TRUE, NULL);    
	if (error != NUR_NO_ERROR)    
	{        
		// failed        
		return error;    
	}    
	
	error = NurApiGetTagCount(m_hNur, &tagCount);    
	if (error != NUR_NO_ERROR)    
	{        
		// failed        
		return error;    
	}    
	
	// loop through tags    
	for (idx = 0; idx < tagCount; idx++)    
	{               
		error = NurApiGetTagData(m_hNur, idx, &tagData);        
		if (error == NUR_NO_ERROR)        
		{
			// copy tag to storage
			m_tag_storage[m_tag_storage_size++] = tagData;

			// get EPC string
			EpcToString(tagData.epc, tagData.epcLen, epcStr);            
			
			// print tag info            
			_tprintf(_T("Tag info:\r\n"));            
			_tprintf(_T("  EPC: [%s]\r\n"), epcStr);            
			_tprintf(_T("  EPC length: %d\r\n"), tagData.epcLen);            
			_tprintf(_T("  RSSI: %d\r\n"), tagData.scaledRssi);            
			_tprintf(_T("  Timestamp: %u\r\n"), tagData.timestamp);            
			_tprintf(_T("  Frequency: %u\r\n"), tagData.freq);            
			_tprintf(_T("  PC bytes: %04x\r\n"), tagData.pc);        
		}        
		else        
		{            
			// print error            
			TCHAR errorMsg[128+1]; // 128 + NULL                
			NurApiGetErrorMessage(error, errorMsg, 128);            
			_tprintf(_T("NurApiTSGetTagData() returns error %d: [%s]\r\n"), error, errorMsg);        
		}    
	}

	return NUR_NO_ERROR;
}

void CNordicIdDevice::setScaning(bool value)
{
	if (value && !m_scaning)
    {
        m_scaning = true;
        //NurApiStartSimpleInventoryStream( m_hNur );  
		PerformInventory();

    }
    else if (!value && m_scaning)
    {
        m_scaning = false;
        //NurApiStopInventoryStream( m_hNur );  
    }
}

// Get nearest tag (with max scaled RSSI)
//---------------------------------------------------------------------------//
ITag* CNordicIdDevice::GetNearestTag()
{
	if (m_tag_storage_size == 0)
		PerformInventory();

	if (m_tag_storage_size > 0)
	{
		BYTE maxScaledRSSI = 0;
		int indexOfMaxScaledRSSI = 0;
		
		for (int i = 0; i < m_tag_storage_size; i++)
		{
			if (m_tag_storage[i].scaledRssi > maxScaledRSSI)
			{
				maxScaledRSSI = m_tag_storage[i].scaledRssi;
				indexOfMaxScaledRSSI = i;
			}
		}

		ITag* result = new CRFIDTag();
		result->setBuffer(m_tag_storage[indexOfMaxScaledRSSI].epc);
		return result;
	}
	else
		return NULL;
}

// Write tag
//---------------------------------------------------------------------------//
IDevice::EResponse CNordicIdDevice::WriteTag(ITag* tag)
{
	IDevice::EResponse response = IDevice::EResponse::Unknown;
	Clear();
	ITag* nearestTag = GetNearestTag();

	if (!nearestTag)
	{
		response = IDevice::EResponse::TagNotFound;
	}
	else
	{
		int error = NurApiWriteEPC( 
			m_hNur,							// hApi
			0,								// passwd
			false,							// secured
			1,								// sBank == EPC_BANK
			32,								// sAddress
			CRFIDTag::EPC_SIZE << 3,		// sMaskBitLength
			(BYTE*)nearestTag->getBuffer(), // sMask
			(BYTE*)tag->getBuffer(),        // newEpcBuffer
			CRFIDTag::EPC_SIZE				// newEpcBufferLen
			);

		if (error != NUR_NO_ERROR)
			response = IDevice::EResponse::WriteError;
		else
			response = IDevice::EResponse::Successful;
	}
	
	Clear();
	
	return response;
}

// Device serial number
//---------------------------------------------------------------------------//
std::wstring CNordicIdDevice::getSerial()
{
	struct NUR_READERINFO info;
	m_error = NurApiGetReaderInfo(m_hNur, &info, sizeof(info));
	wchar_t* serial = &info.serial[0];
	return std::wstring(serial);
}

// Device is writable
bool CNordicIdDevice::getWritable()
{
	return true;
}
    
// METHODS
//---------------------------------------------------------------------------//
// Connect to device
bool CNordicIdDevice::Connect()
{
	if (m_connected)
		return true;

	if (m_hNur == INVALID_HANDLE_VALUE)  
		return false;

	// Set notification callback    
	NurApiSetNotificationCallback(m_hNur, MyNotificationFunc);
	
	// Set full log level, w/o data    
	// NurApiSetLogLevel(m_hNur, NUR_LOG_ALL & ~NUR_LOG_DATA);	
	NurApiSetLogLevel(m_hNur, NUR_LOG_ERROR);

#ifdef USE_USB_AUTO_CONNECT    
	NurApiSetUsbAutoConnect(m_hNur, TRUE);    
	m_error = NurApiIsConnected(m_hNur);
#else    
	// Connect API to module through serial port    
	error = NurApiConnectSerialPort(m_hNur, 10, NUR_DEFAULT_BAUDRATE);
#endif

	if (m_error != NUR_NO_ERROR)    
	{        
		return false;    
	}
	
	m_error = NurApiPing(m_hNur, NULL);    
	if (m_error != NUR_NO_ERROR)    
	{
		return false;
	}
	
	// API ready
	m_connected = true;
	return true;
}

// Clear tag storage
//---------------------------------------------------------------------------//
void CNordicIdDevice::Clear()
{
	// Clear previously inventoried tags from memory    
	m_error = NurApiClearTags(m_hNur);    
	m_tag_storage_size = 0;
}
//---------------------------------------------------------------------------//
void NURAPICALLBACK MyNotificationFunc(
	HANDLE m_hNur, 
	DWORD timestamp, 
	int type, 
	LPVOID data, 
	int dataLen)
{
	switch (type)    
	{    
	case NUR_NOTIFICATION_LOG:        
		{            
			const TCHAR *logMsg = (const TCHAR *)data;            
			_tprintf(_T("LOG: %s\r\n"), logMsg);        
		}        
		break;

	case NUR_NOTIFICATION_INVENTORYSTREAM:
		{
		}
		break;

	case NUR_NOTIFICATION_MODULEBOOT:        
		_tprintf(_T("Module booted\r\n"));        
		break;    
	
	case NUR_NOTIFICATION_TRCONNECTED:        
		_tprintf(_T("Transport connected\r\n"));        
		break;    
	
	case NUR_NOTIFICATION_TRDISCONNECTED:        
		_tprintf(_T("Transport disconnected\r\n"));        
		break;    
	
	default:        
		_tprintf(_T("Unhandled notification: %d\r\n"), type);        
		break;    
	}
}
