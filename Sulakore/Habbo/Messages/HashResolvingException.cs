using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sulakore.Habbo.Messages
{
    public sealed class HashResolvingException : Exception
    {
        public string Revision { get; }
        public ReadOnlyDictionary<string, string[]> Unresolved { get; }

        public HashResolvingException(string revision, IList<string> hashes)
            : base($"Failed to resolve '{hashes.Count}' hash value(s) from revision '{revision}'.")
        {
            Unresolved = new ReadOnlyDictionary<string, string[]>(
                hashes.ToDictionary(u => u, u => new string[0]));
        }
        public HashResolvingException(string revision, IDictionary<string, IList<string>> unresolved)
            : base($"Failed to resolve '{unresolved.Count}' hash value(s) from revision '{revision}'.")
        {
            Unresolved = new ReadOnlyDictionary<string, string[]>(
                unresolved.ToDictionary(u => u.Key, u => u.Value.ToArray()));
        }
    }
}