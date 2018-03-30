// File: MotorolaDevice.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Windows.Forms;
using ru.nikitin.RFIDInventory.Interfaces;
using Symbol.Barcode2;

namespace ru.nikitin.RFIDInventory
{
    class MotorolaDevice : IDevice
    {
        bool m_connected = false;
        bool m_scaning = false;
        ProcessTagDelegate m_processTagDelegate;

        // Parent form object
        private Form m_parent;
        // Laser object
        private Symbol.Barcode2.Design.Barcode2 m_laser = null;
        // Device info structure
        private Symbol.ResourceCoordination.TerminalInfo m_info;

        // scan time in milliseconds
        private const int SCAN_TIMEOUT = 2500;
        
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
                    //m_laser.EnableScanner = true;
                }
                else if (!value && m_scaning)
                {
                    m_scaning = false;
                    //m_laser.EnableScanner = false;
                }
            }
        }

        // Device interface: device serial number
        public string Serial
        {
            get
            {
                return m_info.ESN;
            }
        }

        // Device interface: device is writable
        public bool Writable
        {
            get
            {
                return false;
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
                return "\\Application\\";
            }
        }

        // Create and init device
        public MotorolaDevice(System.Windows.Forms.Form form,
            ProcessTagDelegate processTagDelegate)
        {
            m_parent = form;
            m_processTagDelegate = processTagDelegate;

            // resources
            System.ComponentModel.ComponentResourceManager resources = 
                new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

            // create equipment
            m_laser = new Symbol.Barcode2.Design.Barcode2();
            m_info = new Symbol.ResourceCoordination.TerminalInfo();

            // configuration
            m_laser.Config.DecoderParameters.CODABAR = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.CODABARParams.ClsiEditing = false;
            m_laser.Config.DecoderParameters.CODABARParams.NotisEditing = false;
            m_laser.Config.DecoderParameters.CODABARParams.Redundancy = true;
            m_laser.Config.DecoderParameters.CODE128 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.CODE128Params.EAN128 = true;
            m_laser.Config.DecoderParameters.CODE128Params.ISBT128 = true;
            m_laser.Config.DecoderParameters.CODE128Params.Other128 = true;
            m_laser.Config.DecoderParameters.CODE128Params.Redundancy = false;
            m_laser.Config.DecoderParameters.CODE39 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.CODE39Params.Code32Prefix = false;
            m_laser.Config.DecoderParameters.CODE39Params.Concatenation = false;
            m_laser.Config.DecoderParameters.CODE39Params.ConvertToCode32 = false;
            m_laser.Config.DecoderParameters.CODE39Params.FullAscii = false;
            m_laser.Config.DecoderParameters.CODE39Params.Redundancy = false;
            m_laser.Config.DecoderParameters.CODE39Params.ReportCheckDigit = false;
            m_laser.Config.DecoderParameters.CODE39Params.VerifyCheckDigit = false;
            m_laser.Config.DecoderParameters.CODE93 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.CODE93Params.Redundancy = false;
            m_laser.Config.DecoderParameters.D2OF5 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.D2OF5Params.Redundancy = true;
            m_laser.Config.DecoderParameters.EAN13 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.EAN8 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.EAN8Params.ConvertToEAN13 = false;
            m_laser.Config.DecoderParameters.I2OF5 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.I2OF5Params.ConvertToEAN13 = false;
            m_laser.Config.DecoderParameters.I2OF5Params.Redundancy = true;
            m_laser.Config.DecoderParameters.I2OF5Params.ReportCheckDigit = false;
            m_laser.Config.DecoderParameters.I2OF5Params.VerifyCheckDigit = Symbol.Barcode2.Design.I2OF5.CheckDigitSchemes.Default;
            m_laser.Config.DecoderParameters.KOREAN_3OF5 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.KOREAN_3OF5Params.Redundancy = true;
            m_laser.Config.DecoderParameters.MSI = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.MSIParams.CheckDigitCount = Symbol.Barcode2.Design.CheckDigitCounts.Default;
            m_laser.Config.DecoderParameters.MSIParams.CheckDigitScheme = Symbol.Barcode2.Design.CheckDigitSchemes.Default;
            m_laser.Config.DecoderParameters.MSIParams.Redundancy = true;
            m_laser.Config.DecoderParameters.MSIParams.ReportCheckDigit = false;
            m_laser.Config.DecoderParameters.UPCA = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.UPCAParams.Preamble = Symbol.Barcode2.Design.Preambles.Default;
            m_laser.Config.DecoderParameters.UPCAParams.ReportCheckDigit = true;
            m_laser.Config.DecoderParameters.UPCE0 = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.DecoderParameters.UPCE0Params.ConvertToUPCA = false;
            m_laser.Config.DecoderParameters.UPCE0Params.Preamble = Symbol.Barcode2.Design.Preambles.Default;
            m_laser.Config.DecoderParameters.UPCE0Params.ReportCheckDigit = false;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.AimDuration = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.AimMode = Symbol.Barcode2.Design.AIM_MODE.AIM_MODE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.AimType = Symbol.Barcode2.Design.AIM_TYPE.AIM_TYPE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.BeamTimer = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.DPMMode = Symbol.Barcode2.Design.DPM_MODE.DPM_MODE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.FocusMode = Symbol.Barcode2.Design.FOCUS_MODE.FOCUS_MODE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.FocusPosition = Symbol.Barcode2.Design.FOCUS_POSITION.FOCUS_POSITION_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.IlluminationMode = Symbol.Barcode2.Design.ILLUMINATION_MODE.ILLUMINATION_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.ImageCaptureTimeout = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.ImageCompressionTimeout = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.Inverse1DMode = Symbol.Barcode2.Design.INVERSE1D_MODE.INVERSE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.LinearSecurityLevel = Symbol.Barcode2.Design.LINEAR_SECURITY_LEVEL.SECURITY_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.PicklistMode = Symbol.Barcode2.Design.PICKLIST_MODE.PICKLIST_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.PointerTimer = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.PoorQuality1DMode = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.VFFeedback = Symbol.Barcode2.Design.VIEWFINDER_FEEDBACK.VIEWFINDER_FEEDBACK_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.VFFeedbackTime = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.VFMode = Symbol.Barcode2.Design.VIEWFINDER_MODE.VIEWFINDER_MODE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.VFPosition.Bottom = 0;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.VFPosition.Left = 0;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.VFPosition.Right = 0;
            m_laser.Config.ReaderParameters.ReaderSpecific.ImagerSpecific.VFPosition.Top = 0;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.AimDuration = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.AimMode = Symbol.Barcode2.Design.AIM_MODE.AIM_MODE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.AimType = Symbol.Barcode2.Design.AIM_TYPE.AIM_TYPE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.BeamTimer = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.BeamWidth = Symbol.Barcode2.Design.BEAM_WIDTH.DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.BidirRedundancy = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.ControlScanLed = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.DBPMode = Symbol.Barcode2.Design.DBP_MODE.DBP_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.KlasseEinsEnable = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.LinearSecurityLevel = Symbol.Barcode2.Design.LINEAR_SECURITY_LEVEL.SECURITY_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.PointerTimer = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.RasterHeight = -1;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.RasterMode = Symbol.Barcode2.Design.RASTER_MODE.RASTER_MODE_DEFAULT;
            m_laser.Config.ReaderParameters.ReaderSpecific.LaserSpecific.ScanLedLogicLevel = Symbol.Barcode2.Design.DisabledEnabled.Default;
            m_laser.Config.ScanDataSize = ((uint)(55u));
            m_laser.Config.ScanParameters.BeepFrequency = 2670;
            m_laser.Config.ScanParameters.BeepTime = 200;
            m_laser.Config.ScanParameters.CodeIdType = Symbol.Barcode2.Design.CodeIdTypes.Default;
            m_laser.Config.ScanParameters.LedTime = SCAN_TIMEOUT;
            m_laser.Config.ScanParameters.ScanType = Symbol.Barcode2.Design.SCANTYPES.Default;
            m_laser.Config.ScanParameters.WaveFile = "";
            m_laser.DeviceType = DEVICETYPES.FIRSTAVAILABLE;
            m_laser.EnableScanner = true;
            m_laser.OnScan += new Symbol.Barcode2.Design.Barcode2.OnScanEventHandler(LaserOnScan);
            m_laser.OnStatus += new Symbol.Barcode2.Design.Barcode2.OnStatusEventHandler(LaserOnStatus);
        }

        // Write tag
        public Response WriteTag(ITag tag)
        {
            Response response = Response.WriteError;
            return response;
        }

        // Clear tag storage
        public void Clear()
        {
        
        }

        // IDisposable
        public void Dispose()
        {
            if (m_laser != null) 
            { 
                m_laser.EnableScanner = false;
                m_laser.Dispose(); 
            }
        }

        // program led
        public void Led(LedColor color)
        {
        }

        // This gets called when module is disconnected
        private void LaserOnStatus(StatusData statusData)
        {
            if (statusData.State == States.WAITING && !m_connected)
            {
                m_connected = true;
            }
        }

        // Barcode scaned event
        private void LaserOnScan(ScanDataCollection scanDataCollection)
        {
            if (m_parent == null)
                return;
            
            // Checks if the BeginInvoke method is required because the OnScan delegate is called by a different thread
            if (m_parent.InvokeRequired)
            {
                // Executes the OnScan delegate asynchronously on the main thread
                m_parent.BeginInvoke(
                    new Symbol.Barcode2.Design.Barcode2.OnScanEventHandler(LaserOnScan),
                    new object[] { scanDataCollection } 
                    );
            }
            else
            {
                ScanData scanData = scanDataCollection.GetFirst;
                if (scanData.Result == Results.SUCCESS)
                {
                    ITag tag = new RLTag(scanData.Text);
                    m_processTagDelegate(tag);
                }
            }
        }
    }
}
