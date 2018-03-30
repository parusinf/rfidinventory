// File: Dispatcher.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using ru.nikitin.RFIDInventory.DataModel;
using ru.nikitin.RFIDInventory.Properties;
using ru.nikitin.RFIDInventory.Interfaces;

using NurApiDotNet;
using NordicId;

namespace ru.nikitin.RFIDInventory
{
    public enum ApplicationState : byte
    {
        Start,
        ChooseSheet,
        ChooseLocation,
        ObjectsScaning,
        Exit
    }

    /// <summary>
    /// Main manager
    /// </summary>
    public class Dispatcher : IDisposable
    {
        // Application state
        private ApplicationState m_state = ApplicationState.Start;

        // Device
        private IDevice m_device = null;

        // Main form
        private MainForm m_form = null;

        // Database
        private TextDataBase m_db = null;
        
        // Current sheet
        private ulong m_currentSheet = 0;

        // Application state
        public ApplicationState State
        {
            get
            {
                return m_state;
            }
            set
            {
                SetState(value);
            }
        }

        // Device
        public IDevice Device
        {
            get
            {
                return m_device;
            }
        }

        // Device
        public TextDataBase DB
        {
            get
            {
                return m_db;
            }
        }

        // Company name
        public string CompanyName
        {
            get
            {
                return (string)m_db.DataSet.Tables["Parameters"].Rows[0]["companyName"];
            }
        }

        // Company identificator
        public ulong CompanyId
        {
            get
            {
                return (ulong)m_db.DataSet.Tables["Parameters"].Rows[0]["companyId"];
            }
        }

        // Current object
        public DataRow CurrentObject
        {
            get
            {
                if (m_form.dgObjects.CurrentRowIndex >= 0)
                {
                    ulong id = (ulong)m_form.dgObjects[
                                m_form.dgObjects.CurrentRowIndex, 1];
                    return m_db["Objects"].Select(id);
                }
                else
                    return null;
            }
        }

        // Current location 
        public DataRow CurrentLocation
        {
            get
            {
                return m_db["Locations"].Select(
                    (ulong)m_form.dgLocations[m_form.dgLocations.CurrentRowIndex, 2]);
            }
        }

        // Dispatcher can write the tags
        public bool CanWrite
        {
            get 
            {
                return m_device.Writable && 
                    (bool)m_db.DataSet.Tables["Parameters"].Rows[0]["canWrite"];
            }
        }


        /// <summary>
        /// Create main manager
        /// </summary>
        /// <param name="form">For control MainForm</param>
        public Dispatcher(MainForm form)
        {
            m_form = form;
        }

        /// <summary>
        /// Perform tag found event
        /// </summary>
        /// <param name="tag">Tag value</param>
        public void ProcessTag(ITag tag)
        {
            // check company identificator
            if (tag.CompanyId == (ulong)m_db.DataSet.Tables["Parameters"]
                .Rows[0]["companyId"])
            {
                // object scanned
                if (m_state == ApplicationState.ObjectsScaning &&
                    tag.Type == TagType.Object)
                {
                    // not found before
                    if (m_db["ObjectsFound"].Select(tag.Id) == null)
                    {
                        // find object in data base
                        DataRow obj = m_db["Objects"].Select(tag.Id);

                        // if object exists in current sheet or find all objects is allowed
                        // then add the object in found list
                        if (obj != null && ((ulong)obj["sheetNumber"] == m_currentSheet ||
                            m_form.chFindAll.Checked))
                        {
                            ShowScanStatus(Resources.lbStateStateFound, BeepType.MB_OK, LedColor.Green);

                            // create found row
                            DataRow found = m_db.DataSet.Tables["ObjectsFound"].NewRow();
                            found["id"] = tag.Id;
                            found["sheetNumber"] = (ulong)obj["sheetNumber"];
                            found["dateTime"] = DateTime.Now;
                            found["locationId"] = UInt64.Parse(
                                m_form.dgLocations[
                                    m_form.dgLocations.CurrentRowIndex, 2].ToString());

                            // add the object in found list
                            m_db.DataSet.Tables["ObjectsFound"].Rows.Add(found);
                            m_db.DataSet.Tables["ObjectsFound"].AcceptChanges();

                            // refresh objects
                            BindObjects();
                            ShowInventoryProgress();

                            // add location by sheet
                            DataTable shLoc = m_db.DataSet.Tables["SheetLocations"];

                            if (shLoc.Select(String.Format(
                                "sheetNumber={0} AND locationId={1}",
                                found["sheetNumber"],
                                found["locationId"])).ToList().Count == 0)
                            {
                                shLoc.Rows.Add(new object[] 
                                { 
                                    found["sheetNumber"], 
                                    found["locationId"] 
                                });
                            }
                        }
                        // object not found 
                        else
                        {
                            ShowScanStatus(Resources.lbStateStateNotFound, BeepType.MB_ICONEXCLAMATION, LedColor.Red);
                        }
                    }
                    // object already found 
                    else
                    {
                        ShowScanStatus(Resources.lbStateStateAlreadyFound, BeepType.MB_ICONEXCLAMATION, LedColor.Orange);
                    }
                }

                // location scanned
                else if (m_state == ApplicationState.ChooseLocation &&
                    tag.Type == TagType.Location)
                {
                    // add location by sheet
                    DataTable shLoc = m_db.DataSet.Tables["SheetLocations"];

                    if (shLoc.Select(String.Format("sheetNumber={0} AND locationId={1}",
                        m_currentSheet, tag.Id)).ToList().Count == 0)
                    {
                        shLoc.Rows.Add(new object[] { m_currentSheet, tag.Id });
                        BindLocations();
                    }

                    // find location at data grid
                    int rowIndex = 0;
                    int count = ((IList)m_form.dgLocations.DataSource).Count;
                    for (; rowIndex < count; rowIndex++)
                        if ((ulong)m_form.dgLocations[rowIndex, 2] == tag.Id)
                            break;

                    // location found
                    if (rowIndex < count)
                    {
                        m_device.Scaning = false;
                        m_form.dgLocations.CurrentRowIndex = rowIndex;
                        State = ApplicationState.ObjectsScaning;
                    }
                }
            }
            
            // tag not valid or not current company
            else
            {
                ShowScanStatus(Resources.lbStateStateNotValid, BeepType.MB_ICONEXCLAMATION, LedColor.Red);
            }
        }

        /// <summary>
        /// Set ChooseSheet application state
        /// </summary>
        public void ChooseSheet()
        {
            // switch page
            m_form.PageChanging = true;
            m_form.tcMain.SelectedIndex = 0;
            m_form.PageChanging = false;

            // setup controls
            m_form.lbState.Text = Resources.lbStateChooseSheet;
            m_form.btOK.Text = Resources.btOKSelect;
            m_form.btCancel.Text = Resources.btCancelExit;
            m_form.dgSheets.Focus();
            ShowInventoryProgress();
        }

        /// <summary>
        /// Set ChooseLocation application state
        /// </summary>
        public void ChooseLocation()
        {
            // switch page
            m_form.PageChanging = true;
            m_form.tcMain.SelectedIndex = 1;
            m_form.PageChanging = false;

            // setup controls
            m_form.lbState.Text = Resources.lbStateChooseLocation;
            m_form.btOK.Text = Resources.btOKEnter;
            m_form.btCancel.Text = Resources.btCancelSheets;
            m_form.dgLocations.Focus();
        }

        /// <summary>
        /// Set ObjectsScanning application state
        /// </summary>
        public void ObjectsScanning()
        {
            // switch page
            m_form.PageChanging = true;
            m_form.tcMain.SelectedIndex = 2;
            m_form.PageChanging = false;

            // setup controls
            m_form.lbState.Text = Resources.lbStateObjectsScanning;
            m_form.btOK.Text = Resources.btOKSearch;
            m_form.btCancel.Text = Resources.btCancelLocations;

            if (m_form.dgLocations.CurrentRowIndex < 0)
                BindLocations();

            m_form.chLocation.Text = m_form.dgLocations[
                    m_form.dgLocations.CurrentRowIndex, 0].ToString();

            BindObjects();
            m_form.dgObjects.Focus();
        }

        /// <summary>
        /// Binding locations, depending on the CheckBox.
        /// </summary>
        public void BindLocations()
        {
            // locations by sheet
            if (m_form.chLocationsBySheet.Checked)
            {
                BindingSource bindingSource = new BindingSource();

                bindingSource.DataSource =
                    (
                        from shloc in m_db.DataSet.Tables["SheetLocations"].AsEnumerable()
                        from loc in m_db.DataSet.Tables["Locations"].AsEnumerable()
                        orderby (string)loc["name"]
                        where (ulong)shloc["sheetNumber"] == m_currentSheet
                           && (ulong)shloc["locationId"] == (ulong)loc["id"]
                        select new
                        {
                            id = (ulong)loc["id"],
                            name = (string)loc["name"],
                            counts = (string)loc["counts"]
                        }
                    ).ToList();

                m_form.dgtLocations.MappingName = bindingSource.GetListName(null);
                m_form.dgLocations.DataSource = bindingSource;
            }

            // all locations
            else
            {
                m_form.dgtLocations.MappingName = "Locations";
                m_form.dgLocations.DataSource = m_db.DataSet.Tables["Locations"].DefaultView;
            }

            m_form.chLocationsBySheet.Text = Resources.chLocationsBySheetPrefix + " " +
                m_form.dgSheets[m_form.dgSheets.CurrentRowIndex, 0].ToString();

            if (((IList)m_form.dgLocations.DataSource).Count > 5)
                m_form.dgcLocationsName.Width = 
                    m_form.Width - 15 - m_form.dgcLocationsCount.Width - 12;
            else
                m_form.dgcLocationsName.Width = 
                    m_form.Width - 15 - m_form.dgcLocationsCount.Width - 10;
        }

        /// <summary>
        /// Binding objects, depending on the CheckBoxs.
        /// </summary>
        public void BindObjects()
        {
            if (m_form.dgLocations.CurrentRowIndex < 0)
                BindLocations();

            if (m_form.chLocation.Checked)
            {
                m_db.DataSet.Tables["Objects"].DefaultView.RowFilter =
                    String.Format(
                    "sheetNumber={0} AND found={1} AND ISNULL(foundLocationId,locationId)={2}",
                    new object[] {                
                        (ulong)m_db.DataSet.Tables["Sheets"]
                            .Rows[m_form.dgSheets.CurrentRowIndex]["number"],
                        m_form.chFound.Checked ? 1 : 0,
                        m_form.dgLocations[m_form.dgLocations.CurrentRowIndex, 2].ToString()
                    });
            }
            else
            {
                m_db.DataSet.Tables["Objects"].DefaultView.RowFilter =
                    String.Format("sheetNumber={0} AND found={1}", new object[] {                
                        (ulong)m_db.DataSet.Tables["Sheets"]
                            .Rows[m_form.dgSheets.CurrentRowIndex]["number"],
                        m_form.chFound.Checked ? 1 : 0
                    });
            }

            if (m_form.chFound.Checked)
                m_db.DataSet.Tables["Objects"].DefaultView.Sort = "dateTime DESC";
            else
                m_db.DataSet.Tables["Objects"].DefaultView.Sort = "inventoryNumber";

            if (((IList)m_form.dgObjects.DataSource).Count > 3)
                m_form.dgcObjectsObject.Width = m_form.Width - 15 - 12;
            else
                m_form.dgcObjectsObject.Width = m_form.Width - 15;
        }

        /// <summary>
        /// Show current sheet inventory progress on progress bar.
        /// </summary>
        public void ShowInventoryProgress()
        {
            m_form.progressBar.Maximum =
                (int)m_db.DataSet.Tables["Sheets"]
                    .Rows[m_form.dgSheets.CurrentRowIndex]["count"];
            m_form.progressBar.Value =
                (int)m_db.DataSet.Tables["Sheets"]
                    .Rows[m_form.dgSheets.CurrentRowIndex]["foundCount"];
        }

        /// <summary>
        /// Change application state
        /// </summary>
        /// <param name="state">To state</param>
        private void SetState(ApplicationState state)
        {
            // state must be changed
            if (state == m_state)
                return;
            
            // stop stanning
            if (m_state == ApplicationState.ObjectsScaning && m_device.Scaning)
                m_device.Scaning = false;

            // reset context search string
            m_form.lbContextSearch.Text = "";

            // check password flag
            bool passed = true;

            // try change state
            try
            {
                switch (state)
                {
                    case ApplicationState.Start:
                        throw new ArgumentException("Forbidden start");

                    case ApplicationState.ChooseSheet:
                        if (m_state != ApplicationState.Exit)
                        {
                            ChooseSheet();
                        }
                        else
                            throw new ArgumentException("Forbidden sheet");

                        break;

                    case ApplicationState.ChooseLocation:
                        if (m_state == ApplicationState.ChooseSheet)
                        {
                            if (CheckPassword())
                            {
                                ChooseLocation();

                                if (m_currentSheet != (ulong)m_db.DataSet.Tables["Sheets"].
                                    Rows[m_form.dgSheets.CurrentRowIndex]["number"])
                                {
                                    m_currentSheet = (ulong)m_db.DataSet.Tables["Sheets"].
                                        Rows[m_form.dgSheets.CurrentRowIndex]["number"];
                                    BindLocations();
                                }
                            }
                            else
                                passed = false;
                        }
                        else if (m_state == ApplicationState.ObjectsScaning)
                            ChooseLocation();
                        else
                            throw new ArgumentException("Forbidden location");

                        break;

                    case ApplicationState.ObjectsScaning:
                        if (m_state == ApplicationState.ChooseSheet)
                        {
                            if (CheckPassword())
                                ObjectsScanning();
                            else
                                passed = false;
                        }
                        else if (m_state == ApplicationState.ChooseLocation)
                            ObjectsScanning();
                        else
                            throw new ArgumentException("Forbidden objects");

                        break;

                    case ApplicationState.Exit:
                        if (m_state == ApplicationState.ChooseSheet)
                            m_form.Close();

                        break;
                }

                if (passed)
                    m_state = state;
            }
            catch (Exception e)
            {
                m_form.FatalError(e.Message);
            }
        }

        /// <summary>
        /// Select device
        /// </summary>
        public void CreateDevice(DeviceConnectedDelegate startupDelegate)
        {
            // choose device
            try
            {
                m_device = new NordicIdDevice(this.m_form, startupDelegate, ProcessTag);
            }
            catch (Exception)
            {
                m_device = new MotorolaDevice(this.m_form, ProcessTag);
                startupDelegate();
            }         
        }

        /// <summary>
        /// Open Database, load data, create additional tables and calculated fields, binding
        /// </summary>
        public void OpenDatabase()
        {
            // create database
            m_db = new TextDataBase(Tools.Path + "db\\");

            // load data with progress bar
            int totalRecords = 0;
            foreach (DataTable table in m_db.DataSet.Tables)
                totalRecords += m_db[table.TableName].Count;

            m_form.progressBar.Value = 0;
            m_form.progressBar.Maximum = totalRecords;

            // reverce wave animation
            using (NordicKeyLightWave imagingScan = new NordicKeyLightWave())
            {
                imagingScan.Reverse = true;

                foreach (DataTable table in m_db.DataSet.Tables)
                    totalRecords += m_db[table.TableName].Fill(m_form.progressBar, imagingScan);

                // initialization message
                m_form.lbState.Text = Resources.lbStateInit;
                //m_form.Refresh();

                // Sheets(numberDate, count, foundCount, counts)
                // Locations(count, foundCount, counts)
                CreateCalculatedFields();

                // group objects by sheets and locations for fast filter
                CreateSheetLocations();

                // prepare DataGrids source
                m_form.dgSheets.DataSource = m_db.DataSet.Tables["Sheets"].DefaultView;
                m_db.DataSet.Tables["Locations"].DefaultView.Sort = "name";
                m_form.dgObjects.DataSource = m_db.DataSet.Tables["Objects"].DefaultView;

                if (((IList)m_form.dgSheets.DataSource).Count > 4)
                    m_form.dgcSheetsPerson.Width =
                        m_form.Width - 15 -
                        m_form.dgcSheetsNumber.Width - m_form.dgcSheetsCount.Width - 12;
                else
                    m_form.dgcSheetsPerson.Width =
                        m_form.Width - 15 -
                        m_form.dgcSheetsNumber.Width - m_form.dgcSheetsCount.Width;

                if (((IList)m_form.dgSheets.DataSource).Count > 1)
                    m_form.chFindAll.Enabled = true;
                else
                    m_form.chFindAll.Enabled = false;
            }
        }

        /// <summary>
        /// Create calculated fields
        /// Sheets(numberDate, count, foundCount, counts)
        /// Locations(count, foundCount, counts)
        /// </summary>
        private void CreateCalculatedFields()
        {
            // Sheets
            m_db.DataSet.Tables["Sheets"].Columns.Add(new DataColumn("numberDate", 
                typeof(string), 
                "CONVERT(number,'System.String')+'\n'+date"));

            m_db.DataSet.Tables["Sheets"].Columns.Add(new DataColumn("count", 
                typeof(Int32), 
                "COUNT(CHILD(ObjectsSheetsFK).sheetNumber)"));

            m_db.DataSet.Tables["Sheets"].Columns.Add(new DataColumn("foundCount", 
                typeof(Int32), 
                "COUNT(CHILD(ObjectsFoundSheetsFK).sheetNumber)"));

            m_db.DataSet.Tables["Sheets"].Columns.Add(new DataColumn("counts", 
                typeof(string),
                "CONVERT(count,'System.String')+'\n'+CONVERT(foundCount,'System.String')"));

            m_db.DataSet.Tables["Sheets"].Columns.Add(
                new DataColumn("passwordChecked", typeof(bool)));
            
            foreach (var rec in m_db.DataSet.Tables["Sheets"].Select())
                rec["passwordChecked"] = (string)rec["password"] == "";

            // Locations
            m_db.DataSet.Tables["Locations"].Columns.Add(new DataColumn("count", 
                typeof(Int32), 
                "COUNT(CHILD(ObjectsLocationsFK).locationId)"));

            m_db.DataSet.Tables["Locations"].Columns.Add(new DataColumn("foundCount", 
                typeof(Int32), 
                "COUNT(CHILD(ObjectsFoundLocationFK).locationId)"));

            m_db.DataSet.Tables["Locations"].Columns.Add(new DataColumn("counts", 
                typeof(string),
                "CONVERT(count,'System.String')+'\n'+CONVERT(foundCount,'System.String')"));

            // Objects
            m_db.DataSet.Tables["Objects"].Columns.Add(new DataColumn("found", 
                typeof(Int32), 
                "COUNT(CHILD(ObjectsFoundFK).id)"));

            m_db.DataSet.Tables["Objects"].Columns.Add(new DataColumn("dateTime", 
                typeof(DateTime), 
                "MAX(CHILD(ObjectsFoundFK).dateTime)"));

            m_db.DataSet.Tables["Objects"].Columns.Add(new DataColumn("foundLocationId", 
                typeof(ulong), 
                "MAX(CHILD(ObjectsFoundFK).locationId)"));

            m_db.DataSet.Tables["Objects"].Columns.Add(new DataColumn("object", 
                typeof(string),
                "inventoryNumber+'\n'+serialNumber+'\n'+nomenclatureName"));
        }

        /// <summary>
        /// Group objects by sheets and locations for fast filter.
        /// </summary>
        private void CreateSheetLocations()
        {
            DataTable sheetLocations = new DataTable("SheetLocations");
            sheetLocations.Columns.AddRange(new DataColumn[]
                {
                    new DataColumn("sheetNumber", typeof(ulong)),
                    new DataColumn("locationId", typeof(ulong))
                });
            sheetLocations.PrimaryKey = new DataColumn[] 
                { 
                    sheetLocations.Columns[0], 
                    sheetLocations.Columns[1] 
                };
            m_db.DataSet.Tables.Add(sheetLocations);

            // locations from Objects
            var query =
                (
                    from obj in m_db.DataSet.Tables["Objects"].AsEnumerable()
                    group obj by
                    new
                    {
                        sheetNumber = (ulong)obj["sheetNumber"],
                        locationId = (ulong)obj["locationId"]
                    }
                        into grp
                        select new 
                        { 
                            grp.Key.sheetNumber, 
                            grp.Key.locationId 
                        }
                );

            foreach (var rec in query)
                sheetLocations.Rows.Add(new object[] { rec.sheetNumber, rec.locationId });

            // locations from ObjectsFound
            query =
                (
                    from obj in m_db.DataSet.Tables["ObjectsFound"].AsEnumerable()
                    group obj by
                    new
                    {
                        sheetNumber = (ulong)obj["sheetNumber"],
                        locationId = (ulong)obj["locationId"]
                    }
                        into grp
                        select new
                        {
                            grp.Key.sheetNumber,
                            grp.Key.locationId
                        }
                );

            foreach (var rec in query)
                if (sheetLocations.Select(String.Format("sheetNumber={0} AND locationId={1}", 
                    rec.sheetNumber, rec.locationId)).ToList().Count == 0)
                sheetLocations.Rows.Add(new object[] { rec.sheetNumber, rec.locationId });
        }

        /// <summary>
        /// Check password if present
        /// </summary>
        private bool CheckPassword()
        {
            if (!(bool)m_db.DataSet.Tables["Sheets"]
                .Rows[m_form.dgSheets.CurrentRowIndex]["passwordChecked"])
            {
                var passwordForm = new PasswordForm(
                    (string)m_db.DataSet.Tables["Sheets"]
                        .Rows[m_form.dgSheets.CurrentRowIndex]["personCode"],
                    (string)m_db.DataSet.Tables["Sheets"]
                        .Rows[m_form.dgSheets.CurrentRowIndex]["password"]);

                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    m_db.DataSet.Tables["Sheets"]
                        .Rows[m_form.dgSheets.CurrentRowIndex]["passwordChecked"] = true;
                    return true;
                }
                else
                {
                    m_form.tcMain.SelectedIndex = 0;
                    m_form.dgSheets.Focus();
                    return false;
                }
            }
            else
                return true;
        }

        // Show scan status
        private void ShowScanStatus(string message, BeepType beep, LedColor color)
        {
            m_form.lbState.Text = message;
            Tools.MessageBeep(beep);
            m_device.Led(color);
        }

        /// <summary>
        /// IDisposable method
        /// </summary>
        public void Dispose()
        {
            m_db.Dispose();
            m_device.Dispose();
        }
    }
}
