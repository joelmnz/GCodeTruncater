using System;
using System.IO;
using System.Windows.Forms;

namespace TruncateGCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() != DialogResult.Cancel)
            {
                ProcessCodeFile(openFile.FileName);
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() != DialogResult.Cancel)
            {
                File.WriteAllText(saveFile.FileName, tbTruncate.Text);
                lblSts.Text = "File Saved: " + saveFile.FileName;
            }
        }

        private void btnTruncate_Click(object sender, EventArgs e)
        {
            ProcessCode();
        }

        private int DecimalPlaces { get { return Convert.ToInt32(nudDecimalPlaces.Value); } set { nudDecimalPlaces.Value = value; } }

        public void ProcessCodeFile(string fileToLoad)
        {
            tbSource.Text = string.Empty;
            try
            {
                tbSource.Text = File.ReadAllText(fileToLoad);
            }
            catch (Exception ex)
            {
                ShowError(ex);
                return;
            }
            ProcessCode();
        }

        private void ProcessCode()
        {
            tbTruncate.Text = string.Empty;
            lblSts.Text = string.Empty;

            try
            {
                tbTruncate.Text = GCodeTruncateUtil.ProcessCode(tbSource.Text, DecimalPlaces);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tbSource_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
        }

        private void tbSource_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                tbTruncate.Text = String.Empty;
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files == null || files.Length == 0) { return; }
                //foreach (string file in files) Console.WriteLine(file);
                tbSource.Text = System.IO.File.ReadAllText(files[0]);
                ProcessCode();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
    } // Form1
} // ns