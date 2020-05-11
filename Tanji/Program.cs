using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Windows;

using Sulakore.Habbo;

using Flazzy;
using Flazzy.IO;

using Eavesdrop;

namespace Tanji
{
    public static class Program
    {
        public static Dictionary<string, object> Settings { get; private set; }

        [STAThread]
        private static void Main(string[] args)
        {
            Settings = LoadSettings();
            if (args.Length > 0 && args[0].EndsWith(".swf"))
            {
                var clientInfo = new FileInfo(Path.GetFullPath(args[0]));
                using (var game = new HGame(clientInfo.FullName))
                {
                    game.Disassemble();
                    game.DisableHostChecks();
                    game.InjectKeyShouter(4001);
                    game.InjectEndPointShouter(4000);
                    game.InjectEndPoint("127.0.0.1", (int)Settings["ConnectionListenPort"]);

                    string moddedClientPath = Path.Combine(clientInfo.DirectoryName, "MOD_" + clientInfo.Name);
                    using (var fileOutput = File.Open(moddedClientPath, FileMode.Create))
                    using (var output = new FlashWriter(fileOutput))
                    {
                        game.Assemble(output, CompressionKind.ZLIB);
                    }
                    MessageBox.Show($"File has been modified/re-assembled successfully at '{moddedClientPath}'.", "Tanji - Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                return;
            }

            Eavesdropper.Certifier = new CertificateManager("Tanji", "Tanji Certificate Authority");
            Eavesdropper.Overrides.AddRange(((string)Settings["ProxyOverrides"]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            Eavesdropper.Terminate();

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }
        private static Dictionary<string, object> LoadSettings()
        {
            var settings = new Dictionary<string, object>();
            foreach (string line in File.ReadLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Settings.ini")))
            {
                int splitIndex = line.IndexOf('=');
                string name = line.Substring(0, splitIndex);
                string value = line.Substring(splitIndex + 1, line.Length - (name.Length + 1));

                object oValue = null;
                if (int.TryParse(value, out int iValue))
                {
                    oValue = iValue;
                }
                else if (bool.TryParse(value, out bool bValue))
                {
                    oValue = bValue;
                }
                else if (value.StartsWith("#") && value.Length == 7)
                {
                    oValue = ColorTranslator.FromHtml(value.ToUpper());
                }
                else
                {
                    oValue = value;
                }
                settings.Add(name, oValue);
            }
            return settings;
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