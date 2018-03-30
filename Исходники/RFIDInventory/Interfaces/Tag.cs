// File: Tag.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.nikitin.RFIDInventory
{
    // Tag type
    public enum TagType : byte
    {
        Unknown,
        Object,
        Location
    }

    // abstract tag
    public interface ITag
    {
        TagType Type { get; set; }
        ulong CompanyId { get; set; }
        ulong Id { get; set; }
        byte[] Buffer { get; set; }
    }
}
