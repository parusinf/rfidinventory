// File: ITag.h
// Program: RFIDevolution
// Version 1.0
// Copyright © Pavel Nikitin 2013
#pragma once

// Abstract tag interface
//---------------------------------------------------------------------------//
class ITag
{
public:
// ENUMS
//---------------------------------------------------------------------------//
	enum EType
    {
        Unknown = 0,
        Object,
        Location
    };

// CONSTRUCTORS & DESTRUCTORS
//---------------------------------------------------------------------------//
	virtual ~ITag(void) {}

// PROPERTIES
//---------------------------------------------------------------------------//
	// Tag type
	virtual EType getType() = 0;
	virtual void setType(EType) = 0;
	
	// Company identificator
	virtual __int64 getCompanyId() = 0;
	virtual void setCompanyId(__int64) = 0;

	// Tag identificator
    virtual __int64 getId() = 0;
	virtual void setId(__int64) = 0;

	// Binary tag data representation
    virtual void* getBuffer() = 0;
	virtual void setBuffer(void*) = 0;
};
