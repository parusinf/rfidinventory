// File: MainForm.Designer.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

namespace ru.nikitin.RFIDInventory
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.m_timer1 = new System.Windows.Forms.Timer();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpSheets = new System.Windows.Forms.TabPage();
            this.chFindAll = new System.Windows.Forms.CheckBox();
            this.dgSheets = new System.Windows.Forms.DataGrid();
            this.dgtSheets = new System.Windows.Forms.DataGridTableStyle();
            this.dgcSheetsPerson = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcSheetsNumber = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcSheetsCount = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tpLocations = new System.Windows.Forms.TabPage();
            this.chLocationsBySheet = new System.Windows.Forms.CheckBox();
            this.dgLocations = new System.Windows.Forms.DataGrid();
            this.dgtLocations = new System.Windows.Forms.DataGridTableStyle();
            this.dgcLocationsName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcLocationsCount = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcLocationsId = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tpObjects = new System.Windows.Forms.TabPage();
            this.chLocation = new System.Windows.Forms.CheckBox();
            this.chFound = new System.Windows.Forms.CheckBox();
            this.dgObjects = new System.Windows.Forms.DataGrid();
            this.dgtObjects = new System.Windows.Forms.DataGridTableStyle();
            this.dgcObjectsObject = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcObjectsId = new System.Windows.Forms.DataGridTextBoxColumn();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lbState = new System.Windows.Forms.Label();
            this.lbContextSearch = new System.Windows.Forms.Label();
            this.lbProgramName = new System.Windows.Forms.Label();
            this.lbInCompany = new System.Windows.Forms.Label();
            this.lbCompany = new System.Windows.Forms.Label();
            this.lbSerialText = new System.Windows.Forms.Label();
            this.lbSerial = new System.Windows.Forms.Label();
            this.lbLicenseText = new System.Windows.Forms.Label();
            this.lbAuthorText = new System.Windows.Forms.Label();
            this.lbAuthor = new System.Windows.Forms.Label();
            this.lbLicense = new System.Windows.Forms.Label();
            this.m_timer2 = new System.Windows.Forms.Timer();
            this.m_timer3 = new System.Windows.Forms.Timer();
            this.lbProgramVersion = new System.Windows.Forms.Label();
            this.tcMain.SuspendLayout();
            this.tpSheets.SuspendLayout();
            this.tpLocations.SuspendLayout();
            this.tpObjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_timer1
            // 
            this.m_timer1.Enabled = true;
            this.m_timer1.Interval = 1;
            this.m_timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tcMain
            // 
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tpSheets);
            this.tcMain.Controls.Add(this.tpLocations);
            this.tcMain.Controls.Add(this.tpObjects);
            this.tcMain.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.tcMain.Location = new System.Drawing.Point(0, 28);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(240, 250);
            this.tcMain.TabIndex = 0;
            this.tcMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tcMain_KeyUp);
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tcMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tcMain_KeyDown);
            // 
            // tpSheets
            // 
            this.tpSheets.Controls.Add(this.chFindAll);
            this.tpSheets.Controls.Add(this.dgSheets);
            this.tpSheets.Location = new System.Drawing.Point(4, 28);
            this.tpSheets.Name = "tpSheets";
            this.tpSheets.Size = new System.Drawing.Size(232, 218);
            this.tpSheets.Text = "Sheets";
            // 
            // chFindAll
            // 
            this.chFindAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chFindAll.Checked = true;
            this.chFindAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chFindAll.Enabled = false;
            this.chFindAll.Location = new System.Drawing.Point(2, 196);
            this.chFindAll.Name = "chFindAll";
            this.chFindAll.Size = new System.Drawing.Size(227, 20);
            this.chFindAll.TabIndex = 1;
            this.chFindAll.Text = "Search in all sheets";
            this.chFindAll.CheckStateChanged += new System.EventHandler(this.chFindAll_CheckStateChanged);
            // 
            // dgSheets
            // 
            this.dgSheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgSheets.BackColor = System.Drawing.Color.White;
            this.dgSheets.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgSheets.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.dgSheets.GridLineColor = System.Drawing.SystemColors.ControlDark;
            this.dgSheets.Location = new System.Drawing.Point(0, 0);
            this.dgSheets.Name = "dgSheets";
            this.dgSheets.PreferredRowHeight = 33;
            this.dgSheets.RowHeadersVisible = false;
            this.dgSheets.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            this.dgSheets.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dgSheets.Size = new System.Drawing.Size(232, 192);
            this.dgSheets.TabIndex = 0;
            this.dgSheets.TableStyles.Add(this.dgtSheets);
            this.dgSheets.DoubleClick += new System.EventHandler(this.dgSheets_DoubleClick);
            this.dgSheets.CurrentCellChanged += new System.EventHandler(this.dgSheets_CurrentCellChanged);
            this.dgSheets.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgSheets_KeyDown);
            // 
            // dgtSheets
            // 
            this.dgtSheets.GridColumnStyles.Add(this.dgcSheetsPerson);
            this.dgtSheets.GridColumnStyles.Add(this.dgcSheetsNumber);
            this.dgtSheets.GridColumnStyles.Add(this.dgcSheetsCount);
            this.dgtSheets.MappingName = "Sheets";
            // 
            // dgcSheetsPerson
            // 
            this.dgcSheetsPerson.Format = "";
            this.dgcSheetsPerson.FormatInfo = null;
            this.dgcSheetsPerson.HeaderText = "Person";
            this.dgcSheetsPerson.MappingName = "personCode";
            this.dgcSheetsPerson.Width = 94;
            // 
            // dgcSheetsNumber
            // 
            this.dgcSheetsNumber.Format = "";
            this.dgcSheetsNumber.FormatInfo = null;
            this.dgcSheetsNumber.HeaderText = "#, Date";
            this.dgcSheetsNumber.MappingName = "numberDate";
            this.dgcSheetsNumber.Width = 68;
            // 
            // dgcSheetsCount
            // 
            this.dgcSheetsCount.Format = "";
            this.dgcSheetsCount.FormatInfo = null;
            this.dgcSheetsCount.HeaderText = "Count";
            this.dgcSheetsCount.MappingName = "counts";
            this.dgcSheetsCount.Width = 62;
            // 
            // tpLocations
            // 
            this.tpLocations.Controls.Add(this.chLocationsBySheet);
            this.tpLocations.Controls.Add(this.dgLocations);
            this.tpLocations.Location = new System.Drawing.Point(4, 28);
            this.tpLocations.Name = "tpLocations";
            this.tpLocations.Size = new System.Drawing.Size(232, 218);
            this.tpLocations.Text = "Locations";
            // 
            // chLocationsBySheet
            // 
            this.chLocationsBySheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chLocationsBySheet.Location = new System.Drawing.Point(2, 196);
            this.chLocationsBySheet.Name = "chLocationsBySheet";
            this.chLocationsBySheet.Size = new System.Drawing.Size(229, 20);
            this.chLocationsBySheet.TabIndex = 1;
            this.chLocationsBySheet.Text = "Locations of selected sheet";
            this.chLocationsBySheet.CheckStateChanged += new System.EventHandler(this.chLocationsBySheet_CheckStateChanged);
            this.chLocationsBySheet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chLocationsBySheet_KeyDown);
            // 
            // dgLocations
            // 
            this.dgLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgLocations.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgLocations.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.dgLocations.GridLineColor = System.Drawing.SystemColors.ControlDark;
            this.dgLocations.Location = new System.Drawing.Point(0, 0);
            this.dgLocations.Name = "dgLocations";
            this.dgLocations.PreferredRowHeight = 33;
            this.dgLocations.RowHeadersVisible = false;
            this.dgLocations.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            this.dgLocations.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dgLocations.Size = new System.Drawing.Size(232, 192);
            this.dgLocations.TabIndex = 0;
            this.dgLocations.TableStyles.Add(this.dgtLocations);
            this.dgLocations.DoubleClick += new System.EventHandler(this.dgLocations_DoubleClick);
            this.dgLocations.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgLocations_MouseUp);
            this.dgLocations.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgLocations_MouseDown);
            this.dgLocations.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgLocations_KeyPress);
            this.dgLocations.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgLocations_KeyUp);
            this.dgLocations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgLocations_KeyDown);
            // 
            // dgtLocations
            // 
            this.dgtLocations.GridColumnStyles.Add(this.dgcLocationsName);
            this.dgtLocations.GridColumnStyles.Add(this.dgcLocationsCount);
            this.dgtLocations.GridColumnStyles.Add(this.dgcLocationsId);
            this.dgtLocations.MappingName = "Locations";
            // 
            // dgcLocationsName
            // 
            this.dgcLocationsName.Format = "";
            this.dgcLocationsName.FormatInfo = null;
            this.dgcLocationsName.HeaderText = "Name";
            this.dgcLocationsName.MappingName = "name";
            this.dgcLocationsName.Width = 164;
            // 
            // dgcLocationsCount
            // 
            this.dgcLocationsCount.Format = "";
            this.dgcLocationsCount.FormatInfo = null;
            this.dgcLocationsCount.HeaderText = "Count";
            this.dgcLocationsCount.MappingName = "counts";
            this.dgcLocationsCount.Width = 62;
            // 
            // dgcLocationsId
            // 
            this.dgcLocationsId.Format = "";
            this.dgcLocationsId.FormatInfo = null;
            this.dgcLocationsId.MappingName = "id";
            this.dgcLocationsId.Width = 0;
            // 
            // tpObjects
            // 
            this.tpObjects.Controls.Add(this.chLocation);
            this.tpObjects.Controls.Add(this.chFound);
            this.tpObjects.Controls.Add(this.dgObjects);
            this.tpObjects.Location = new System.Drawing.Point(4, 28);
            this.tpObjects.Name = "tpObjects";
            this.tpObjects.Size = new System.Drawing.Size(232, 218);
            this.tpObjects.Text = "Objects";
            // 
            // chLocation
            // 
            this.chLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chLocation.Checked = true;
            this.chLocation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chLocation.Location = new System.Drawing.Point(118, 196);
            this.chLocation.Name = "chLocation";
            this.chLocation.Size = new System.Drawing.Size(114, 20);
            this.chLocation.TabIndex = 2;
            this.chLocation.Text = "Location";
            this.chLocation.CheckStateChanged += new System.EventHandler(this.chLocation_CheckStateChanged);
            // 
            // chFound
            // 
            this.chFound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chFound.Checked = true;
            this.chFound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chFound.Location = new System.Drawing.Point(2, 196);
            this.chFound.Name = "chFound";
            this.chFound.Size = new System.Drawing.Size(82, 20);
            this.chFound.TabIndex = 1;
            this.chFound.Text = "Found";
            this.chFound.CheckStateChanged += new System.EventHandler(this.chFound_CheckStateChanged);
            // 
            // dgObjects
            // 
            this.dgObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgObjects.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgObjects.ColumnHeadersVisible = false;
            this.dgObjects.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular);
            this.dgObjects.GridLineColor = System.Drawing.SystemColors.ControlDark;
            this.dgObjects.Location = new System.Drawing.Point(0, 0);
            this.dgObjects.Name = "dgObjects";
            this.dgObjects.PreferredRowHeight = 62;
            this.dgObjects.RowHeadersVisible = false;
            this.dgObjects.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            this.dgObjects.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dgObjects.Size = new System.Drawing.Size(232, 192);
            this.dgObjects.TabIndex = 0;
            this.dgObjects.TableStyles.Add(this.dgtObjects);
            this.dgObjects.DoubleClick += new System.EventHandler(this.dgObjects_DoubleClick);
            this.dgObjects.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgObjects_MouseUp);
            this.dgObjects.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgObjects_MouseDown);
            this.dgObjects.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgObjects_KeyUp);
            this.dgObjects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgObjects_KeyDown);
            // 
            // dgtObjects
            // 
            this.dgtObjects.GridColumnStyles.Add(this.dgcObjectsObject);
            this.dgtObjects.GridColumnStyles.Add(this.dgcObjectsId);
            this.dgtObjects.MappingName = "Objects";
            // 
            // dgcObjectsObject
            // 
            this.dgcObjectsObject.Format = "";
            this.dgcObjectsObject.FormatInfo = null;
            this.dgcObjectsObject.HeaderText = "Inv#, Ser#, Nomenclature";
            this.dgcObjectsObject.MappingName = "object";
            this.dgcObjectsObject.Width = 225;
            // 
            // dgcObjectsId
            // 
            this.dgcObjectsId.Format = "";
            this.dgcObjectsId.FormatInfo = null;
            this.dgcObjectsId.MappingName = "id";
            this.dgcObjectsId.Width = 0;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.BackColor = System.Drawing.Color.DarkOrange;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btOK.Location = new System.Drawing.Point(122, 282);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(114, 34);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "Select >";
            this.btOK.Click += new System.EventHandler(this.buttonOK_Click);
            this.btOK.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btOK_KeyUp);
            this.btOK.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btOK_KeyDown);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.BackColor = System.Drawing.Color.OrangeRed;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btCancel.Location = new System.Drawing.Point(4, 282);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(114, 34);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "< Exit";
            this.btCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 24);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(240, 6);
            // 
            // lbState
            // 
            this.lbState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbState.BackColor = System.Drawing.Color.Yellow;
            this.lbState.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lbState.Location = new System.Drawing.Point(2, 2);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(236, 20);
            this.lbState.Text = "Data loading...";
            this.lbState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbContextSearch
            // 
            this.lbContextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbContextSearch.BackColor = System.Drawing.Color.Yellow;
            this.lbContextSearch.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lbContextSearch.Location = new System.Drawing.Point(196, 2);
            this.lbContextSearch.Name = "lbContextSearch";
            this.lbContextSearch.Size = new System.Drawing.Size(40, 20);
            this.lbContextSearch.Visible = false;
            // 
            // lbProgramName
            // 
            this.lbProgramName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgramName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lbProgramName.Location = new System.Drawing.Point(4, 36);
            this.lbProgramName.Name = "lbProgramName";
            this.lbProgramName.Size = new System.Drawing.Size(233, 23);
            this.lbProgramName.Text = "RFIDInventory";
            this.lbProgramName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbInCompany
            // 
            this.lbInCompany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInCompany.Location = new System.Drawing.Point(4, 83);
            this.lbInCompany.Name = "lbInCompany";
            this.lbInCompany.Size = new System.Drawing.Size(229, 15);
            this.lbInCompany.Text = "Company:";
            // 
            // lbCompany
            // 
            this.lbCompany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCompany.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.lbCompany.ForeColor = System.Drawing.Color.Blue;
            this.lbCompany.Location = new System.Drawing.Point(4, 100);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(229, 18);
            this.lbCompany.Text = "No company";
            // 
            // lbSerialText
            // 
            this.lbSerialText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSerialText.Location = new System.Drawing.Point(4, 129);
            this.lbSerialText.Name = "lbSerialText";
            this.lbSerialText.Size = new System.Drawing.Size(229, 20);
            this.lbSerialText.Text = "Device serial number:";
            // 
            // lbSerial
            // 
            this.lbSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSerial.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular);
            this.lbSerial.ForeColor = System.Drawing.Color.Blue;
            this.lbSerial.Location = new System.Drawing.Point(4, 146);
            this.lbSerial.Name = "lbSerial";
            this.lbSerial.Size = new System.Drawing.Size(229, 20);
            this.lbSerial.Text = "SERIAL";
            // 
            // lbLicenseText
            // 
            this.lbLicenseText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLicenseText.Location = new System.Drawing.Point(6, 177);
            this.lbLicenseText.Name = "lbLicenseText";
            this.lbLicenseText.Size = new System.Drawing.Size(229, 20);
            this.lbLicenseText.Text = "License:";
            // 
            // lbAuthorText
            // 
            this.lbAuthorText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAuthorText.Location = new System.Drawing.Point(4, 222);
            this.lbAuthorText.Name = "lbAuthorText";
            this.lbAuthorText.Size = new System.Drawing.Size(100, 20);
            this.lbAuthorText.Text = "Developer:";
            // 
            // lbAuthor
            // 
            this.lbAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAuthor.ForeColor = System.Drawing.Color.Blue;
            this.lbAuthor.Location = new System.Drawing.Point(6, 239);
            this.lbAuthor.Name = "lbAuthor";
            this.lbAuthor.Size = new System.Drawing.Size(229, 17);
            this.lbAuthor.Text = "Pavel Nikitin © 2013";
            // 
            // lbLicense
            // 
            this.lbLicense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLicense.ForeColor = System.Drawing.Color.Blue;
            this.lbLicense.Location = new System.Drawing.Point(4, 194);
            this.lbLicense.Name = "lbLicense";
            this.lbLicense.Size = new System.Drawing.Size(229, 20);
            this.lbLicense.Text = "Demo";
            // 
            // m_timer2
            // 
            this.m_timer2.Interval = 500;
            this.m_timer2.Tick += new System.EventHandler(this.m_timer2_Tick);
            // 
            // m_timer3
            // 
            this.m_timer3.Interval = 500;
            this.m_timer3.Tick += new System.EventHandler(this.m_timer3_Tick);
            // 
            // lbProgramVersion
            // 
            this.lbProgramVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgramVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbProgramVersion.Location = new System.Drawing.Point(4, 59);
            this.lbProgramVersion.Name = "lbProgramVersion";
            this.lbProgramVersion.Size = new System.Drawing.Size(233, 23);
            this.lbProgramVersion.Text = "Version 1.1";
            this.lbProgramVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbContextSearch);
            this.Controls.Add(this.lbState);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.lbProgramName);
            this.Controls.Add(this.lbCompany);
            this.Controls.Add(this.lbInCompany);
            this.Controls.Add(this.lbSerial);
            this.Controls.Add(this.lbSerialText);
            this.Controls.Add(this.lbLicense);
            this.Controls.Add(this.lbLicenseText);
            this.Controls.Add(this.lbAuthorText);
            this.Controls.Add(this.lbAuthor);
            this.Controls.Add(this.lbProgramVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFIDInventory";
            this.TopMost = true;
            this.Closed += new System.EventHandler(this.MainForm_Closed);
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.tcMain.ResumeLayout(false);
            this.tpSheets.ResumeLayout(false);
            this.tpLocations.ResumeLayout(false);
            this.tpObjects.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer m_timer1;
        System.Windows.Forms.TabPage tpLocations;
        System.Windows.Forms.TabPage tpObjects;
        public System.Windows.Forms.DataGrid dgSheets;
        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.DataGrid dgLocations;
        public System.Windows.Forms.TabPage tpSheets;
        public System.Windows.Forms.CheckBox chLocationsBySheet;
        public System.Windows.Forms.Label lbState;
        public System.Windows.Forms.Button btOK;
        public System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.DataGrid dgObjects;
        System.Windows.Forms.DataGridTableStyle dgtObjects;
        System.Windows.Forms.DataGridTableStyle dgtSheets;
        public System.Windows.Forms.DataGridTableStyle dgtLocations;
        public System.Windows.Forms.CheckBox chFound;
        public System.Windows.Forms.CheckBox chLocation;
        public System.Windows.Forms.DataGridTextBoxColumn dgcLocationsCount;
        System.Windows.Forms.DataGridTextBoxColumn dgcLocationsId;
        public System.Windows.Forms.DataGridTextBoxColumn dgcSheetsCount;
        public System.Windows.Forms.CheckBox chFindAll;
        public System.Windows.Forms.Label lbContextSearch;
        public System.Windows.Forms.TabControl tcMain;
        public System.Windows.Forms.DataGridTextBoxColumn dgcObjectsObject;
        public System.Windows.Forms.Label lbSerial;
        private System.Windows.Forms.Label lbProgramName;
        private System.Windows.Forms.Label lbInCompany;
        private System.Windows.Forms.Label lbCompany;
        private System.Windows.Forms.Label lbSerialText;
        private System.Windows.Forms.Label lbLicenseText;
        private System.Windows.Forms.Label lbAuthorText;
        private System.Windows.Forms.Label lbAuthor;
        private System.Windows.Forms.Label lbLicense;
        private System.Windows.Forms.DataGridTextBoxColumn dgcObjectsId;
        private System.Windows.Forms.Timer m_timer2;
        private System.Windows.Forms.Timer m_timer3;
        private System.Windows.Forms.Label lbProgramVersion;
        public System.Windows.Forms.DataGridTextBoxColumn dgcLocationsName;
        public System.Windows.Forms.DataGridTextBoxColumn dgcSheetsPerson;
        public System.Windows.Forms.DataGridTextBoxColumn dgcSheetsNumber;
    }
}

