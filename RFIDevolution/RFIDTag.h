// File: RFIDTag.h
// Program: RFIDevolution
// Version 1.0
// Copyright © Pavel Nikitin 2013
#pragma once
#include "itag.h"

// RFID tag implementation
//---------------------------------------------------------------------------//
class CRFIDTag : public ITag
{
public:
	static const int EPC_SIZE = 12;

// CONSTRUCTORS & DESTRUCTORS
//---------------------------------------------------------------------------//
	CRFIDTag();
	~CRFIDTag();

// PROPERTIES
//---------------------------------------------------------------------------//
	// Tag type
	virtual ITag::EType getType();
	virtual void setType(ITag::EType);
	
	// Company identificator 40-bit
	virtual __int64 getCompanyId();
	virtual void setCompanyId(__int64);

	// Tag identificator 40-bit
    virtual __int64 getId();
	virtual void setId(__int64);

	// Binary tag data representation 96-bit
    virtual void* getBuffer();
	virtual void setBuffer(void*);

private:
	unsigned char m_epc[EPC_SIZE];
};
