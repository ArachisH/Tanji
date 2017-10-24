using System.IO;
using System.Collections.Generic;

namespace Sulakore.Habbo.Messages
{
    public abstract class Identifiers
    {
        private readonly string _section;
        private readonly Dictionary<string, ushort> _ids;
        private readonly Dictionary<string, string> _namesByHash;

        public Identifiers()
        {
            _section = GetType().Name;
            _ids = new Dictionary<string, ushort>();
            _namesByHash = new Dictionary<string, string>();
        }

        public ushort this[string name]
        {
            get => _ids[name];
            set => _ids[name] = value;
        }

        public string GetName(string hash)
        {
            _namesByHash.TryGetValue(hash, out string name);
            return name;
        }

        public void Save(string path)
        {
            using (var output = new StreamWriter(path))
            {
                Save(output);
            }
        }
        public void Save(StreamWriter output)
        {
            output.WriteLine($"[{_section}]");
            foreach (string name in _ids.Keys)
            {
                output.WriteLine($"{name}={_ids[name]}");
            }
        }
        public void Load(HGame game, string path)
        {
            using (var input = new StreamReader(path))
            {
                bool isInSection = false;
                while (!input.EndOfStream)
                {
                    string line = input.ReadLine();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        isInSection = (line == ("[" + _section + "]"));
                    }
                    else if (isInSection)
                    {
                        string[] values = line.Split('=');
                        string name = values[0].Trim();
                        string hash = values[1].Trim();

                        var id = ushort.MaxValue;
                        if (game.Messages.TryGetValue(hash, out List<MessageItem> messages) && messages.Count == 1)
                        {
                            id = messages[0].Id;
                            if (!_namesByHash.ContainsKey(hash))
                            {
                                _namesByHash.Add(hash, name);
                            }
                        }

                        _ids[name] = id;
                        GetType().GetProperty(name)?.SetValue(this, id);
                    }
                }
            }
        }
    }
}