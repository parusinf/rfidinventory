// File: MainForm.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ru.nikitin.RFIDInventory.Properties;
using ru.nikitin.RFIDInventory.DataModel;

namespace ru.nikitin.RFIDInventory
{
    public partial class MainForm : Form
    {
        // scale controls constants
        public const int BUTTON_HEIGHT = 34;
        public const int BORDER = 4;

        // Main manager Dispatcher
        private Dispatcher m_dispatcher = null;

        // Page is changing now
        private bool m_pageChanging = false;

        // About view
        private bool m_aboutView = false;
        private string m_lbStateText;
        private string m_btCancelText;

        // Error handling
        private string m_errorMessage;

        // Forms
        Form m_objectForm = null;

        // Dispatcher
        public Dispatcher Dispatcher
        {
            get
            {
                return m_dispatcher;
            }
        }

        // Change page in process
        public bool PageChanging
        {
            set
            {
                m_pageChanging = value; 
            }
        }
        
        // Form constructor
        public MainForm()
        {
            InitializeComponent();
            ScaleWorkingArea(this);

            // set labels
            Text = Resources.lbProgramName;
            lbProgramName.Text = Resources.lbProgramName;
            lbInCompany.Text = Resources.lbInCompany;
            btOK.Text = Resources.btOKSelect;
            btCancel.Text = Resources.btCancelExit;
            lbState.Text = Resources.lbStateLoadData;
            lbCompany.Text = Resources.lbNoCompany;
            lbSerialText.Text = Resources.lbDeviceSerialText;
            lbLicenseText.Text = Resources.lbLicenseText;
            lbAuthorText.Text = Resources.lbAuthorText;
            lbAuthor.Text = Resources.lbAuthor;
            lbLicense.Text = Resources.lbDemo;
            lbProgramVersion.Text = Resources.lbVersion;

            // set control labels
            tpSheets.Text = Resources.tpSheets;
            tpLocations.Text = Resources.tpLocations;
            tpObjects.Text = Resources.tpObjects;
            chFindAll.Text = Resources.chFindAll;
            chFound.Text = Resources.chFound;
            chLocation.Text = Resources.chLocation;

            // grid properties
            dgcSheetsPerson.HeaderText = Resources.dgcSheetsPerson;
            dgcSheetsNumber.HeaderText = Resources.dgcSheetsNumber;
            dgcSheetsCount.HeaderText = Resources.dgcCount;
            dgcLocationsName.HeaderText = Resources.dgcLocationsName;
            dgcLocationsCount.HeaderText = Resources.dgcCount;
            dgcObjectsObject.HeaderText = Resources.dgcObjectsObject;

            // create application link on desktop
            if (!File.Exists("\\Windows\\Desktop\\" + Resources.lbProgramName + ".lnk"))
            {
                byte[] header = { 0xEF, 0xBB, 0xBF, 0x32, 0x37, 0x23, 0x22 };
                FileStream lab = File.Open("\\Windows\\Desktop\\" + 
                    Resources.lbProgramName + ".lnk", 
                    FileMode.CreateNew, FileAccess.Write, FileShare.None);
                lab.Write(header, 0, header.Length);
                lab.Write(System.Text.Encoding.ASCII.GetBytes(
                    GetType().Module.FullyQualifiedName), 
                    0, GetType().Module.FullyQualifiedName.Length);
                lab.WriteByte(0x22);
                lab.Close();
            }
        }

        // Open Database, check license on device connection event
        public void Startup()
        {
            // open database
            try
            {
                m_dispatcher.OpenDatabase();
            }
            catch (Exception ex)
            {
                m_errorMessage = ex.Message;
                FatalError(Resources.lbStateDatabaseError);
                return;
            }

            try
            {
                // show read company
                lbCompany.Text = m_dispatcher.CompanyName;

                // show device serial number
                lbSerial.Text = m_dispatcher.Device.Serial;

                // prepare user interface
                m_dispatcher.State = ApplicationState.ChooseSheet;
            }
            catch (Exception ex)
            {
                m_errorMessage = ex.Message;
                FatalError(ex.Message);
            }
        }

        /// <summary>
        /// Fatal error: show message, hide OK button
        /// </summary>
        /// <param name="message">Error message</param>
        public void FatalError(string message)
        {
            //throw new Exception(message);
            lbState.BackColor = Color.Red;
            lbState.ForeColor = Color.White;
            lbContextSearch.BackColor = Color.Red;
            lbState.Text = message;
            lbCompany.Text = m_errorMessage;
            tcMain.Visible = false;
            btOK.Visible = false;
            btCancel.Text = Resources.btCancelExit;
            btCancel.Focus();

            lbInCompany.Height = 140;
            lbInCompany.Text = m_errorMessage;
            lbCompany.Visible = false;
            lbSerialText.Visible = false;
            lbSerial.Visible = false;
            lbLicenseText.Visible = false;
            lbLicense.Visible = false;

            m_dispatcher.State = ApplicationState.Exit;
        }

        // After form creation
        private void timer1_Tick(object sender, EventArgs e)
        {
            m_timer1.Enabled = false;

            if (m_dispatcher == null)
            {
                // create dispatcher
                m_dispatcher = new Dispatcher(this);

                // select device
                try
                {
                    m_dispatcher.CreateDevice(Startup);
                }
                catch (Exception ex)
                {
                    m_errorMessage = ex.Message;
                    FatalError(Resources.lbStateDeviceError);
                }

                // timeout of context search is 4 seconds
                m_timer1.Interval = 4000;
            }
            else
            {
                lbContextSearch.Visible = false;
                lbContextSearch.Text = "";
            }
        }

        // Change application state after page changed
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_pageChanging)
                switch (tcMain.SelectedIndex)
                {
                    case 0:
                        m_dispatcher.State = ApplicationState.ChooseSheet;
                        break;
                    
                    case 1:
                        m_dispatcher.State = ApplicationState.ChooseLocation;
                        break;

                    case 2:
                        m_dispatcher.State = ApplicationState.ObjectsScaning;
                        break;
                }
        }

        // Process action
        private void DoAction()
        {
            switch (m_dispatcher.State)
            {
                case ApplicationState.ChooseSheet:
                    m_dispatcher.State = ApplicationState.ChooseLocation;
                    break;

                case ApplicationState.ChooseLocation:
                    m_dispatcher.State = ApplicationState.ObjectsScaning;
                    break;

                case ApplicationState.ObjectsScaning:
                    if (!m_dispatcher.Device.Scaning)
                    {
                        btOK.Text = Resources.btOKStop;
                        m_dispatcher.Device.Scaning = true;
                    }
                    else
                    {
                        btOK.Text = Resources.btOKSearch;
                        m_dispatcher.Device.Scaning = false;
                    }
                    break;

                case ApplicationState.Exit:
                    Close();
                    break;
            }

            SetFocus();
        }

        // Cancel action
        private void CancelAction()
        {
            if (m_aboutView)
                RestoreInterface();
            else
                switch (m_dispatcher.State)
                {
                    case ApplicationState.ChooseSheet:
                        m_dispatcher.State = ApplicationState.Exit;
                        break;

                    case ApplicationState.ChooseLocation:
                        m_pageChanging = true;
                        tcMain.SelectedIndex = 0;
                        m_pageChanging = false;
                        m_dispatcher.State = ApplicationState.ChooseSheet;
                        break;

                    case ApplicationState.ObjectsScaning:
                        m_pageChanging = true;
                        tcMain.SelectedIndex = 1;
                        m_pageChanging = false;
                        m_dispatcher.State = ApplicationState.ChooseLocation;
                        break;

                    case ApplicationState.Exit:
                        Close();
                        break;
                }
        }

        // OK button pressed
        private void buttonOK_Click(object sender, EventArgs e)
        {
            DoAction();
        }

        // Cancel button pressed
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CancelAction();
        }

        // Refresh locations after sheet changed
        private void chLocationsBySheet_CheckStateChanged(object sender, EventArgs e)
        {
            m_dispatcher.BindLocations();
            SetFocus();
        }

        // On Sheets page double click perform context action (switch to Locations)
        private void dgSheets_DoubleClick(object sender, EventArgs e)
        {
            DoAction();
        }

        // Process key down event
        private void KeyDownEvent(Keys key)
        {
            switch (key)
            {
                case Keys.EraseEof: // Scan
                    if (m_dispatcher.State == ApplicationState.ChooseLocation)
                    {
                        if (!m_dispatcher.Device.Scaning)
                        {
                            btOK.Text = Resources.btOKStop;
                            m_dispatcher.Device.Scaning = true;
                        }
                    }
                    else if (!m_dispatcher.Device.Scaning)
                        DoAction();
                    
                    break;
 
                case Keys.Enter:
                    DoAction();
                    break;

                case Keys.Escape: 
                    CancelAction();
                    break;
                
                case Keys.Back:
                    if (m_dispatcher.State == ApplicationState.ChooseLocation && 
                        lbContextSearch.Text != "")
                        lbContextSearch.Text = lbContextSearch.Text.Substring(0, 
                            lbContextSearch.Text.Length - 1);
                    break;
            }
        }

        // Process key up event
        private void KeyUpEvent(Keys key)
        {
            switch (key)
            {
                case Keys.EraseEof: // Scan
                    if (m_dispatcher.State == ApplicationState.ChooseLocation)
                    {
                        if (m_dispatcher.Device.Scaning)
                        {
                            btOK.Text = Resources.btOKEnter;
                            m_dispatcher.Device.Scaning = false;
                        }
                    }
                    else if (m_dispatcher.Device.Scaning)
                        DoAction();
                    
                    break;
            }
        }

        // All controls key down event
        private void dgSheets_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownEvent(e.KeyCode);
        }

        private void dgLocations_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownEvent(e.KeyCode);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownEvent(e.KeyCode);
        }

        private void tcMain_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownEvent(e.KeyCode);
        }

        private void chLocationsBySheet_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownEvent(e.KeyCode);
        }

        private void btOK_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownEvent(e.KeyCode);
        }

        private void dgObjects_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownEvent(e.KeyCode);
        }

        
        // All controls key up event
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpEvent(e.KeyCode);
        }

        private void tcMain_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpEvent(e.KeyCode);
        }

        private void btOK_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpEvent(e.KeyCode);
        }

        private void dgObjects_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpEvent(e.KeyCode);
        }

        private void dgLocations_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUpEvent(e.KeyCode);
        }
        
        // Refresh object list after location changed
        private void chFound_CheckStateChanged(object sender, EventArgs e)
        {
            m_dispatcher.BindObjects();
            SetFocus();
        }

        private void chLocation_CheckStateChanged(object sender, EventArgs e)
        {
            m_dispatcher.BindObjects();
            SetFocus();
        }

        
        // Restore focus
        private void chFindAll_CheckStateChanged(object sender, EventArgs e)
        {
            SetFocus();
        }

        
        // Search location by pressing digits
        private void dgLocations_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                if (lbContextSearch.Text == "")
                {
                    lbContextSearch.Visible = true;
                    m_timer1.Enabled = true;
                }

                lbContextSearch.Text += e.KeyChar;

                for (int i = 0; i < ((IList)dgLocations.DataSource).Count; i++)
                    if (dgLocations[i, 0].ToString().Length >= lbContextSearch.Text.Length &&
                        dgLocations[i, 0].ToString().Substring(0, 
                        lbContextSearch.Text.Length).Equals(lbContextSearch.Text))
                    {
                        dgLocations.CurrentRowIndex = i;
                        break;
                    }
            }
        }


        // Show inventory progress for selected sheet
        private void dgSheets_CurrentCellChanged(object sender, EventArgs e)
        {
            m_dispatcher.ShowInventoryProgress();
        }

        // Click on top table shows about dialog
        private void MainForm_Click(object sender, EventArgs e)
        {
            if (!m_aboutView)
                ShowAbout();
        }

        // Show about dialog
        private void ShowAbout()
        {
            if (m_dispatcher.State != ApplicationState.Exit)
            {
                m_lbStateText = lbState.Text;
                m_btCancelText = btCancel.Text;
                m_aboutView = true;
                tcMain.Visible = false;
                lbState.Text = Resources.lbStateAbout;
                
                lbSerial.Text = m_dispatcher.Device.Serial;
                if (lbSerial.Text == "")
                    lbSerial.Text = Resources.lbNoSerial;

                btOK.Visible = false;
                btCancel.Text = Resources.btCancelClose;
            }
        }

        // Restore interface after about dialog
        private void RestoreInterface()
        {
            m_aboutView = false;
            tcMain.Visible = true;
            lbState.Text = m_lbStateText;
            btOK.Visible = true;
            btCancel.Text = m_btCancelText;
            SetFocus();
        }

        // On Sheets page double click perform context action (switch to Objects)
        private void dgLocations_DoubleClick(object sender, EventArgs e)
        {
            DoAction();
        }

        // On Objects page double click shows object dialog
        private void dgObjects_DoubleClick(object sender, EventArgs e)
        {
            ShowObjectDialog();
        }

        // Show location dialog
        private void ShowLocationDialog()
        {
            if (m_dispatcher.CurrentLocation != null)
            {
                var locationForm = new Forms.LocationForm(this);
                locationForm.ShowDialog();
            }
        }

        // Show object dialog
        private void ShowObjectDialog()
        {
            if (m_dispatcher.CurrentObject != null)
            {
                if (m_objectForm == null)
                    m_objectForm = new Forms.ObjectForm(this);
                
                if (!m_objectForm.Visible)
                    m_objectForm.ShowDialog();
            }
        }

        // Set focus on data grid
        private void SetFocus()
        {
            switch (m_dispatcher.State)
            {
                case ApplicationState.ChooseSheet:
                    dgSheets.Focus();
                    break;

                case ApplicationState.ChooseLocation:
                    dgLocations.Focus();
                    break;

                case ApplicationState.ObjectsScaning:
                    dgObjects.Focus();
                    break;
            }
        }

        // On Locations page mouse down starts timer to determine long click
        private void dgLocations_MouseDown(object sender, MouseEventArgs e)
        {
            m_timer2.Enabled = true;
        }


        // Show Location dialog after half second
        private void m_timer2_Tick(object sender, EventArgs e)
        {
            m_timer2.Enabled = false;
            ShowLocationDialog();
        }

        // On Locations page mouse up disable timer
        private void dgLocations_MouseUp(object sender, MouseEventArgs e)
        {
            m_timer2.Enabled = false;
        }

        // On Objects page mouse down starts timer to determine long click
        private void dgObjects_MouseDown(object sender, MouseEventArgs e)
        {
            m_timer3.Enabled = true;
        }

        // Show Object dialog after half second
        private void m_timer3_Tick(object sender, EventArgs e)
        {
            m_timer3.Enabled = false;
            ShowObjectDialog();
        }

        // On Objects page mouse up disable timer
        private void dgObjects_MouseUp(object sender, MouseEventArgs e)
        {
            m_timer3.Enabled = false;
        }

        // Scale buttons
        public static void ScaleButtons(Form form, Button left, Button right, int offset)
        {
            left.Width = (form.Width - 3 * BORDER) / 2 - 1;
            left.Location = new Point(BORDER, form.ClientSize.Height - BORDER - BUTTON_HEIGHT - offset);
            right.Width = left.Width;
            right.Location = new Point(right.Width + 2 * BORDER,
                form.ClientSize.Height - BORDER - BUTTON_HEIGHT - offset);
        }

        // Full screen
        public static void ScaleWorkingArea(Form form)
        {
            form.Width = Screen.PrimaryScreen.WorkingArea.Width;
            form.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ScaleButtons(this, btCancel, btOK, 0);
            tcMain.Height = this.Height - BUTTON_HEIGHT - 38;
            tcMain.Width = this.Width;
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            if (m_dispatcher != null)
            {
                m_dispatcher.Dispose();
                m_dispatcher = null;
            }
        }
    }
}
