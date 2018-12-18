namespace NavitaireTE
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.UnhandledExceptionHandler);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            EventLog.WriteEntry(string.Format("Unhandled exception caught in {0}. Application is {1}terminating.", AppDomain.CurrentDomain.FriendlyName, args.IsTerminating ? "" : "not "), (args.ExceptionObject as Exception).ToString(), EventLogEntryType.Error);
        }
    }
}

