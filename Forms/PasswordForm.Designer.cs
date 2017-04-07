// File: PasswordForm.Designer.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

namespace ru.nikitin.RFIDInventory
{
    partial class PasswordForm
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
            this.lbPasswordCaption = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lbWrongPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbPasswordCaption
            // 
            this.lbPasswordCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPasswordCaption.BackColor = System.Drawing.Color.Yellow;
            this.lbPasswordCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lbPasswordCaption.Location = new System.Drawing.Point(2, 2);
            this.lbPasswordCaption.Name = "lbPasswordCaption";
            this.lbPasswordCaption.Size = new System.Drawing.Size(236, 20);
            this.lbPasswordCaption.Text = "Password";
            this.lbPasswordCaption.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(73, 139);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 23);
            this.tbPassword.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.BackColor = System.Drawing.Color.OrangeRed;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btCancel.Location = new System.Drawing.Point(4, 282);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(114, 34);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "< Cancel";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.BackColor = System.Drawing.Color.DarkOrange;
            this.btOK.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btOK.Location = new System.Drawing.Point(122, 282);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(114, 34);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "OK >";
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // lbWrongPassword
            // 
            this.lbWrongPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbWrongPassword.BackColor = System.Drawing.Color.Red;
            this.lbWrongPassword.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lbWrongPassword.ForeColor = System.Drawing.Color.White;
            this.lbWrongPassword.Location = new System.Drawing.Point(2, 256);
            this.lbWrongPassword.Name = "lbWrongPassword";
            this.lbWrongPassword.Size = new System.Drawing.Size(236, 20);
            this.lbWrongPassword.Text = "Wrong password";
            this.lbWrongPassword.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PasswordForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lbWrongPassword);
            this.Controls.Add(this.lbPasswordCaption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.Text = "PasswordForm";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.PasswordForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.Label lbPasswordCaption;
        System.Windows.Forms.TextBox tbPassword;
        System.Windows.Forms.Button btCancel;
        System.Windows.Forms.Button btOK;
        System.Windows.Forms.Label lbWrongPassword;
    }
}