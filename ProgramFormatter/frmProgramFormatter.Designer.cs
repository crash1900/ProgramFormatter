namespace ProgramFormatter
{
    partial class frmProgramFormatter
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
            this.lblFile = new System.Windows.Forms.Label();
            this.txtSelectedFile = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboBlockNumber = new System.Windows.Forms.ComboBox();
            this.btnGenerateSheet = new System.Windows.Forms.Button();
            this.chkSaveNewFile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(28, 36);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(87, 20);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "Select File:";
            // 
            // txtSelectedFile
            // 
            this.txtSelectedFile.Location = new System.Drawing.Point(148, 36);
            this.txtSelectedFile.Name = "txtSelectedFile";
            this.txtSelectedFile.ReadOnly = true;
            this.txtSelectedFile.Size = new System.Drawing.Size(420, 26);
            this.txtSelectedFile.TabIndex = 2;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(589, 33);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(43, 33);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Block Number:";
            // 
            // cboBlockNumber
            // 
            this.cboBlockNumber.FormattingEnabled = true;
            this.cboBlockNumber.Location = new System.Drawing.Point(148, 89);
            this.cboBlockNumber.Name = "cboBlockNumber";
            this.cboBlockNumber.Size = new System.Drawing.Size(121, 28);
            this.cboBlockNumber.TabIndex = 4;
            // 
            // btnGenerateSheet
            // 
            this.btnGenerateSheet.Enabled = false;
            this.btnGenerateSheet.Location = new System.Drawing.Point(32, 195);
            this.btnGenerateSheet.Name = "btnGenerateSheet";
            this.btnGenerateSheet.Size = new System.Drawing.Size(135, 32);
            this.btnGenerateSheet.TabIndex = 5;
            this.btnGenerateSheet.Text = "Generate";
            this.btnGenerateSheet.UseVisualStyleBackColor = true;
            this.btnGenerateSheet.Click += new System.EventHandler(this.btnGenerateSheet_Click);
            // 
            // chkSaveNewFile
            // 
            this.chkSaveNewFile.AutoSize = true;
            this.chkSaveNewFile.Enabled = false;
            this.chkSaveNewFile.Location = new System.Drawing.Point(32, 137);
            this.chkSaveNewFile.Name = "chkSaveNewFile";
            this.chkSaveNewFile.Size = new System.Drawing.Size(153, 24);
            this.chkSaveNewFile.TabIndex = 6;
            this.chkSaveNewFile.Text = "Save to New File";
            this.chkSaveNewFile.UseVisualStyleBackColor = true;
            // 
            // frmProgramFormatter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 248);
            this.Controls.Add(this.chkSaveNewFile);
            this.Controls.Add(this.btnGenerateSheet);
            this.Controls.Add(this.cboBlockNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtSelectedFile);
            this.Controls.Add(this.lblFile);
            this.Name = "frmProgramFormatter";
            this.Text = "Program Formatter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtSelectedFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBlockNumber;
        private System.Windows.Forms.Button btnGenerateSheet;
        private System.Windows.Forms.CheckBox chkSaveNewFile;
    }
}

