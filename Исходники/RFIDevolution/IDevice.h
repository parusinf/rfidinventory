// File: IDevice.h
// Program: RFIDevolution
// Version 1.0
// Copyright © Pavel Nikitin 2013
#pragma once
#include <string>
#include "itag.h"

// Abstract device interface
//---------------------------------------------------------------------------//
class IDevice
{
public:
// ENUMS
//---------------------------------------------------------------------------//
	enum EResponse
    {
        Unknown = 0,
        Successful,
        TagNotFound,
        WriteError
    };
    
// CONSTRUCTORS & DESTRUCTORS
//---------------------------------------------------------------------------//
	virtual ~IDevice() {}
	
// PROPERTIES
//---------------------------------------------------------------------------//
	// Device init successful
    virtual bool getConnected() = 0;
    
    // Scaning in process
    virtual bool getScaning() = 0;
	virtual void setScaning(bool) = 0;

    // Device serial number
	virtual std::wstring getSerial() = 0;

    // Device is writable
    virtual bool getWritable() = 0;
    
// METHODS
//---------------------------------------------------------------------------//
	// Connect to device
	virtual bool Connect() = 0; 

	virtual ITag* GetNearestTag() = 0;

	// Write tag
    virtual EResponse WriteTag(ITag*) = 0;

    // Clear tag storage
    virtual void Clear() = 0;
};
