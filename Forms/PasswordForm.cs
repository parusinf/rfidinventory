// File: PasswordForm.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Windows.Forms;

using ru.nikitin.RFIDInventory.Properties;

namespace ru.nikitin.RFIDInventory
{
    public partial class PasswordForm : Form
    {
        string m_password;

        public PasswordForm(string person, string password)
        {
            InitializeComponent();
            MainForm.ScaleWorkingArea(this);
            lbPasswordCaption.Text = Resources.lbPasswordCaption + person;
            m_password = password;
            lbWrongPassword.Visible = false;
            tbPassword.Focus();

            lbPasswordCaption.Text = Resources.lbPasswordCaption;
            btCancel.Text = Resources.btCancel;
            lbWrongPassword.Text = Resources.lbWrongPassword;
        }

        void btOK_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text != m_password)
            {
                tbPassword.Text = "";
                tbPassword.Focus();
                lbWrongPassword.Visible = true;
            }
            else
            {
                lbWrongPassword.Visible = false;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PasswordForm_Resize(object sender, EventArgs e)
        {
            MainForm.ScaleButtons(this, btCancel, btOK, 0);
        }
    }
}