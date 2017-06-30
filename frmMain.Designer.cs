namespace FileAsciiViewer
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGoToCharPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTextViewer = new System.Windows.Forms.ToolStripMenuItem();
            this.txtGoToPosition = new System.Windows.Forms.TextBox();
            this.lblGoToPosition = new System.Windows.Forms.Label();
            this.btnGoToPosition = new System.Windows.Forms.Button();
            this.txtTextViewer = new System.Windows.Forms.TextBox();
            this.mnuContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuContext
            // 
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowGrid,
            this.mnuGoToCharPosition,
            this.mnuTextViewer});
            this.mnuContext.Name = "mnuContext";
            this.mnuContext.Size = new System.Drawing.Size(176, 70);
            // 
            // mnuShowGrid
            // 
            this.mnuShowGrid.CheckOnClick = true;
            this.mnuShowGrid.Name = "mnuShowGrid";
            this.mnuShowGrid.Size = new System.Drawing.Size(175, 22);
            this.mnuShowGrid.Text = "&Show grid";
            this.mnuShowGrid.Click += new System.EventHandler(this.mnuShowGrid_Click);
            // 
            // mnuGoToCharPosition
            // 
            this.mnuGoToCharPosition.Name = "mnuGoToCharPosition";
            this.mnuGoToCharPosition.Size = new System.Drawing.Size(175, 22);
            this.mnuGoToCharPosition.Text = "&Go to char position";
            this.mnuGoToCharPosition.Click += new System.EventHandler(this.mnuGoToCharPosition_Click);
            // 
            // mnuTextViewer
            // 
            this.mnuTextViewer.CheckOnClick = true;
            this.mnuTextViewer.Name = "mnuTextViewer";
            this.mnuTextViewer.Size = new System.Drawing.Size(175, 22);
            this.mnuTextViewer.Text = "&Text viewer";
            this.mnuTextViewer.Click += new System.EventHandler(this.mnuTextViewer_Click);
            // 
            // txtGoToPosition
            // 
            this.txtGoToPosition.Location = new System.Drawing.Point(475, 7);
            this.txtGoToPosition.Name = "txtGoToPosition";
            this.txtGoToPosition.Size = new System.Drawing.Size(183, 20);
            this.txtGoToPosition.TabIndex = 1;
            this.txtGoToPosition.Visible = false;
            this.txtGoToPosition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGoToPosition_KeyDown);
            // 
            // lblGoToPosition
            // 
            this.lblGoToPosition.AutoSize = true;
            this.lblGoToPosition.Location = new System.Drawing.Point(364, 11);
            this.lblGoToPosition.Name = "lblGoToPosition";
            this.lblGoToPosition.Size = new System.Drawing.Size(108, 13);
            this.lblGoToPosition.TabIndex = 2;
            this.lblGoToPosition.Text = "Go To Char Position :";
            this.lblGoToPosition.Visible = false;
            // 
            // btnGoToPosition
            // 
            this.btnGoToPosition.Location = new System.Drawing.Point(667, 6);
            this.btnGoToPosition.Name = "btnGoToPosition";
            this.btnGoToPosition.Size = new System.Drawing.Size(44, 22);
            this.btnGoToPosition.TabIndex = 3;
            this.btnGoToPosition.Text = "Go";
            this.btnGoToPosition.UseVisualStyleBackColor = true;
            this.btnGoToPosition.Visible = false;
            this.btnGoToPosition.Click += new System.EventHandler(this.btnGoToPosition_Click);
            // 
            // txtTextViewer
            // 
            this.txtTextViewer.BackColor = System.Drawing.Color.White;
            this.txtTextViewer.Location = new System.Drawing.Point(378, 233);
            this.txtTextViewer.Multiline = true;
            this.txtTextViewer.Name = "txtTextViewer";
            this.txtTextViewer.ReadOnly = true;
            this.txtTextViewer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTextViewer.Size = new System.Drawing.Size(368, 193);
            this.txtTextViewer.TabIndex = 5;
            this.txtTextViewer.Visible = false;
            this.txtTextViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtTextViewer_MouseClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 717);
            this.ContextMenuStrip = this.mnuContext;
            this.Controls.Add(this.txtTextViewer);
            this.Controls.Add(this.btnGoToPosition);
            this.Controls.Add(this.lblGoToPosition);
            this.Controls.Add(this.txtGoToPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Ascii Viewer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseWheel);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.mnuContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuContext;
        private System.Windows.Forms.ToolStripMenuItem mnuShowGrid;
        private System.Windows.Forms.TextBox txtGoToPosition;
        private System.Windows.Forms.Label lblGoToPosition;
        private System.Windows.Forms.Button btnGoToPosition;
        private System.Windows.Forms.ToolStripMenuItem mnuGoToCharPosition;
        private System.Windows.Forms.ToolStripMenuItem mnuTextViewer;
        private System.Windows.Forms.TextBox txtTextViewer;

    }
}

