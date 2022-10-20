using System;
using System.Windows.Forms;


namespace CWDX {
    public static class Program {

        public static TransmitForm mainForm;
        
        public static TextBox txLiveView;
        public static ProgressBar pbTXProgress;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() {
            Application.SetHighDpiMode( HighDpiMode.SystemAware );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            Program.mainForm = new TransmitForm();

            Application.Run( Program.mainForm );
        }
    }
}
