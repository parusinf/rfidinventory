// Implementation of device file deployer on RAPI
// Author: Pavel Nikitin (c) 2013
#pragma once
#include "IFileDeployer.h"
#include <atlbase.h>
#include <rapi2.h>

// Implementation of device file deployer by RAPI
class CDeviceFileDeployer : public IFileDeployer
{
public:
	// Get Singleton Instance
	static CDeviceFileDeployer* CreateInstance()
	{
		if (!m_self)
			m_self = new CDeviceFileDeployer();
		return m_self;
	}

// PROPERTIES
//---------------------------------------------------------------------------//
	// Get device name 
	std::wstring GetDeviceName();

	// Get device platform name
	std::wstring GetDevicePlatform();


// METHODS
//---------------------------------------------------------------------------//
	// Copy file from desktop to device
	virtual DWORD SendFile(
		std::wstring sourceDesktopFileName, 
		std::wstring destinationDeviceFileName );
    
	// Copy file from device to desktop
	virtual DWORD ReceiveFile(
		std::wstring sourceDeviceFileName, 
		std::wstring destinationDesktopFileName );

	// Destructor
	~CDeviceFileDeployer();

protected:
	// Constructor protected for Singleton
	CDeviceFileDeployer();

private:
	// Singleton pattern
	static CDeviceFileDeployer* m_self;
	
	// Private members
	bool						m_connected;
	bool						m_device_info_captured;
	CComPtr< IRAPIDesktop >		m_rapi_desktop;
	CComPtr< IRAPIEnumDevices > m_rapi_device_list;
	CComPtr< IRAPIDevice >		m_rapi_device;
	CComPtr< IRAPISession >		m_rapi_session;
	RAPI_DEVICEINFO				m_device_info;

	// Private methods
	DWORD Connect();
	bool Disconnect();
	bool GetDeviceInfo();
};
