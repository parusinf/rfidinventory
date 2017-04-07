// File: ObjectForm.Designer.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

namespace ru.nikitin.RFIDInventory.Forms
{
    partial class ObjectForm
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
            this.lbState = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lbInvNumberText = new System.Windows.Forms.Label();
            this.lbInvNumber = new System.Windows.Forms.Label();
            this.lbSerialText = new System.Windows.Forms.Label();
            this.lbSerial = new System.Windows.Forms.Label();
            this.lbNomenclaruteText = new System.Windows.Forms.Label();
            this.lbNomenclature = new System.Windows.Forms.Label();
            this.lbObjectIdText = new System.Windows.Forms.Label();
            this.lbObjectId = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.lbState.Text = "Object";
            this.lbState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "< Close";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.BackColor = System.Drawing.Color.DarkOrange;
            this.btOK.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular);
            this.btOK.Location = new System.Drawing.Point(122, 282);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(114, 34);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "Write";
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // lbInvNumberText
            // 
            this.lbInvNumberText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInvNumberText.Location = new System.Drawing.Point(4, 74);
            this.lbInvNumberText.Name = "lbInvNumberText";
            this.lbInvNumberText.Size = new System.Drawing.Size(232, 20);
            this.lbInvNumberText.Text = "Iventory number:";
            // 
            // lbInvNumber
            // 
            this.lbInvNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInvNumber.ForeColor = System.Drawing.Color.Blue;
            this.lbInvNumber.Location = new System.Drawing.Point(4, 91);
            this.lbInvNumber.Name = "lbInvNumber";
            this.lbInvNumber.Size = new System.Drawing.Size(232, 20);
            this.lbInvNumber.Text = "inventoryNumber";
            // 
            // lbSerialText
            // 
            this.lbSerialText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSerialText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbSerialText.Location = new System.Drawing.Point(4, 123);
            this.lbSerialText.Name = "lbSerialText";
            this.lbSerialText.Size = new System.Drawing.Size(232, 20);
            this.lbSerialText.Text = "Serial number:";
            // 
            // lbSerial
            // 
            this.lbSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSerial.ForeColor = System.Drawing.Color.Blue;
            this.lbSerial.Location = new System.Drawing.Point(4, 140);
            this.lbSerial.Name = "lbSerial";
            this.lbSerial.Size = new System.Drawing.Size(232, 20);
            this.lbSerial.Text = "serialNumber";
            // 
            // lbNomenclaruteText
            // 
            this.lbNomenclaruteText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNomenclaruteText.Location = new System.Drawing.Point(4, 172);
            this.lbNomenclaruteText.Name = "lbNomenclaruteText";
            this.lbNomenclaruteText.Size = new System.Drawing.Size(232, 20);
            this.lbNomenclaruteText.Text = "Nomenclature name:";
            // 
            // lbNomenclature
            // 
            this.lbNomenclature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNomenclature.ForeColor = System.Drawing.Color.Blue;
            this.lbNomenclature.Location = new System.Drawing.Point(4, 189);
            this.lbNomenclature.Name = "lbNomenclature";
            this.lbNomenclature.Size = new System.Drawing.Size(232, 69);
            this.lbNomenclature.Text = "nomenclatureName";
            // 
            // lbObjectIdText
            // 
            this.lbObjectIdText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbObjectIdText.Location = new System.Drawing.Point(4, 26);
            this.lbObjectIdText.Name = "lbObjectIdText";
            this.lbObjectIdText.Size = new System.Drawing.Size(232, 20);
            this.lbObjectIdText.Text = "Object identificator:";
            // 
            // lbObjectId
            // 
            this.lbObjectId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbObjectId.ForeColor = System.Drawing.Color.Blue;
            this.lbObjectId.Location = new System.Drawing.Point(4, 43);
            this.lbObjectId.Name = "lbObjectId";
            this.lbObjectId.Size = new System.Drawing.Size(232, 20);
            this.lbObjectId.Text = "id";
            // 
            // ObjectForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lbObjectId);
            this.Controls.Add(this.lbObjectIdText);
            this.Controls.Add(this.lbNomenclature);
            this.Controls.Add(this.lbNomenclaruteText);
            this.Controls.Add(this.lbSerial);
            this.Controls.Add(this.lbSerialText);
            this.Controls.Add(this.lbInvNumber);
            this.Controls.Add(this.lbInvNumberText);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.lbState);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectForm";
            this.Text = "ObjectForm";
            this.TopMost = true;
            this.GotFocus += new System.EventHandler(this.ObjectForm_GotFocus);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ObjectForm_KeyPress);
            this.Resize += new System.EventHandler(this.ObjectForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lbState;
        public System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label lbInvNumberText;
        private System.Windows.Forms.Label lbInvNumber;
        private System.Windows.Forms.Label lbSerialText;
        private System.Windows.Forms.Label lbSerial;
        private System.Windows.Forms.Label lbNomenclaruteText;
        private System.Windows.Forms.Label lbNomenclature;
        private System.Windows.Forms.Label lbObjectIdText;
        private System.Windows.Forms.Label lbObjectId;
    }
}