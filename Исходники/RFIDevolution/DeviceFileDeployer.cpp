// Implementation of device file deployer on RAPI
// Author: Pavel Nikitin (c) 2013
#include "DeviceFileDeployer.h"

using namespace std;

// Init Singleton
CDeviceFileDeployer* CDeviceFileDeployer::m_self = NULL;

// Constructor
CDeviceFileDeployer::CDeviceFileDeployer()
{
	// members init
	m_connected = false;
	m_device_info_captured = false;
	m_rapi_desktop = NULL;
	m_rapi_device_list = NULL;
	m_rapi_device = NULL;
	m_rapi_session = NULL;
}

// Destructor
CDeviceFileDeployer::~CDeviceFileDeployer()
{
	if (m_self)
	{
		Disconnect();
		m_self = NULL;
	}
}

// Copy file from desktop to device
DWORD CDeviceFileDeployer::SendFile(
	wstring sourceDesktopFileName, 
	wstring destinationDeviceFileName )
{
	DWORD res = Connect();
	if (res != 0)
		return res;

	// from desktop
	HANDLE hReadFile = CreateFile( 
		sourceDesktopFileName.c_str(),
		GENERIC_READ,
		0, // the file cannot be shared
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		NULL );

	if (hReadFile == INVALID_HANDLE_VALUE)
		return 10;

	// to device
	HANDLE hWriteFile = m_rapi_session->CeCreateFile( 
		destinationDeviceFileName.c_str(),
		GENERIC_WRITE,
		0, // the file cannot be shared
		NULL,
		CREATE_ALWAYS,
		FILE_ATTRIBUTE_NORMAL,
		NULL );

	if (hWriteFile == INVALID_HANDLE_VALUE)
	{	
		//DWORD last_error = m_rapi_session->CeGetLastError();
		CloseHandle(hReadFile);
		return 11;
	}

	// get file size
	DWORD dwSizeHigh = 0, dwSizeLow = 0, dwProcessBytes = 0;
	dwSizeLow = GetFileSize(hReadFile, &dwSizeHigh);
	
	// copy file from desktop to device
	DWORD error = 0;
	if (dwSizeLow > 0)
	{
		char* buffer = new char[dwSizeLow];

		// read file to buffer
		HRESULT hResult = ReadFile(
			hReadFile,
			buffer,
			dwSizeLow,
			&dwProcessBytes,
			NULL );

		
		// if file readed then write
		if( (hResult == S_FALSE) && (dwSizeLow == dwProcessBytes) )
		{
			hResult = m_rapi_session->CeWriteFile(
				hWriteFile,
				buffer,
				dwSizeLow,
				&dwProcessBytes,
				NULL );
		}

		delete[] buffer;
		
		if( (hResult != S_FALSE) || (dwSizeLow != dwProcessBytes) )
			error = 12;
	}

	// close files
	CloseHandle(hReadFile);
	m_rapi_session->CeCloseHandle(hWriteFile);

	Disconnect();

	return error;
}

// Copy file from device to desktop
DWORD CDeviceFileDeployer::ReceiveFile(
	wstring sourceDeviceFileName, 
	wstring destinationDesktopFileName )
{
	DWORD res = Connect();
	if (res != 0)
		return res;

	// from device
	HANDLE hReadFile = m_rapi_session->CeCreateFile( 
		sourceDeviceFileName.c_str(),
		GENERIC_READ,
		0, // the file cannot be shared
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		NULL );

	if (hReadFile == INVALID_HANDLE_VALUE)
		return 6;

	// to desktop
	HANDLE hWriteFile = CreateFile( 
		destinationDesktopFileName.c_str(),
		GENERIC_WRITE,
		0, // the file cannot be shared
		NULL,
		CREATE_ALWAYS,
		FILE_ATTRIBUTE_NORMAL,
		NULL );

	if (hWriteFile == INVALID_HANDLE_VALUE)
	{
		m_rapi_session->CeCloseHandle(hReadFile);
		return 7;
	}

	// get file size
	DWORD dwSizeHigh = 0, dwSizeLow = 0, dwProcessBytes = 0;
	dwSizeLow = m_rapi_session->CeGetFileSize(hReadFile, &dwSizeHigh);
	
	// copy file from desktop to device
	DWORD error = 0;
	if (dwSizeLow > 0)
	{
		char* buffer = new char[dwSizeLow];

		// read file to buffer
		HRESULT hResult = m_rapi_session->CeReadFile(
			hReadFile,
			buffer,
			dwSizeLow,
			&dwProcessBytes,
			NULL );

		
		// if file readed then write
		if( (hResult == S_FALSE) && (dwSizeLow == dwProcessBytes) )
		{
			hResult = WriteFile(
				hWriteFile,
				buffer,
				dwSizeLow,
				&dwProcessBytes,
				NULL );
		}

		delete[] buffer;
		
		if( (hResult != S_FALSE) || (dwSizeLow != dwProcessBytes) )
			error = 8;
	}

	// close files
	m_rapi_session->CeCloseHandle(hReadFile);
	CloseHandle(hWriteFile);

	Disconnect();

	return error;
}

// Get device name
wstring CDeviceFileDeployer::GetDeviceName()
{
	if (GetDeviceInfo())
		return wstring(m_device_info.bstrName);
	else
		return L"N/A";
}

// Get device platform name
wstring CDeviceFileDeployer::GetDevicePlatform()
{
	if (GetDeviceInfo())
		return wstring(m_device_info.bstrPlatform);
	else
		return L"N/A";
}

// Connect to device
DWORD CDeviceFileDeployer::Connect()
{
	Disconnect();

	HRESULT hr = S_OK;

	// create RAPI desktop instance
	hr = m_rapi_desktop.CoCreateInstance( CLSID_RAPI );
    if( FAILED( hr ) )
        return 1;

	// enum devices
	hr = m_rapi_desktop->EnumDevices( &m_rapi_device_list );
    if( FAILED( hr ) )
	{
		m_rapi_desktop = NULL;
        return 2;
	}

    // get first device
	hr = m_rapi_device_list->Next( &m_rapi_device );

    if( FAILED( hr ) )
	{
		m_rapi_device_list = NULL;
		m_rapi_desktop = NULL;
        return 3;
	}

    // create session
	hr = m_rapi_device->CreateSession( &m_rapi_session );
    if( FAILED( hr ) )
	{
		m_rapi_device = NULL;
		m_rapi_device_list = NULL;
		m_rapi_desktop = NULL;
        return 4;
	}

    // init session
	hr = m_rapi_session->CeRapiInit();
    if( FAILED( hr ) )
	{
		m_rapi_session = NULL;
		m_rapi_device = NULL;
		m_rapi_device_list = NULL;
		m_rapi_desktop = NULL;
        return 5;
	}
		
	m_connected = true;
	return 0;
}

// Disconnect from device
bool CDeviceFileDeployer::Disconnect()
{
	if (m_rapi_session != NULL)
	{
		m_rapi_session->CeRapiUninit();
		m_rapi_session = NULL;
	}

	if (m_rapi_device != NULL)
	{
		m_rapi_device = NULL;
	}
	
	if (m_rapi_device_list != NULL)
	{
		m_rapi_device_list->Reset();
		m_rapi_device_list = NULL;
	}

	if (m_rapi_desktop != NULL)
	{
		m_rapi_desktop = NULL;
	}

	m_connected = false;

	return true;
}

// Get device info
bool CDeviceFileDeployer::GetDeviceInfo()
{
	if (Connect() != 0)
		return false;

	if (!m_device_info_captured)
	{
		m_rapi_device->GetDeviceInfo( &m_device_info );
		m_device_info_captured = true;
	}

	Disconnect();

	return true;
}
