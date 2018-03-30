// File: RFIDTag.cpp
// Program: RFIDevolution
// Version 1.0
// Copyright © Pavel Nikitin 2013
#include "RFIDTag.h"
#include <string>
#include <Windows.h>

// CONSTRUCTORS & DESTRUCTORS
//---------------------------------------------------------------------------//
CRFIDTag::CRFIDTag()
{
	ZeroMemory(&m_epc, 12);
}

CRFIDTag::~CRFIDTag()
{
}

// PROPERTIES
//---------------------------------------------------------------------------//
// Tag type
//---------------------------------------------------------------------------//
ITag::EType CRFIDTag::getType()
{
	return (ITag::EType)m_epc[0];
}

void CRFIDTag::setType(ITag::EType value)
{
    m_epc[0] = (char)value;
}

// Company identificator 40-bit
//---------------------------------------------------------------------------//
__int64 CRFIDTag::getCompanyId()
{
    __int64 value = 0;
	memcpy(&value, &(m_epc[2]), 5);
    return value;
}

void CRFIDTag::setCompanyId(__int64 value)
{
	memcpy(&m_epc[2], &value, 5);
}

// Tag identificator 40-bit
//---------------------------------------------------------------------------//
__int64 CRFIDTag::getId()
{
    __int64 value = 0;
	memcpy(&value, &m_epc[7], 5);
    return value;
}

void CRFIDTag::setId(__int64 value)
{
	memcpy(&m_epc[7], &value, 5);
}

// Binary tag data representation 96-bit
//---------------------------------------------------------------------------//
void* CRFIDTag::getBuffer()
{
	return &m_epc[0];
}

void CRFIDTag::setBuffer(void* value)
{
	memcpy(&m_epc[0], value, 12);	
}
