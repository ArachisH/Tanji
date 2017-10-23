using System;
using System.Windows.Forms;

using Eavesdrop;

namespace Tanji
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain
                .UnhandledException += UnhandledException;

            Eavesdropper.Overrides.AddRange(new[] { "*google.com", "*discordapp.com" });

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            MessageBox.Show($"Message: {exception.Message}\r\n\r\n{exception.StackTrace.Trim()}",
                "Tanji ~ Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (e.IsTerminating)
                Eavesdropper.Terminate();
        }
    }
}