using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Security.Principal;
using System.Collections.Generic;

using Tanji.Windows;

using Eavesdrop;

using Sulakore.Habbo;

using Flazzy;
using Flazzy.IO;

using Microsoft.Win32;

namespace Tanji
{
    public static class Program
    {
        public static bool HasAdminPrivilages { get; private set; }
        public static Dictionary<string, object> Settings { get; private set; }

        [STAThread]
        private static int Main(string[] args)
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                HasAdminPrivilages = new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
            }

            Settings = LoadSettings();
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            if (args.Length > 0)
            {
                switch (args[0].Substring(args[0].Length - 3))
                {
                    case "dcs": DestroyCertificates(); break;
                    case "ica": InstallCertificateAuthority(); break;
                    case "ems": return (int)EnsureManualWinHttpAutoProxySvcStartup();
                    case "swf": PatchClient(new FileInfo(Path.GetFullPath(args[0]))); break;
                }
                return 0;
            }

            if (EnsureManualWinHttpAutoProxySvcStartup() == ServiceControllerStatus.Running)
            {
                Eavesdropper.Certifier = new CertificateManager("Tanji", "Tanji Certificate Authority");
                Eavesdropper.Overrides.AddRange(((string)Settings["ProxyOverrides"]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                Eavesdropper.Terminate();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainFrm());
            }
            return 1;
        }

        private static void DestroyCertificates()
        {
            var tanjiCertificateManager = new CertificateManager("Tanji", "Tanji Certificate Authority");
            bool destroyedCertificates = tanjiCertificateManager.DestroyCertificates();
            Console.WriteLine("Tanji Generated Certificates Destroyed: " + destroyedCertificates);
        }
        private static void InstallCertificateAuthority()
        {
            var tanjiCertificateManager = new CertificateManager("Tanji", "Tanji Certificate Authority");
            bool installedRootCA = tanjiCertificateManager.CreateTrustedRootCertificate();
            Console.WriteLine("Tanji Certificate Authority Installed: " + installedRootCA);
        }
        private static bool PatchClient(FileInfo clientInfo)
        {
            using (var game = new HGame(clientInfo.FullName))
            {
                game.Disassemble();
                bool disabledHostChecks = game.DisableHostChecks();
                bool injectedKeyShouter = game.InjectKeyShouter(4001);
                bool injectedEndPointShouter = game.InjectEndPointShouter(4000);
                bool injectedEndPoint = game.InjectEndPoint("127.0.0.1", (int)Settings["ConnectionListenPort"]);

                string moddedClientPath = Path.Combine(clientInfo.DirectoryName, "MOD_" + clientInfo.Name);
                using (var fileOutput = File.Open(moddedClientPath, FileMode.Create))
                using (var output = new FlashWriter(fileOutput))
                {
                    game.Assemble(output, CompressionKind.ZLIB);
                }
                MessageBox.Show($"File has been modified/re-assembled successfully at '{moddedClientPath}'.", "Tanji - Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return disabledHostChecks && injectedKeyShouter && injectedEndPointShouter && injectedEndPoint;
            }
        }

        public static int RunTanjiAsAdmin(string argument)
        {
            using (var proc = new Process())
            {
                proc.StartInfo.FileName = Path.GetFullPath("Tanji.exe");
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.StartInfo.Arguments = argument;

                proc.Start();
                proc.WaitForExit();
                return proc.ExitCode;
            }
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

        private static ServiceControllerStatus EnsureManualWinHttpAutoProxySvcStartup()
        {
            using (var controller = new ServiceController("WinHttpAutoProxySvc"))
            {
                ServiceControllerStatus status = controller.Status;
                if (controller.StartType == ServiceStartMode.Disabled)
                {
                    if (HasAdminPrivilages)
                    {
                        RegistryKey winHttpAutoProxySvcKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\WinHttpAutoProxySvc", true);
                        winHttpAutoProxySvcKey.SetValue("Start", ServiceStartMode.Manual, RegistryValueKind.DWord);
                        winHttpAutoProxySvcKey.Flush();

                    }
                    else status = (ServiceControllerStatus)RunTanjiAsAdmin("ems");
                }
                if (status != ServiceControllerStatus.Running && !HasAdminPrivilages)
                {
                    // Tanji was opened again, but having already set the registry value, and not having done a full restart(smh)
                    MessageBox.Show("Changes have been made regarding the 'WinHttpAutoProxy' service, please restart your computer for them to take effect.", "Tanji - Alert! ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                return status;
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