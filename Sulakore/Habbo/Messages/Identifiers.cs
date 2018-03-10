using System.IO;
using System.Collections.Generic;

namespace Sulakore.Habbo.Messages
{
    public abstract class Identifiers
    {
        private readonly string _section;
        private readonly Dictionary<ushort, string> _namesById;
        private readonly Dictionary<string, ushort> _idsByName;
        private readonly Dictionary<string, string> _namesByHash;

        public Identifiers()
        {
            _section = GetType().Name;
            _namesById = new Dictionary<ushort, string>();
            _idsByName = new Dictionary<string, ushort>();
            _namesByHash = new Dictionary<string, string>();
        }

        public ushort this[string name]
        {
            get => _idsByName[name];
            set => _idsByName[name] = value;
        }

        public ushort GetId(string name)
        {
            if (!_idsByName.TryGetValue(name, out ushort id))
            {
                return ushort.MaxValue;
            }
            return id;
        }
        public bool TryGetId(string name, out ushort id)
        {
            return _idsByName.TryGetValue(name, out id);
        }

        public string GetName(ushort id)
        {
            _namesById.TryGetValue(id, out string name);
            return name;
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
            foreach (string name in _idsByName.Keys)
            {
                output.WriteLine($"{name}={_idsByName[name]}");
            }
        }
        public void Load(HGame game, string path)
        {
            _namesById.Clear();
            _idsByName.Clear();
            _namesByHash.Clear();
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

                        if (id != ushort.MaxValue)
                        {
                            _namesById[id] = name;
                        }
                        _idsByName[name] = id;
                        GetType().GetProperty(name)?.SetValue(this, id);
                    }
                }
            }
        }
    }
}