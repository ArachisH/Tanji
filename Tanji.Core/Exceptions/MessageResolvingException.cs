namespace Tanji.Core;

public sealed class MessageResolvingException : Exception
{
    public string Name { get; }
    public string Revision { get; }

    public MessageResolvingException(string name, string revision)
        : base($"Failed to resolve message \"{name}\" for revision {revision}.")
    {
        Name = name;
        Revision = revision;
    }
}