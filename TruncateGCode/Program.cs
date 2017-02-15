using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TruncateGCode
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 mainForm = new Form1();
            mainForm.Show();
            if (args.Length>0)
            {
                // allow file input, assume first arg is file name
                mainForm.ProcessCodeFile(args[0]);
            }

            Application.Run(mainForm);
        }

        internal static string VersionText()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
