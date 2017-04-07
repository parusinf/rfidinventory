// File: LocationForm.Designer.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

namespace ru.nikitin.RFIDInventory.Forms
{
    partial class LocationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lbState = new System.Windows.Forms.Label();
            this.lbLocationId = new System.Windows.Forms.Label();
            this.lbLocationIdText = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbNameText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.BackColor = System.Drawing.Color.OrangeRed;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btCancel.Location = new System.Drawing.Point(4, 283);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(114, 34);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "Exit";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.BackColor = System.Drawing.Color.DarkOrange;
            this.btOK.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btOK.Location = new System.Drawing.Point(122, 283);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(114, 34);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "Write";
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // lbState
            // 
            this.lbState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbState.BackColor = System.Drawing.Color.Yellow;
            this.lbState.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lbState.Location = new System.Drawing.Point(2, 3);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(236, 20);
            this.lbState.Text = "Location";
            this.lbState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbLocationId
            // 
            this.lbLocationId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLocationId.ForeColor = System.Drawing.Color.Blue;
            this.lbLocationId.Location = new System.Drawing.Point(4, 43);
            this.lbLocationId.Name = "lbLocationId";
            this.lbLocationId.Size = new System.Drawing.Size(232, 20);
            this.lbLocationId.Text = "id";
            // 
            // lbLocationIdText
            // 
            this.lbLocationIdText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLocationIdText.Location = new System.Drawing.Point(4, 26);
            this.lbLocationIdText.Name = "lbLocationIdText";
            this.lbLocationIdText.Size = new System.Drawing.Size(232, 20);
            this.lbLocationIdText.Text = "Location identificator:";
            // 
            // lbName
            // 
            this.lbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbName.ForeColor = System.Drawing.Color.Blue;
            this.lbName.Location = new System.Drawing.Point(4, 91);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(232, 20);
            this.lbName.Text = "name";
            // 
            // lbNameText
            // 
            this.lbNameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNameText.Location = new System.Drawing.Point(4, 74);
            this.lbNameText.Name = "lbNameText";
            this.lbNameText.Size = new System.Drawing.Size(232, 20);
            this.lbNameText.Text = "Location name:";
            // 
            // LocationForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbLocationId);
            this.Controls.Add(this.lbLocationIdText);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.lbNameText);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.lbState);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocationForm";
            this.Text = "LocationForm";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.LocationForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.Button btOK;
        public System.Windows.Forms.Label lbState;
        private System.Windows.Forms.Label lbLocationId;
        private System.Windows.Forms.Label lbLocationIdText;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbNameText;
    }
}