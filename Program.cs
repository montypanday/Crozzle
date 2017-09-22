using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crozzle_App
{
    static class Program
    {
        public static bool KeepRunning { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            while (KeepRunning)
            {
                KeepRunning = false;
                Application.Run(new Form1());
            }
        }
    }
}
