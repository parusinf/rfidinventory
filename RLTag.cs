// File: RFIDTag.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;

namespace ru.nikitin.RFIDInventory
{
    class RLTag : ITag
    {
        // Binary storage of EPC memory
        byte[] m_epc;

        // Create tag by epc buffer
        public RLTag(byte[] epc)
        {
            if (epc.Length < 12)
                throw new ArgumentOutOfRangeException(
                    "EPC must contain at least 12 bytes");

            m_epc = new byte[12];
            Array.Copy(epc, m_epc, 12);
        }

        // Create tag by tag type, company identificator and object (location) identificator
        public RLTag(TagType type, ulong companyId, ulong id)
        {
            m_epc = new byte[12];
            this.Type = type;
            this.CompanyId = companyId;
            this.Id = id;
        }

        // Create tag by string barcode
        public RLTag(string barcode)
        {
            m_epc = new byte[12];
            string bc = barcode.PadRight(25, '0').Substring(0, 25);

            for (int i = 0; i < bc.Length; i++)
                if (bc[i] < '0' || bc[i] > '9')
                {
                    this.Type = TagType.Unknown;
                    return;
                }

            if (bc.Substring(0, 5) == "02001")
                this.Type = TagType.Object;
            else if (bc.Substring(0, 5) == "04001")
                this.Type = TagType.Location;
            else
                this.Type = TagType.Unknown;

            try
            {
                this.CompanyId = UInt64.Parse(bc.Substring(5, 8));
                this.Id = UInt64.Parse(bc.Substring(13, 12));
            }
            catch (FormatException)
            {
                this.Type = TagType.Unknown;
            }
        }

        // Tag type
        public TagType Type
        {
            get
            {
                TagType type;

                switch (m_epc[0])
                {
                    case 1:
                        type = TagType.Object;
                        break;

                    case 2:
                        type = TagType.Location;
                        break;

                    default:
                        type = TagType.Unknown;
                        break;
                }

                return type;
            }

            set
            {
                switch (value)
                {
                    case TagType.Object:
                        m_epc[0] = 1;
                        break;

                    case TagType.Location:
                        m_epc[0] = 2;
                        break;

                    default:
                        m_epc[0] = 0;
                        break;
                }
            }
        }

        // Company identificator
        public ulong CompanyId
        {
            get
            {
                byte[] buffer = new byte[8];
                Array.Copy(m_epc, 2, buffer, 0, 5);
                return BitConverter.ToUInt64(buffer, 0);
            }

            set
            {
                Array.Copy(BitConverter.GetBytes(value), 0, m_epc, 2, 5);
            }
        }

        // Object or location identificator
        public ulong Id
        {
            get
            {
                byte[] buffer = new byte[8];
                Array.Copy(m_epc, 7, buffer, 0, 5);
                return BitConverter.ToUInt64(buffer, 0);
            }

            set
            {
                Array.Copy(BitConverter.GetBytes(value), 0, m_epc, 7, 5);
            }
        }

        // Bit tag buffer
        public byte[] Buffer
        {
            get
            {
                return m_epc;
            }
            set
            {
                if (value.Length < 12)
                    throw new ArgumentOutOfRangeException(
                        "EPC must contain at least 12 bytes");

                m_epc = value;
            }
        }
    }
}
