// Interface of a file deployer 
// Author: Pavel Nikitin (c) 2013
#pragma once
#include <string>

// Interface of the file deployer 
class IFileDeployer
{
public:
    virtual unsigned long SendFile(
		std::wstring sourceFileName, 
		std::wstring destinationFileName) = 0;
    
	virtual unsigned long ReceiveFile(
		std::wstring sourceFileName, 
		std::wstring destinationFileName) = 0;

		// Get device name 
	virtual std::wstring GetDeviceName() = 0;

	// Get device platform name
	virtual std::wstring GetDevicePlatform() = 0;

	virtual ~IFileDeployer() {}
};
