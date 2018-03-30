// File: LocationForm.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ru.nikitin.RFIDInventory.Properties;

namespace ru.nikitin.RFIDInventory.Forms
{
    public partial class LocationForm : Form
    {
        private MainForm m_parent;

        public LocationForm(MainForm parent)
        {
            InitializeComponent();
            MainForm.ScaleWorkingArea(this);
        
            m_parent = parent;

            DataRow obj = parent.Dispatcher.CurrentLocation;
            lbLocationId.Text = obj["id"].ToString();
            lbName.Text = obj["name"].ToString();
            
            // can write
            btOK.Visible = parent.Dispatcher.CanWrite;

            // set labels
            btCancel.Text = Resources.btCancelExit;
            btOK.Text = Resources.btOKWrite;
            lbState.Text = Resources.chLocation;
            lbLocationIdText.Text = Resources.lbLocationIdText;
            lbNameText.Text = Resources.lbNameText;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            switch (m_parent.Dispatcher.Device.WriteTag(new RLTag(
                TagType.Location, m_parent.Dispatcher.CompanyId,
                (ulong)m_parent.Dispatcher.CurrentLocation["id"])))
            {
                case Interfaces.Response.TagNotFound:
                    lbState.BackColor = Color.Red;
                    lbState.ForeColor = Color.White;
                    lbState.Text = Resources.lbStateTagNotFound;
                    break;

                case Interfaces.Response.WriteError:
                    lbState.BackColor = Color.Red;
                    lbState.ForeColor = Color.White;
                    lbState.Text = Resources.lbStateWriteError;
                    break;

                case Interfaces.Response.Successful:
                    lbState.BackColor = Color.Yellow;
                    lbState.ForeColor = Color.Black;
                    lbState.Text = Resources.lbStateWriteSuccessful;
                    btOK.Visible = false;
                    break;
            }
        }

        private void LocationForm_Resize(object sender, EventArgs e)
        {
            MainForm.ScaleButtons(this, btCancel, btOK, 0);
        }
    }
}