using System;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Tanji.Utilities
{
    public class TConfiguration : NameValueCollection
    {
        public string[] UnityInterceptionTriggers { get; }
        public string[] FlashInterceptionTriggers { get; }

        public int GameListenPort { get; set; }
        public int ProxyListenPort { get; set; }

        public int ModulesListenPort { get; set; }
        public bool IsQueuingModulePackets { get; set; }

        public Color UIScheme { get; set; }
        public bool IsCheckingForUpdates { get; set; }
        public bool IsModifyingRetroClientLoaders { get; set; }

        public string[] ProxyOverrides { get; }

        public TConfiguration()
            : base(ConfigurationManager.AppSettings)
        {
            UnityInterceptionTriggers = Split(this[nameof(UnityInterceptionTriggers)]);
            FlashInterceptionTriggers = Split(this[nameof(FlashInterceptionTriggers)]);

            GameListenPort = int.Parse(this[nameof(GameListenPort)]);
            ProxyListenPort = int.Parse(this[nameof(ProxyListenPort)]);

            ModulesListenPort = int.Parse(this[nameof(ModulesListenPort)]);
            IsQueuingModulePackets = Convert.ToBoolean(this[nameof(IsQueuingModulePackets)]);

            UIScheme = ColorTranslator.FromHtml(this[nameof(UIScheme)]);
            IsCheckingForUpdates = Convert.ToBoolean(this[nameof(IsCheckingForUpdates)]);
            IsModifyingRetroClientLoaders = Convert.ToBoolean(this[nameof(IsQueuingModulePackets)]);

            ProxyOverrides = Split(this[nameof(ProxyOverrides)]);
        }

        private static string[] Split(string value)
        {
            return value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
        private static IDictionary<string, string> PopulateStructureOverrides(string value)
        {
            var structureOverrides = new Dictionary<string, string>();
            string[] sections = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string section in sections)
            {
                string[] pieces = section.Split('_');
                structureOverrides.Add(pieces[0], pieces[1]);
            }
            return structureOverrides;
        }
    }
}