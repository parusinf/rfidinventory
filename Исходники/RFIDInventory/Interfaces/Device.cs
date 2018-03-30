// File: Device.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;


namespace ru.nikitin.RFIDInventory.Interfaces
{
    // Process device connected event
    public delegate void DeviceConnectedDelegate();

    // Process tag after read from device
    public delegate void ProcessTagDelegate(ITag tag);

    // Operation response
    public enum Response : byte
    {
        Unknown,
        Successful,
        TagNotFound,
        WriteError
    }

    // Device vendor
    public enum DeviceVendor : byte
    {
        Unknown,
        NordicID,
        Motorola
    }

    // Led colors 
    public enum LedColor
    {
        Off = 0,
        Green = 1,
        Orange = 2,
        Red = 3
    }

    // abstract inventory device
    public interface IDevice : IDisposable 
    {
        // device init successful
        bool Connected { get; }
        
        // scaning in process
        bool Scaning { get; set; }
        
        // device serial number
        string Serial { get; }

        // device is writable
        bool Writable { get; }

        // device vendor
        DeviceVendor Vendor { get; }

        // device path
        string FlashPath { get; }
        
        // write tag
        Response WriteTag(ITag tag);

        // clear tag storage
        void Clear();

        // program led
        void Led(LedColor color);
    }
}
