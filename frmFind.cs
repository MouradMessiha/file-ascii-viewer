using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileAsciiViewer
{
    public partial class frmFind : Form
    {
        private frmMain mCallerForm;

        public frmFind()
        {
            InitializeComponent();
        }

        public frmMain CallerForm
        {
            set
            {
                mCallerForm = value;
            }
        }

        private void cmdFind_Click(object sender, EventArgs e)
        {
            mCallerForm.FindInFile(cboFind.Text, chkWholeWords.Checked, chkBinary.Checked);
            this.Focus();   
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkBinary_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBinary.Checked)
            {
                lblHint.Visible = true;
            }
            else
            {
                lblHint.Visible = false;
            }
        }
    }
}
