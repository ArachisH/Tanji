using System;
using System.Windows.Forms;
using System.Security.Principal;

using Tanji.Windows;

using Eavesdrop;

namespace Tanji
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            if (IsAdministrator())
            {
                string title = "Tanji - Alert";
                string message = "It is NOT recommended that you run this application with administrative privileges, please restart me without doing so!\r\n\r\nDon't worry, you will not lose any functionality.";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Eavesdropper.Overrides.AddRange(new[]
            {
                "*google*",
                "*discordapp.com",
                "*gstatic.com",
                "*imgur.com",
                "*github.com",
                "*googleapis.com",
                "*facebook.com",
                "*cloudfront.net",
                "*gvt1.com",
                "*jquery.com",
                "*akamai.net",
                "*ultra-rv.com"
            });

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }

        private static bool IsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;

            string title = ("Tanji - " + (e.IsTerminating ? "Critical Error!" : "Error!"));
            string message = $"Message: {exception.Message}\r\n\r\n{exception.StackTrace.Trim()}";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (e.IsTerminating)
            {
                Eavesdropper.Terminate();
            }
        }
    }
}