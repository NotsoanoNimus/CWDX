using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWDX {
    static class Program {
        public static TransmitForm mainForm;
        public static TextBox txLiveView; public static ProgressBar pbTXProgress;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.mainForm = new TransmitForm();
            Application.Run(Program.mainForm);
        }
    }
}
