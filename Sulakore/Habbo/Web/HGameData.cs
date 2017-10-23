using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sulakore.Habbo.Web
{
    [DebuggerDisplay("Host: {InfoHost}, Port(s): {InfoPort}")]
    public class HGameData
    {
        private readonly Dictionary<string, string> _variables;

        public string Source { get; set; }

        public string InfoHost
        {
            get
            {
                string infoHost = string.Empty;
                _variables.TryGetValue("connection.info.host", out infoHost);
                return infoHost;
            }
        }
        public string InfoPort
        {
            get
            {
                string infoPort = string.Empty;
                _variables.TryGetValue("connection.info.port", out infoPort);
                return infoPort;
            }
        }

        public string this[string key]
        {
            get
            {
                string value = string.Empty;
                _variables.TryGetValue(key, out value);
                return value;
            }
        }

        public HGameData()
        {
            _variables = new Dictionary<string, string>();
        }
        public HGameData(string source)
            : this()
        {
            Update(source);
        }

        public void Update()
        {
            _variables.Clear();

            MatchCollection matches = Regex.Matches(Source,
                "\"(?<variable>.*)\"(:| :|: | : )\"(?<value>.*)\"", RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                string variable = match.Groups["variable"].Value;
                string value = match.Groups["value"].Value;

                if (value.Contains("\\/"))
                    value = value.Replace("\\/", "/");

                _variables[variable] = value;
            }
        }
        public void Update(string source)
        {
            Source = source;
            Update();
        }
    }
}