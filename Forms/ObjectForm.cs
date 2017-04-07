// File: ObjectForm.cs
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
    public partial class ObjectForm : Form
    {
        private MainForm m_parent;

        public ObjectForm(MainForm parent)
        {
            InitializeComponent();
            MainForm.ScaleWorkingArea(this);

            m_parent = parent;
            
            // set labels
            lbState.Text = Resources.lbStateObject;
            btCancel.Text = Resources.btCancelClose;
            btOK.Text = Resources.btOKWrite;
            lbInvNumberText.Text = Resources.lbInvNumberText;
            lbSerialText.Text = Resources.lbSerialText;
            lbNomenclaruteText.Text = Resources.lbNomenclaruteText;
            lbObjectIdText.Text = Resources.lbObjectIdText;

            ShowObjectParameters();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            switch (m_parent.Dispatcher.Device.WriteTag(new RLTag(
                TagType.Object, m_parent.Dispatcher.CompanyId,
                (ulong)m_parent.Dispatcher.CurrentObject["id"])))
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
                    //btOK.Visible = false;
                    break;
            }
        }

        private void ObjectForm_Resize(object sender, EventArgs e)
        {
            MainForm.ScaleButtons(this, btCancel, btOK, 0);
        }

        private void ObjectForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // remove object from found list
           /* if (e.KeyChar == '8')
            {
                DataRow row = m_parent.Dispatcher.DB["FoundObjects"].Select(
                    (ulong)m_parent.Dispatcher.CurrentObject["id"]);
                
                if (row != null)
                {
                    m_parent.Dispatcher.DB.DataSet.Tables["FoundObjects"].Rows.Remove(row);
                }
            }*/
        }

        private void ObjectForm_GotFocus(object sender, EventArgs e)
        {
            ShowObjectParameters();
        }

        private void ShowObjectParameters()
        {
            lbState.Text = Resources.lbStateObject;
            // Can write
            btOK.Visible = m_parent.Dispatcher.CanWrite;

            // Object parameters
            DataRow obj = m_parent.Dispatcher.CurrentObject;
            lbObjectId.Text = obj["id"].ToString();
            lbInvNumber.Text = obj["inventoryNumber"].ToString();
            lbSerial.Text = obj["serialNumber"].ToString();
            lbNomenclature.Text = obj["nomenclatureName"].ToString();
        }
    }
}