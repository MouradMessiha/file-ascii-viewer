using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace FileAsciiViewer
{
    public partial class frmMain : Form
    {
        private long mlngLength;
        private string mstrFileName = "";
        private byte[] marrFileContents;
        private Bitmap mobjFormBitmap;
        private Graphics mobjBitmapGraphics;
        private int mintFormWidth;
        private int mintFormHeight;
        private Boolean mblnDoneOnce = false;
        private int mintCharacterAreaX1;
        private int mintCharacterAreaY1;
        private int mintCharacterAreaX2;
        private int mintCharacterAreaY2;
        private int mintAsciiAreaX1;
        private int mintAsciiAreaY1;
        private int mintAsciiAreaX2;
        private int mintAsciiAreaY2;
        private int mintNumberColumns = 40;
        private int mintNumberRows = 15;
        private int mintCellWidth;
        private int mintCellHeight;
        private long mlngStartCharIndex;
        private long mlngSelectedCharIndex;
        private long mlngSelectionLength;
        private long mlngContentsStart;
        private frmFind mfrmFind;
        private List<string> mlstPreviousFindInFile;
        private string mstrFindInFile = "";
        private bool mblnWholeWordsFindInFile;
        private bool mblnBinaryCode;

        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (! mblnDoneOnce) 
            {
                mblnDoneOnce = true;
                mintFormWidth = this.Width;
                mintFormHeight = this.Height;
                mobjFormBitmap = new Bitmap(mintFormWidth, mintFormHeight, this.CreateGraphics());
                mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap);
                mintCharacterAreaX1= 10;
                mintCharacterAreaY1=  30;
                mintCharacterAreaX2 = mintFormWidth - 10;
                mintCharacterAreaY2 =  30 + (mintFormHeight - 80) / 2;
                mintAsciiAreaX1 = mintCharacterAreaX1;
                mintAsciiAreaY2 = mintFormHeight - 50;
                mintCellWidth = (mintCharacterAreaX2 - mintCharacterAreaX1 ) / mintNumberColumns;
                mintCellHeight = (mintCharacterAreaY2 - mintCharacterAreaY1 ) / mintNumberRows;
                mintCharacterAreaX2 = mintCharacterAreaX1 + mintCellWidth * mintNumberColumns;   // recalculate to make it an even number of pixels per column
                mintAsciiAreaX2 = mintCharacterAreaX2;
                mintCharacterAreaY2 = mintCharacterAreaY1 + mintCellHeight * mintNumberRows;     // recalculate to make it an even number of pixels per row
                mintAsciiAreaY1 = mintCharacterAreaY2 + 1;
                mintAsciiAreaY2 = mintAsciiAreaY1 + mintCellHeight * mintNumberRows;             // recalculate to make it an even number of pixels per row

                txtTextViewer.Left = mintCharacterAreaX1;
                txtTextViewer.Top = mintCharacterAreaY1;
                txtTextViewer.Width = mintCharacterAreaX2 - mintCharacterAreaX1;
                txtTextViewer.Height = mintAsciiAreaY2 - mintCharacterAreaY1;
                
                if (mstrFileName != "")
                {
                    DisplayFile();
                    this.WindowState = FormWindowState.Minimized;   // bring form to front as it always starts behind
                    this.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(mobjFormBitmap, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do nothing
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            mlstPreviousFindInFile = new List<string>();
            OpenFileDialog objDialog = new OpenFileDialog();
            objDialog.InitialDirectory = "C:\\";
            objDialog.DefaultExt = ".*"; 
            objDialog.Filter = "All files|*.*"; 

            DialogResult result = objDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                mstrFileName = objDialog.FileName;

                LoadContentsArray(0);
                mlngSelectedCharIndex = 0;
                mlngSelectionLength = 0;
                mlngStartCharIndex = 0;
            }
            else
            {
                Application.Exit();
            }

        }

        private void LoadContentsArray(long plngStartPosition)
        // read the file into the byte array, starting from a given position in the file
        {            
            FileStream objFileStream = new FileStream(mstrFileName, FileMode.Open, FileAccess.Read);
            mlngLength = objFileStream.Length;
            objFileStream.Seek(plngStartPosition,SeekOrigin.Begin);

            try
            {                
                int intAllocatedLength;
               
                if (mlngLength - plngStartPosition  < 1048576)  // less than a megabyte
                {
                    intAllocatedLength = (int)(mlngLength - plngStartPosition);
                }
                else
                {
                    intAllocatedLength = 1048576;
                }
                marrFileContents = new byte[intAllocatedLength];

                int intCount;
                int intOffset = 0;
                
                while ((intCount = objFileStream.Read(marrFileContents, intOffset, intAllocatedLength - intOffset)) > 0)
                    intOffset += intCount;

                mlngContentsStart = plngStartPosition;
                txtTextViewer.Text = Encoding.Default.GetString(marrFileContents);
            }
            finally
            {
                objFileStream.Close();
            }
        }

        private void DisplayFile()
        {
            if (mnuTextViewer.Checked)
            {
                // white form
                mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight);

                this.Invalidate();
                
                txtTextViewer.SelectionStart = (int)(mlngSelectedCharIndex - mlngContentsStart);
                txtTextViewer.SelectionLength = (int)mlngSelectionLength;
                txtTextViewer.ScrollToCaret();                
            }
            else
            {
                int intX;
                int intY;
                bool blnHighlighted = false;

                // white form
                mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight);
                // character area rectangle
                mobjBitmapGraphics.DrawRectangle(Pens.Black, mintCharacterAreaX1, mintCharacterAreaY1, mintCharacterAreaX2 - mintCharacterAreaX1, mintCharacterAreaY2 - mintCharacterAreaY1);
                // ascii area rectangle
                mobjBitmapGraphics.DrawRectangle(Pens.Black, mintAsciiAreaX1, mintAsciiAreaY1, mintAsciiAreaX2 - mintAsciiAreaX1, mintAsciiAreaY2 - mintAsciiAreaY1);
                // draw columns lines
                for (int intCounter = 1; intCounter < mintNumberColumns; intCounter++)
                {
                    intX = mintCharacterAreaX1 + (intCounter * (mintCharacterAreaX2 - mintCharacterAreaX1) / mintNumberColumns);
                    if (mnuShowGrid.Checked)
                    {
                        mobjBitmapGraphics.DrawLine(Pens.Black, intX, mintCharacterAreaY1, intX, mintAsciiAreaY2);
                    }
                }
                // draw row lines in character area
                for (int intCounter = 1; intCounter < mintNumberRows; intCounter++)
                {
                    intY = mintCharacterAreaY1 + (intCounter * (mintCharacterAreaY2 - mintCharacterAreaY1) / mintNumberRows);
                    if (mnuShowGrid.Checked)
                    {
                        mobjBitmapGraphics.DrawLine(Pens.Black, mintCharacterAreaX1, intY, mintCharacterAreaX2, intY);
                    }
                }
                // draw row lines in ascii area
                for (int intCounter = 1; intCounter < mintNumberRows; intCounter++)
                {
                    intY = mintAsciiAreaY1 + (intCounter * (mintAsciiAreaY2 - mintAsciiAreaY1) / mintNumberRows);
                    if (mnuShowGrid.Checked)
                    {
                        mobjBitmapGraphics.DrawLine(Pens.Black, mintAsciiAreaX1, intY, mintAsciiAreaX2, intY);
                    }
                }
                // draw line separating the character area from the ascii area
                mobjBitmapGraphics.DrawLine(Pens.Red, mintCharacterAreaX1 + 1, mintCharacterAreaY2, mintCharacterAreaX2, mintCharacterAreaY2);
                mobjBitmapGraphics.DrawLine(Pens.Red, mintAsciiAreaX1 + 1, mintAsciiAreaY1, mintAsciiAreaX2, mintAsciiAreaY1);

                Font objFont1;
                Font objFont2;
                objFont1 = new Font("MS Sans Serif", 14, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
                objFont2 = new Font("MS Sans Serif", 8, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);

                // display the file contents and its ascii codes
                for (int intYCounter = 1; intYCounter <= mintNumberRows; intYCounter++)
                {
                    for (int intXCounter = 1; intXCounter <= mintNumberColumns; intXCounter++)
                    {
                        long lngCharIndex;       // index of character to be displayed, zero based
                        int intCellLeft;
                        int intCellTop1;
                        int intCellTop2;

                        lngCharIndex = mlngStartCharIndex + intXCounter + ((intYCounter - 1) * mintNumberColumns) - 1;

                        // get location where character should be displayed
                        intCellLeft = mintCharacterAreaX1 + (((intXCounter - 1) * (mintCharacterAreaX2 - mintCharacterAreaX1)) / mintNumberColumns);
                        intCellTop1 = mintCharacterAreaY1 + (((intYCounter - 1) * (mintCharacterAreaY2 - mintCharacterAreaY1)) / mintNumberRows);
                        intCellTop2 = mintAsciiAreaY1 + (((intYCounter - 1) * (mintAsciiAreaY2 - mintAsciiAreaY1)) / mintNumberRows);

                        if (lngCharIndex < mlngLength)
                        {
                            mobjBitmapGraphics.FillRectangle(Brushes.LightGray, intCellLeft + 1, intCellTop1 + 1, mintCellWidth - 1, mintCellHeight - 1);
                            mobjBitmapGraphics.FillRectangle(Brushes.LightGray, intCellLeft + 1, intCellTop2 + 1, mintCellWidth - 1, mintCellHeight - 1);

                            string strCharacter;
                            SizeF objSize;

                            blnHighlighted = false;
                            if (lngCharIndex >= mlngSelectedCharIndex & lngCharIndex <= mlngSelectedCharIndex + mlngSelectionLength - 1)
                            {
                                // highlight cell in blue
                                mobjBitmapGraphics.FillRectangle(Brushes.Blue, intCellLeft + 1, intCellTop1 + 1, mintCellWidth - 1, mintCellHeight - 1);
                                mobjBitmapGraphics.FillRectangle(Brushes.Blue, intCellLeft + 1, intCellTop2 + 1, mintCellWidth - 1, mintCellHeight - 1);
                                blnHighlighted = true;
                            }

                            strCharacter = Encoding.Default.GetString(marrFileContents, (int)(lngCharIndex - mlngContentsStart), 1);
                            objSize = mobjBitmapGraphics.MeasureString(strCharacter, objFont1, 100);

                            intX = intCellLeft + (mintCellWidth / 2) - ((int)objSize.Width / 2);
                            intY = intCellTop1 + (mintCellHeight / 2) - ((int)objSize.Height / 2);
                            if (blnHighlighted)
                            {
                                mobjBitmapGraphics.DrawString(strCharacter, objFont1, Brushes.White, (float)intX, (float)intY);
                            }
                            else
                            {
                                mobjBitmapGraphics.DrawString(strCharacter, objFont1, Brushes.Black, (float)intX, (float)intY);
                            }

                            strCharacter = marrFileContents[lngCharIndex - mlngContentsStart].ToString();
                            objSize = mobjBitmapGraphics.MeasureString(strCharacter, objFont2, 100);

                            intX = intCellLeft + (mintCellWidth / 2) - ((int)objSize.Width / 2);
                            intY = intCellTop2 + (mintCellHeight / 2) - ((int)objSize.Height / 2);
                            if (blnHighlighted)
                            {
                                mobjBitmapGraphics.DrawString(strCharacter, objFont2, Brushes.White, (float)intX, (float)intY);
                            }
                            else
                            {
                                mobjBitmapGraphics.DrawString(strCharacter, objFont2, Brushes.Black, (float)intX, (float)intY);
                            }

                            //highlight the cell if this is the selected character
                            if (lngCharIndex == mlngSelectedCharIndex)
                            {
                                mobjBitmapGraphics.DrawRectangle(Pens.Black, intCellLeft + 1, intCellTop1 + 1, mintCellWidth - 1, mintCellHeight - 1);
                                mobjBitmapGraphics.DrawRectangle(Pens.Black, intCellLeft + 2, intCellTop1 + 2, mintCellWidth - 3, mintCellHeight - 3);
                                mobjBitmapGraphics.DrawRectangle(Pens.Black, intCellLeft + 3, intCellTop1 + 3, mintCellWidth - 5, mintCellHeight - 5);

                                mobjBitmapGraphics.DrawRectangle(Pens.Black, intCellLeft + 1, intCellTop2 + 1, mintCellWidth - 1, mintCellHeight - 1);
                                mobjBitmapGraphics.DrawRectangle(Pens.Black, intCellLeft + 2, intCellTop2 + 2, mintCellWidth - 3, mintCellHeight - 3);
                                mobjBitmapGraphics.DrawRectangle(Pens.Black, intCellLeft + 3, intCellTop2 + 3, mintCellWidth - 5, mintCellHeight - 5);

                                // write the current position on top of the form
                                mobjBitmapGraphics.DrawString("Position: " + mlngSelectedCharIndex.ToString() + "       File size: " + mlngLength.ToString(), objFont1, Brushes.Black, 5, 5);
                            }
                        }
                    }
                }
                this.Invalidate();
            }

            // hide these controls as they are confusing because they have the focus but the form gets the keydown events
            lblGoToPosition.Visible = false;
            txtGoToPosition.Visible = false;
            btnGoToPosition.Visible = false;
        }

        private void mnuShowGrid_Click(object sender, EventArgs e)
        {
            DisplayFile();
        }

        private void frmMain_MouseClick(object sender, MouseEventArgs e)
        {
            int intXCellIndex;
            int intYCellIndex;
            long lngNewSelectedIndex;

            if (!mnuTextViewer.Checked)
            {
                if (e.X >= mintCharacterAreaX1 && e.X < mintCharacterAreaX2)
                {
                    if (e.Y >= mintCharacterAreaY1 && e.Y < mintCharacterAreaY2)       // mouse clicked in upper rectangle
                    {
                        intXCellIndex = 1 + (e.X - mintCharacterAreaX1) / ((mintCharacterAreaX2 - mintCharacterAreaX1) / mintNumberColumns);
                        intYCellIndex = 1 + (e.Y - mintCharacterAreaY1) / ((mintCharacterAreaY2 - mintCharacterAreaY1) / mintNumberRows);

                        lngNewSelectedIndex = mlngStartCharIndex + intXCellIndex + ((intYCellIndex - 1) * mintNumberColumns) - 1;
                        if (lngNewSelectedIndex <= mlngLength - 1)
                        {
                            mlngSelectedCharIndex = lngNewSelectedIndex;
                            mlngSelectionLength = 0;
                            DisplayFile();
                        }
                    }
                    else
                        if (e.Y >= mintAsciiAreaY1 && e.Y < mintAsciiAreaY2)               // mouse clicked in lower rectangle
                        {
                            intXCellIndex = 1 + (e.X - mintAsciiAreaX1) / ((mintAsciiAreaX2 - mintAsciiAreaX1) / mintNumberColumns);
                            intYCellIndex = 1 + (e.Y - mintAsciiAreaY1) / ((mintAsciiAreaY2 - mintAsciiAreaY1) / mintNumberRows);

                            lngNewSelectedIndex = mlngStartCharIndex + intXCellIndex + ((intYCellIndex - 1) * mintNumberColumns) - 1;
                            if (lngNewSelectedIndex <= mlngLength - 1)
                            {
                                mlngSelectedCharIndex = lngNewSelectedIndex;
                                mlngSelectionLength = 0;
                                DisplayFile();
                            }
                        }
                }
            }
        }

        private void txtTextViewer_MouseClick(object sender, MouseEventArgs e)
        {
            mlngSelectedCharIndex = mlngContentsStart + txtTextViewer.SelectionStart;
            mlngSelectionLength = txtTextViewer.SelectionLength;
            if ((mlngSelectedCharIndex < mlngStartCharIndex) | (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1))
            {
                mlngStartCharIndex = mlngSelectedCharIndex / mintNumberColumns;      // get number of rows before the current selected cell
                mlngStartCharIndex = mlngStartCharIndex * mintNumberColumns;         // scroll to the current line
                AdjustArrayLocationInfile();
                DisplayFile();
            }
        }

        void AdjustArrayLocationInfile()
        // check if the current position within the array requires loading the array from a new position in the file
        {
            if ((mlngStartCharIndex - mlngContentsStart <= marrFileContents.Length - 1) & (mlngStartCharIndex - mlngContentsStart >= 0)) // we are still withing the array
            {
                if (mlngStartCharIndex - mlngContentsStart > 786432) // 3 quarters of a megabyte into array
                {
                    if (mlngLength > mlngContentsStart + marrFileContents.Length) // if there is more in the file than the array already has
                    {
                        LoadContentsArray(mlngContentsStart + (262144 - (262144 % mintNumberColumns))); // move array quarter a megabyte down in the file
                    }
                }
                else
                {
                    if (mlngStartCharIndex - mlngContentsStart < 262144)  // a quarter megabyte from beginning of array
                    {
                        if (mlngContentsStart > 0)                        // there is room in file to move up
                        {
                            if (mlngContentsStart > 262144)               // array is more than quarter a megabyte from beginning of file
                            {
                                LoadContentsArray(mlngContentsStart - (262144 - (262144 % mintNumberColumns))); // move array quarter a megabyte up in the file
                            }
                            else
                            {
                                LoadContentsArray(0);                       // move array to beginnign of file
                            }
                        }
                    }
                }
            }
            else               // user wants to jump to a position outside the current array
            {
                // move array half a megabyte before start position
                if (mlngStartCharIndex > 524288)    // more than half a megabyte from start of file
                {
                    LoadContentsArray(mlngStartCharIndex - (524288 - (524288 % mintNumberColumns))); // move array half a megabyte before start position
                }
                else
                {
                    LoadContentsArray(0);                       // move array to beginnign of file
                }
            }
        }

        void frmMain_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!mnuTextViewer.Checked)
            {
                if (e.Delta > 0)  // scroll up
                {
                    if (mlngStartCharIndex - mintNumberColumns >= 0) // enough to scroll up the view one line
                    {
                        mlngSelectedCharIndex -= mintNumberColumns;
                        mlngStartCharIndex -= mintNumberColumns;
                        AdjustArrayLocationInfile();
                        DisplayFile();
                    }
                    else
                    {
                        if (mlngSelectedCharIndex - mintNumberColumns >= 0)   // enough space to move up the selected character one line
                            mlngSelectedCharIndex -= mintNumberColumns;
                        else
                            mlngSelectedCharIndex = 0;      // highlight first character of the file
                        DisplayFile();
                    }
                }
                else   // scroll down
                {
                    if (mlngSelectedCharIndex + mintNumberColumns <= mlngLength - 1) //enough left in file to move one line down
                    {
                        mlngSelectedCharIndex += mintNumberColumns;
                        mlngStartCharIndex += mintNumberColumns;
                        AdjustArrayLocationInfile();
                        DisplayFile();
                    }
                    else
                    {
                        if (mlngStartCharIndex + mintNumberColumns <= mlngLength - 1)  // we can still scroll view one line down
                        {
                            mlngStartCharIndex += mintNumberColumns;
                        }
                        mlngSelectedCharIndex = mlngLength - 1;     // highlight last byte in the file
                        DisplayFile();
                    }
                }
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            long lngNewSelectedIndex;

            if (!mnuTextViewer.Checked)
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        if (mlngSelectedCharIndex < mlngLength - 1)
                        {
                            mlngSelectedCharIndex += 1;
                            mlngSelectionLength = 0;
                            if (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1)
                            {
                                mlngStartCharIndex += mintNumberColumns;
                                AdjustArrayLocationInfile();
                            }
                            DisplayFile();
                        }

                        break;

                    case Keys.Left:
                        if (mlngSelectedCharIndex > 0)
                        {
                            mlngSelectedCharIndex -= 1;
                            mlngSelectionLength = 0;
                            if (mlngSelectedCharIndex < mlngStartCharIndex)
                            {
                                mlngStartCharIndex -= mintNumberColumns;
                                AdjustArrayLocationInfile();
                            }
                            DisplayFile();
                        }

                        break;

                    case Keys.Down:
                        if (mlngSelectedCharIndex + mintNumberColumns < mlngLength) // can go one line down and not reach eof
                        {
                            mlngSelectedCharIndex += mintNumberColumns;
                            mlngSelectionLength = 0;
                            if (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1) // need to scroll to show new selection
                            {
                                mlngStartCharIndex += mintNumberColumns;
                                AdjustArrayLocationInfile();
                            }
                            DisplayFile();
                        }
                        else   // go to last character
                        {
                            mlngSelectedCharIndex = mlngLength - 1;     // highlight last byte in the file
                            mlngSelectionLength = 0;
                            if (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1) // need to scroll to show new selection
                            {
                                mlngStartCharIndex += mintNumberColumns;
                                AdjustArrayLocationInfile();
                            }
                            DisplayFile();
                        }

                        break;

                    case Keys.Up:
                        if (mlngSelectedCharIndex - mintNumberColumns >= 0)  //can go one line up and not reach bof
                        {
                            mlngSelectedCharIndex -= mintNumberColumns;
                            mlngSelectionLength = 0;
                            if (mlngSelectedCharIndex < mlngStartCharIndex) // need to scroll to show new selection
                            {
                                mlngStartCharIndex -= mintNumberColumns;
                                AdjustArrayLocationInfile();
                            }
                            DisplayFile();
                        }
                        else  //  we are on the first line, go to first character
                        {
                            mlngSelectedCharIndex = 0;
                            mlngSelectionLength = 0;
                            DisplayFile();
                        }

                        break;

                    case Keys.PageDown:
                        if (mlngSelectedCharIndex + (mintNumberRows * mintNumberColumns) <= mlngLength - 1) //enough left in file to move one page down
                        {
                            mlngSelectedCharIndex += mintNumberRows * mintNumberColumns;
                            mlngSelectionLength = 0;
                            mlngStartCharIndex += mintNumberRows * mintNumberColumns;
                            AdjustArrayLocationInfile();
                            DisplayFile();
                        }
                        else
                        {
                            if (mlngStartCharIndex + (mintNumberRows * mintNumberColumns) <= mlngLength - 1)  // we can still scroll view one page down
                            {
                                mlngStartCharIndex += mintNumberRows * mintNumberColumns;
                                AdjustArrayLocationInfile();
                            }
                            mlngSelectedCharIndex = mlngLength - 1;     // highlight last byte in the file
                            mlngSelectionLength = 0;
                            DisplayFile();
                        }

                        break;

                    case Keys.PageUp:
                        if (mlngStartCharIndex - (mintNumberRows * mintNumberColumns) >= 0) // enough to page up the view
                        {
                            mlngSelectedCharIndex -= mintNumberRows * mintNumberColumns;
                            mlngSelectionLength = 0;
                            mlngStartCharIndex -= mintNumberRows * mintNumberColumns;
                            AdjustArrayLocationInfile();
                            DisplayFile();
                        }
                        else
                        {
                            mlngStartCharIndex = 0;             // scroll view up to beginning of file
                            AdjustArrayLocationInfile();
                            if (mlngSelectedCharIndex - (mintNumberRows * mintNumberColumns) >= 0)   // enough space to page up the selected character
                            {
                                mlngSelectedCharIndex -= mintNumberRows * mintNumberColumns;
                                mlngSelectionLength = 0;
                            }
                            else
                            {
                                mlngSelectedCharIndex = 0;      // highlight first character of the file
                                mlngSelectionLength = 0;
                            }
                            DisplayFile();
                        }

                        break;

                    case Keys.Home:
                        if (e.Control)    // ctrl-Home
                        {
                            mlngStartCharIndex = 0;         // scroll view up to beginning of file
                            AdjustArrayLocationInfile();
                            mlngSelectedCharIndex = 0;      // highlight first character of the file                    
                            mlngSelectionLength = 0;
                            DisplayFile();
                        }
                        else             // Home
                        {
                            mlngSelectedCharIndex = mlngSelectedCharIndex / mintNumberColumns;   // get number of rows before the current selected cell
                            mlngSelectedCharIndex = mlngSelectedCharIndex * mintNumberColumns;   // go to first character on the line
                            mlngSelectionLength = 0;
                            DisplayFile();
                        }

                        break;

                    case Keys.End:
                        if (e.Control)   // ctrl-End
                        {
                            mlngSelectedCharIndex = mlngLength - 1;     // highlight last byte in the file
                            mlngSelectionLength = 0;
                            mlngStartCharIndex = (mlngLength - 1) / (mintNumberRows * mintNumberColumns); // get number of full screen scrolls required to view the end of the file
                            mlngStartCharIndex = mlngStartCharIndex * mintNumberRows * mintNumberColumns; // scroll down by that number of pages
                            AdjustArrayLocationInfile();
                            DisplayFile();
                        }
                        else            // End
                        {
                            lngNewSelectedIndex = 1 + (mlngSelectedCharIndex / mintNumberColumns);   // get number of rows before and including the current row
                            lngNewSelectedIndex = (lngNewSelectedIndex * mintNumberColumns) - 1;     // go to last character on the line
                            if (lngNewSelectedIndex <= mlngLength - 1)
                            {
                                mlngSelectedCharIndex = lngNewSelectedIndex;
                                mlngSelectionLength = 0;
                            }
                            else
                            {
                                mlngSelectedCharIndex = mlngLength - 1;
                                mlngSelectionLength = 0;
                            }
                            DisplayFile();
                        }

                        break;
                }
            }
            else  // text viewer
            {
                switch (e.KeyCode)
                {
                    case Keys.Home:
                        if (e.Control)    // ctrl-Home
                        {
                            mlngStartCharIndex = 0;         // scroll view up to beginning of file
                            AdjustArrayLocationInfile();
                            mlngSelectedCharIndex = 0;      // highlight first character of the file                    
                            mlngSelectionLength = 0;
                            DisplayFile();
                        }
                        else             // Home
                        {
                            mlngSelectedCharIndex = mlngContentsStart + txtTextViewer.SelectionStart;
                            mlngSelectionLength = txtTextViewer.SelectionLength;
                            if ((mlngSelectedCharIndex < mlngStartCharIndex) | (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1))
                            {
                                mlngStartCharIndex = mlngSelectedCharIndex / mintNumberColumns;      // get number of rows before the current selected cell
                                mlngStartCharIndex = mlngStartCharIndex * mintNumberColumns;         // scroll to the current line
                                AdjustArrayLocationInfile();
                                DisplayFile();
                            }
                        }

                        break;

                    case Keys.End:
                        if (e.Control)   // ctrl-End
                        {
                            mlngSelectedCharIndex = mlngLength - 1;     // highlight last byte in the file
                            mlngSelectionLength = 0;
                            mlngStartCharIndex = (mlngLength - 1) / (mintNumberRows * mintNumberColumns); // get number of full screen scrolls required to view the end of the file
                            mlngStartCharIndex = mlngStartCharIndex * mintNumberRows * mintNumberColumns; // scroll down by that number of pages
                            AdjustArrayLocationInfile();
                            DisplayFile();
                        }
                        else            // End
                        {
                            mlngSelectedCharIndex = mlngContentsStart + txtTextViewer.SelectionStart;
                            mlngSelectionLength = txtTextViewer.SelectionLength;
                            if ((mlngSelectedCharIndex < mlngStartCharIndex) | (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1))
                            {
                                mlngStartCharIndex = mlngSelectedCharIndex / mintNumberColumns;      // get number of rows before the current selected cell
                                mlngStartCharIndex = mlngStartCharIndex * mintNumberColumns;         // scroll to the current line
                                AdjustArrayLocationInfile();
                                DisplayFile();
                            }
                        }

                        break;

                    default:
                        mlngSelectedCharIndex = mlngContentsStart + txtTextViewer.SelectionStart;
                        mlngSelectionLength = txtTextViewer.SelectionLength;
                        if ((mlngSelectedCharIndex < mlngStartCharIndex) | (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1))
                        {
                            mlngStartCharIndex = mlngSelectedCharIndex / mintNumberColumns;      // get number of rows before the current selected cell
                            mlngStartCharIndex = mlngStartCharIndex * mintNumberColumns;         // scroll to the current line
                            AdjustArrayLocationInfile();
                            DisplayFile();
                        }
                        break;
                }
            }

            switch (e.KeyCode)
            {
                case Keys.F:
                    if (e.Control)  // ctrl-F
                    {
                        mfrmFind = new frmFind();
                        mfrmFind.CallerForm = this;
                        mfrmFind.Show(this);
                        foreach (string strPreviousFindInList in mlstPreviousFindInFile)
                        {
                            mfrmFind.cboFind.Items.Add(strPreviousFindInList);
                        }
                        mfrmFind.cboFind.Text = mstrFindInFile;
                        mfrmFind.cboFind.SelectionLength = mstrFindInFile.Length;
                        mfrmFind.chkWholeWords.Checked = mblnWholeWordsFindInFile;
                        mfrmFind.chkBinary.Checked = mblnBinaryCode;
                    }
                    break;

                case Keys.F3:
                    FindInFile(mstrFindInFile, mblnWholeWordsFindInFile, mblnBinaryCode);
                    break;
            }
        }

        private void btnGoToPosition_Click(object sender, EventArgs e)
        {
            GoToPosition(txtGoToPosition.Text);
        }

        private void txtGoToPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GoToPosition(txtGoToPosition.Text);
        }

        private void GoToPosition(string strCharPosition)
        {
            long lngCharPosition;
            
            if (long.TryParse(strCharPosition,out lngCharPosition))      // if it's a valid number
                if (lngCharPosition >= 0 & lngCharPosition <= mlngLength - 1)     // if it's within the file length
                {
                    mlngSelectedCharIndex = lngCharPosition;
                    mlngSelectionLength = 0;
                    if ((mlngSelectedCharIndex < mlngStartCharIndex) | (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1))
                    {
                        mlngStartCharIndex = lngCharPosition / mintNumberColumns;      // get number of rows before the current selected cell
                        mlngStartCharIndex = mlngStartCharIndex * mintNumberColumns;   // scroll to the current line
                        AdjustArrayLocationInfile();
                    }
                    DisplayFile();
                }
        }

        private void mnuGoToCharPosition_Click(object sender, EventArgs e)
        {
            // show these controls only when they are needed, as it's confusing that the text box has the focus but the keydown goes to the form
            lblGoToPosition.Visible = true;
            txtGoToPosition.Visible = true;
            btnGoToPosition.Visible = true;
            txtGoToPosition.Focus();            
        }

        public void FindInFile(string pstrText, bool pblnWholeWords, bool pblnBinaryCode)
        {
            long lngPosition;

            mstrFindInFile = pstrText;
            if (!mlstPreviousFindInFile.Contains(pstrText))
            {
                mlstPreviousFindInFile.Add(pstrText);
                mfrmFind.cboFind.Items.Add(pstrText);
            }
            mblnWholeWordsFindInFile = pblnWholeWords;
            mblnBinaryCode = pblnBinaryCode;

            if (pblnBinaryCode)
            {
                EncodeBinaryString(ref pstrText);
            }

            if (mnuTextViewer.Checked)
            {
                mlngSelectedCharIndex = mlngContentsStart + txtTextViewer.SelectionStart;
                mlngSelectionLength = txtTextViewer.SelectionLength;
            }

            if (mlngSelectedCharIndex + mlngSelectionLength < mlngLength)
            {
                lngPosition = Find(pstrText, pblnWholeWords, pblnBinaryCode, mlngSelectedCharIndex + mlngSelectionLength);
            } 
            else
            {
                lngPosition = -1;
            }

            if (lngPosition == -1)
            {
                FlashViewer(3);
                if (mlngSelectedCharIndex + mlngSelectionLength > 0)  // first search was not from beginning of file
                {
                    lngPosition = Find(pstrText, pblnWholeWords, pblnBinaryCode, 0);
                }                
            }

            if (lngPosition == -1)
            {
                MessageBox.Show("Text not found", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private long Find(string pstrSearch, bool pblnWholeWords, bool pblnBinaryCode, long plngStartPosition)
        {
            long lngPosition;

            lngPosition = SearchFile (pstrSearch, pblnWholeWords, pblnBinaryCode, plngStartPosition);

            if (lngPosition > -1)
            {
                mlngSelectedCharIndex = lngPosition;
                mlngSelectionLength = pstrSearch.Length;
                if ((mlngSelectedCharIndex < mlngStartCharIndex) | (mlngSelectedCharIndex > mlngStartCharIndex + (mintNumberRows * mintNumberColumns) - 1))
                {
                    mlngStartCharIndex = (int)Math.Floor((double)(lngPosition / mintNumberColumns)); // get number of rows before the current selected cell
                    mlngStartCharIndex = mlngStartCharIndex * mintNumberColumns;  // scroll to the current line
                    AdjustArrayLocationInfile();
                }
                DisplayFile();
            }
            return lngPosition;
        }

        private long SearchFile(string pstrSearch, bool pblnWholeWords, bool pblnBinaryCode, long plngStartPosition)
        {
            int intPosition = -1;
            string strText;
            byte[] arrFileBuffer;
            long lngStart;

            FileStream objFileStream = new FileStream(mstrFileName, FileMode.Open, FileAccess.Read);
            mlngLength = objFileStream.Length;
            
            try
            {
                int intAllocatedLength = 0;

                lngStart = plngStartPosition;

                while (intPosition == -1 & lngStart + intAllocatedLength < mlngLength)
                {
                    if (lngStart + intAllocatedLength != plngStartPosition)   // not the first buffer
                    {
                        lngStart += (intAllocatedLength - 2048);  // go back 2kb in case the search string is on the border between 2 buffers
                    }

                    objFileStream.Seek(lngStart, SeekOrigin.Begin);

                    if (mlngLength - lngStart < 1048576)  // less than a megabyte
                    {
                        intAllocatedLength = (int)(mlngLength - lngStart);
                    }
                    else
                    {
                        intAllocatedLength = 1048576;
                    }
                    arrFileBuffer = new byte[intAllocatedLength];

                    int intCount;
                    int intOffset = 0;

                    while ((intCount = objFileStream.Read(arrFileBuffer, intOffset, intAllocatedLength - intOffset)) > 0)
                        intOffset += intCount;

                    strText = Encoding.Default.GetString(arrFileBuffer);
                    if (pblnWholeWords)
                    {
                        intPosition = InstrWholeWord(ref strText, ref pstrSearch, pblnBinaryCode, 0);
                    }
                    else
                    {
                        if (pblnBinaryCode)
                        {
                            intPosition = strText.IndexOf(pstrSearch,  StringComparison.Ordinal);
                        }
                        else
                        {
                            intPosition = strText.IndexOf(pstrSearch, StringComparison.OrdinalIgnoreCase);
                        }
                    }
                }
                
            }
            finally
            {
                objFileStream.Close();
            }

            if (intPosition == -1)
            {
                return -1;
            }
            else
            {
                return lngStart + intPosition;
            }            
        }

        private int InstrWholeWord(ref string pstrString1, ref string pstrString2, bool pblnBinaryCode, int pintStartPosition)
        {
            int intPosition;
            string strCharBefore;
            string strCharAfter;

            if (pblnBinaryCode)
            {
                intPosition = pstrString1.IndexOf(pstrString2, pintStartPosition, StringComparison.Ordinal);
            }
            else
            {
                intPosition = pstrString1.IndexOf(pstrString2, pintStartPosition, StringComparison.OrdinalIgnoreCase);
            }            
            while (intPosition != -1)
            {
                if (intPosition == 0)
                {
                    strCharBefore = " ";
                }
                else
                {
                    strCharBefore = pstrString1.Substring(intPosition - 1, 1);
                }
                
                if (intPosition + pstrString2.Length == pstrString1.Length)
                {
                    strCharAfter = " ";
                }
                else
                {
                    strCharAfter = pstrString1.Substring(intPosition + pstrString2.Length, 1);
                }

                if (((int)strCharBefore[0] < (int)"A"[0] | (int)strCharBefore[0] > (int)"Z"[0]) &
                    ((int)strCharBefore[0] < (int)"a"[0] | (int)strCharBefore[0] > (int)"z"[0]) &
                    ((int)strCharBefore[0] < (int)"0"[0] | (int)strCharBefore[0] > (int)"9"[0]) &
                    ((int)strCharAfter[0] < (int)"A"[0] | (int)strCharAfter[0] > (int)"Z"[0]) &
                    ((int)strCharAfter[0] < (int)"a"[0] | (int)strCharAfter[0] > (int)"z"[0]) &
                    ((int)strCharAfter[0] < (int)"0"[0] | (int)strCharAfter[0] > (int)"9"[0]))
                {
                    return intPosition;
                }
                if (pblnBinaryCode)
                {
                    intPosition = pstrString1.IndexOf(pstrString2, intPosition + 1, StringComparison.Ordinal);
                }
                else
                {
                    intPosition = pstrString1.IndexOf(pstrString2, intPosition + 1, StringComparison.OrdinalIgnoreCase);
                }                
            }
            return -1;
        }

        private void EncodeBinaryString(ref string pstrText)
        // parameter passed in is a comma separated list of numbers, passed back as a string of binary values
        {
            char[] chrSplitter = { ',' };
            string[] arrList = pstrText.Split(chrSplitter, System.StringSplitOptions.RemoveEmptyEntries);
            pstrText = "";
            foreach(string strByteCode in arrList)
            {
                byte bytResult;
                if (byte.TryParse(strByteCode, out bytResult))
                {
                    byte[] arrByte = { bytResult };
                    pstrText += Encoding.Default.GetString(arrByte);
                }
                else
                {
                    MessageBox.Show("Invalid byte value: " + strByteCode.ToString(), "Search", MessageBoxButtons.OK);
                }
            }
        }

        private void FlashViewer(int pintFlashCount)
        {
            for (int intCount = 1; intCount <= pintFlashCount; intCount++)
            {
                if (mnuTextViewer.Checked)
                {
                    txtTextViewer.BackColor = Color.Gray;
                    Application.DoEvents();
                    Thread.Sleep(100);
                    txtTextViewer.BackColor = Color.White;
                    DisplayFile();
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
                else
                {
                    mobjBitmapGraphics.FillRectangle(Brushes.Gray, 0, 0, mintFormWidth, mintFormHeight);
                    this.Invalidate();
                    Application.DoEvents();
                    Thread.Sleep(100);
                    DisplayFile();
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
            }
        }

        private void mnuTextViewer_Click(object sender, EventArgs e)
        {
            if (mnuTextViewer.Checked)
            {
                txtTextViewer.Visible = true;
                txtTextViewer.Focus();
            }
            else
            {
                txtTextViewer.Visible = false;
                this.Focus();
            }
            DisplayFile();
        }
    }
}








