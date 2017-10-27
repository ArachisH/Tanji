using System;
using System.Windows.Forms;

using Tanji.Windows;

using Eavesdrop;

namespace Tanji
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
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