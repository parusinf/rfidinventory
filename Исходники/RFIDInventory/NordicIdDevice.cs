// File: NordicIdDevice.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;

using ru.nikitin.RFIDInventory.Interfaces;
using NurApiDotNet;
using NordicId;

namespace ru.nikitin.RFIDInventory
{
    class NordicIdDevice : IDevice
    {
        NurApi m_nur = null;
        NurApi.TagStorage m_tagStorage = null;
        ScanHelper m_laser = null;
        MHLDriver m_mhlUtil = null;
        NordicKeyLightWave m_imagingScan = null;

        NurApi.ReaderInfo m_readerInfo;
        bool m_connected = false;
        bool m_scaning = false;
        ProcessTagDelegate m_processTagDelegate;
        DeviceConnectedDelegate m_startupDelegate;

        
        // Device interface: Connected
        public bool Connected
        {
            get
            {
                return m_connected;
            }
        }

        // Device interface: Inventory state
        public bool Scaning
        {
            get
            {
                return m_scaning;
            }

            set
            {
                if (value && !m_scaning)
                {
                    m_scaning = true;
                    m_imagingScan.Enabled = true;
                    m_nur.StartInventoryStream();
                }
                else if (!value && m_scaning)
                {
                    m_scaning = false;
                    m_imagingScan.Enabled = false;
                    m_nur.StopInventoryStream();
                }
            }
        }

        // Device interface: device serial number
        public string Serial
        {
            get
            {
                return m_readerInfo.serial;
            }
        }

        // Device interface: device is writable
        public bool Writable
        {
            get
            {
                return true;
            }
        }

        // Device vendor
        public DeviceVendor Vendor
        {
            get
            {
                return DeviceVendor.NordicID;
            }
        }

        // Device flash path
        public string FlashPath
        {
            get
            {
                return "\\Flash\\";
            }
        }

        // Create and init device
        public NordicIdDevice(System.Windows.Forms.Form form, 
            DeviceConnectedDelegate startupDelegate, 
            ProcessTagDelegate processTagDelegate)
        {
            // delegates
            m_startupDelegate = startupDelegate;
            m_processTagDelegate = processTagDelegate;

            
            // create NurApi object with notifications receiver
            m_nur = new NurApi(form);

            
            // this gets called when module is connected
            m_nur.ConnectedEvent += new EventHandler<NurApi.NurEventArgs>(
                nur_ConnectedEvent);

            
            // this gets called when module is disconnected
            m_nur.DisconnectedEvent += new EventHandler<NurApi.NurEventArgs>(
                nur_DisconnectedEvent);

            
            // this gets called when new tags arrives
            m_nur.InventoryStreamEvent += 
                new EventHandler<NurApi.InventoryStreamEventArgs>(
                    nur_InventoryStreamEvent);

            
            // connect to integrated reader
            m_nur.ConnectIntegratedReader();

            // create tag storage
            m_tagStorage = new NurApi.TagStorage();
            
            // Initialize the scan helper, this starts the receive queue thread
            // and makes the scan wedge redirect output to the queue
            //
            // The Laser, Imager and RFID drivers all use the scan wedge to output
            // stuff to the cursor, so this will intercept that output for all those
            // drivers. All Wedge settings such as prefix, postfix and character 
            // replacement take effect before passing the code to you.
            //
            // It takes your form class instance and your ScanResult delegate function as an argument.
            // ScanHelper will use Invoke to call ScanResult delegate, so it is executed in UI thread.
            //
            // ScanHelper will dispose it self when attached form is disposed. 
            // You can also call directly ShutDownScanHelper() to stop ScanHelper immediately
            m_laser = new ScanHelper();
            m_laser.Initialize(form, mhl_InventoryBarcodeEvent);


            // create MHL Utility driver
            m_mhlUtil = new MHLDriver("Utility");
            m_mhlUtil.Open("Utility");

            // create Imaging scan object
            m_imagingScan = new NordicKeyLightWave();

            // setup for Russia
            m_nur.BuildCustomHoptable(866000, 4, 600, 100, 100, 256000, 1, false);
            NurApi.ModuleSetup newSetup = m_nur.GetModuleSetup();
            newSetup.regionId = NurApi.REGIONID_CUSTOM;
            newSetup.txLevel = 7;
            // Set settings to NurApi
            m_nur.SetModuleSetup(NurApi.SETUP_ALL, ref newSetup);
        }

        // Write tag
        public Response WriteTag(ITag tag)
        {
            // init response
            Response response = Response.Unknown;

            // clear previously inventoried tags from memory
            m_nur.ClearTags();

            // perform simple inventory
            m_nur.SimpleInventory();

            // fetch tags from module, including tag meta
            NurApi.TagStorage tags = m_nur.FetchTags(true);

            // find near tag
            if (tags.Count > 0)
            {
                // find maximum RSSI
                int tagIndex = 0;
                sbyte maxRssi = -1;
                for (int i = 0; i < tags.Count; i++)
                    if (tags[i].scaledRssi > maxRssi)
                    {
                        maxRssi = tags[i].scaledRssi;
                        tagIndex = i;
                    }

                // write
                try
                {
                    m_nur.WriteEPC(0, false, NurApi.BANK_EPC, 32,
                        tags[tagIndex].epc.Length * 8, tags[tagIndex].epc, tag.Buffer);

                    response = Response.Successful;
                    Tools.MessageBeep(BeepType.MB_OK);
                }
                catch (Exception)
                {
                    response = Response.WriteError;
                    Tools.MessageBeep(BeepType.MB_ICONASTERISK);
                }

            }
            else
            {
                response = Response.TagNotFound;
                Tools.MessageBeep(BeepType.MB_ICONASTERISK);
            }

            return response;
        }

        // Clear tag storage
        public void Clear()
        {
            m_tagStorage.Clear();
            m_nur.ClearTags();
        }

        // IDisposable
        public void Dispose()
        {
            m_nur.Dispose();
            Led(LedColor.Off);
            m_mhlUtil.Close();
        }

        // Manage Programmable Led
        public void Led(LedColor color)
        {
            m_mhlUtil.SetDword("Utility.ProgrammableLed", (uint)color);
        }
        
        // This gets called when module is connected
        private void nur_ConnectedEvent(object sender, NurApi.NurEventArgs e)
        {
            // Connected, module info
            m_readerInfo = m_nur.GetReaderInfo();
            m_connected = true;
            m_startupDelegate();
        }

        // This gets called when module is disconnected
        private void nur_DisconnectedEvent(object sender, NurApi.NurEventArgs e)
        {
            m_connected = false;
        }

        // This gets called when new tags arrives
        private void nur_InventoryStreamEvent(object sender, 
            NurApi.InventoryStreamEventArgs e)
        {
            foreach (NurApi.Tag epcTag in m_nur.GetTagStorage())
            {
                ITag tag = new RLTag(epcTag.epc);

                // copy to application tag storage
                if (tag.Type == TagType.Location || tag.Type == 
                    TagType.Object && m_tagStorage.AddTag(epcTag))
                {
                    // perform, if tag was new unique tag
                    m_processTagDelegate(tag);
                }
            }

            if (e.data.stopped && m_scaning)
            {
                // restart streaming
                try
                {
                    m_nur.StartInventoryStream();
                }
                catch { } // ignore error..
            }
        }

        // This gets called when new barcode arrives
        private void mhl_InventoryBarcodeEvent(string barcode)
        {
            ITag tag = new RLTag(barcode);
            m_processTagDelegate(tag);
        }
    }
}
