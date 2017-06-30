namespace FileAsciiViewer
{
    partial class frmFind
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.chkWholeWords = new System.Windows.Forms.CheckBox();
            this.cboFind = new System.Windows.Forms.ComboBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.cmdFind = new System.Windows.Forms.Button();
            this.chkBinary = new System.Windows.Forms.CheckBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(421, 109);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(67, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chkWholeWords
            // 
            this.chkWholeWords.AutoSize = true;
            this.chkWholeWords.Location = new System.Drawing.Point(18, 66);
            this.chkWholeWords.Name = "chkWholeWords";
            this.chkWholeWords.Size = new System.Drawing.Size(110, 17);
            this.chkWholeWords.TabIndex = 2;
            this.chkWholeWords.Text = "Whole words only";
            this.chkWholeWords.UseVisualStyleBackColor = true;
            // 
            // cboFind
            // 
            this.cboFind.FormattingEnabled = true;
            this.cboFind.Location = new System.Drawing.Point(15, 31);
            this.cboFind.Name = "cboFind";
            this.cboFind.Size = new System.Drawing.Size(449, 21);
            this.cboFind.TabIndex = 1;
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(15, 15);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(33, 13);
            this.lblFind.TabIndex = 0;
            this.lblFind.Text = "Find :";
            // 
            // cmdFind
            // 
            this.cmdFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFind.Location = new System.Drawing.Point(297, 109);
            this.cmdFind.Name = "cmdFind";
            this.cmdFind.Size = new System.Drawing.Size(111, 23);
            this.cmdFind.TabIndex = 4;
            this.cmdFind.Text = "&Find Next";
            this.cmdFind.Click += new System.EventHandler(this.cmdFind_Click);
            // 
            // chkBinary
            // 
            this.chkBinary.AutoSize = true;
            this.chkBinary.Location = new System.Drawing.Point(18, 89);
            this.chkBinary.Name = "chkBinary";
            this.chkBinary.Size = new System.Drawing.Size(117, 17);
            this.chkBinary.TabIndex = 6;
            this.chkBinary.Text = "Binary code search";
            this.chkBinary.UseVisualStyleBackColor = true;
            this.chkBinary.CheckedChanged += new System.EventHandler(this.chkBinary_CheckedChanged);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Location = new System.Drawing.Point(155, 91);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(331, 13);
            this.lblHint.TabIndex = 7;
            this.lblHint.Text = "Enter a sequence of binary codes (in decimal), separated by commas";
            this.lblHint.Visible = false;
            // 
            // frmFind
            // 
            this.AcceptButton = this.cmdFind;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(502, 146);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.chkBinary);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.chkWholeWords);
            this.Controls.Add(this.cboFind);
            this.Controls.Add(this.lblFind);
            this.Controls.Add(this.cmdFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFind";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.CheckBox chkWholeWords;
        internal System.Windows.Forms.ComboBox cboFind;
        internal System.Windows.Forms.Label lblFind;
        internal System.Windows.Forms.Button cmdFind;
        internal System.Windows.Forms.CheckBox chkBinary;
        private System.Windows.Forms.Label lblHint;
    }
}