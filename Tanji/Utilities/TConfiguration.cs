using System;
using System.Linq;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Tanji.Utilities
{
    public class TConfiguration : NameValueCollection
    {
        public Color UIScheme { get; set; }

        public int GameListenPort { get; set; }
        public int ProxyListenPort { get; set; }

        public bool IsCheckingForUpdates { get; set; }
        public bool IsQueuingModulePackets { get; set; }

        public IList<string> ProxyOverrides { get; }
        public IList<string> CacheBlacklist { get; }
        public IList<string> UserInterceptionTriggers { get; }
        public IList<string> WASMInterceptionTriggers { get; }
        public IList<string> FlashInterceptionTriggers { get; }

        public IDictionary<string, string> InStructureOverrides { get; }
        public IDictionary<string, string> OutStructureOverrides { get; }

        public TConfiguration()
            : base(ConfigurationManager.AppSettings)
        {
            UIScheme = ColorTranslator.FromHtml(this[nameof(UIScheme)]);

            GameListenPort = int.Parse(this[nameof(GameListenPort)]);
            ProxyListenPort = int.Parse(this[nameof(ProxyListenPort)]);

            IsCheckingForUpdates = Convert.ToBoolean(this[nameof(IsCheckingForUpdates)]);
            IsQueuingModulePackets = Convert.ToBoolean(this[nameof(IsQueuingModulePackets)]);

            ProxyOverrides = this[nameof(ProxyOverrides)].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            CacheBlacklist = this[nameof(CacheBlacklist)].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            UserInterceptionTriggers = this[nameof(UserInterceptionTriggers)].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            WASMInterceptionTriggers = this[nameof(WASMInterceptionTriggers)].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            FlashInterceptionTriggers = this[nameof(FlashInterceptionTriggers)].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            InStructureOverrides = PopulateStructureOverrides(this[nameof(InStructureOverrides)]);
            OutStructureOverrides = PopulateStructureOverrides(this[nameof(OutStructureOverrides)]);
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