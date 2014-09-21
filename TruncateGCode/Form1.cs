using System;
using System.IO;
using System.Windows.Forms;

namespace TruncateGCode
{
    public partial class Form1 : Form
    {
        private bool end = false;
        public Form1()
        {
            InitializeComponent();
        }

        public string truncateGCode(string code)
        {
            try
            {

                if ((code.Substring(0, 1) == "(") || (code.Substring(0, 1) == ";") || (code.Substring(0, 1) == ":") ||
                    (code.Substring(0, 1) == "'"))
                {
                    return code;
                }

                string[] codes = code.Split(' ');
                string finCode = "";

                foreach (string gCode in codes)
                {
                    if (gCode.IndexOf(".") > -1)
                    {
                        double val = 0;
                        if (double.TryParse(gCode.Substring(0, 1), out val))
                        {
                        }
                        else
                        {
                            if (Convert.ToDouble(gCode.Substring(1, gCode.Length - 1)) != 0)
                            {
                                string num = Convert.ToDouble(gCode.Substring(1, gCode.Length - 1)).ToString("#.###");
                                if (num == "")
                                {
                                    num = "0";
                                }
                                finCode += gCode.Substring(0, 1) + num;

                            }
                            else
                            {
                                finCode += gCode.Substring(0, 1) + "0";
                            }
                        }
                    }
                    else
                    {
                        finCode += gCode;
                    }
                }

                return finCode;
            }
            catch (Exception ex)
            {
               
                    MessageBox.Show(ex.Message, "Unable to Process GCode",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                 
                end = true;
                return "(ERROR)";
            }

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() != DialogResult.Cancel)
            {
                tbSource.Text = "";
                tbTruncate.Text = "";
                tbSource.Text = File.ReadAllText(openFile.FileName);
                ProcessCode();
                lblSts.Text = "";
            }
        }



        void ProcessCode()
        {
            string[] lines = tbSource.Text.Split('\n');
            foreach (string line in lines)
            {
                line.Replace("\r","");
                tbTruncate.Text += truncateGCode(line) + Environment.NewLine;
                if (end)
                {
                    end = false;
                    break;
                }
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
            tbTruncate.Text = "";
            lblSts.Text = "";
            ProcessCode();
        }
    }
}
