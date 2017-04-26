// Implements Nordic ID device
// Copyright © Pavel Nikitin 2013
#pragma once
#include "IDevice.h"
#include "ITag.h"
#include "NurAPI.h"

// Nordic ID device implementation
//---------------------------------------------------------------------------//
class CNordicIdDevice : public IDevice
{
public:
	static const int MAX_TAG_STORAGE_SIZE = 150;

	// Get Singleton Instance
	static CNordicIdDevice* CreateInstance()
	{
		if (!m_self)
			m_self = new CNordicIdDevice();
		return m_self;
	}

	~CNordicIdDevice();

// PROPERTIES
//---------------------------------------------------------------------------//
	// Device init successful
    virtual bool getConnected();
    
    // Scaning in process
    virtual bool getScaning();
	virtual void setScaning(bool);

    // Device serial number
    virtual std::wstring getSerial();

    // Device is writable
    virtual bool getWritable();

	// Tag storage
	NUR_TAG_DATA m_tag_storage[MAX_TAG_STORAGE_SIZE];
	int m_tag_storage_size;

    
// METHODS
//---------------------------------------------------------------------------//
	// Connect to device
	virtual bool Connect(); 

	// Write tag
    virtual EResponse WriteTag(ITag*);

    // Clear tag storage
    virtual void Clear();

	// Get nearest tag (with max scaled RSSI)
	ITag* GetNearestTag();

protected:
	// Constructor protected for Singleton
	CNordicIdDevice();

private:
// PROPERTIES
//---------------------------------------------------------------------------//
	// Singleton pattern
	static CNordicIdDevice* m_self;

	// Private members
	bool m_connected;
	bool m_scaning;
	int m_error;
	void* m_hNur;

// METHODS
//---------------------------------------------------------------------------//
	// Convert EPC byte array to string
	void EpcToString(BYTE *epc, DWORD epcLen, TCHAR *epcStr);
	// Perform inventory
	int PerformInventory();
};
