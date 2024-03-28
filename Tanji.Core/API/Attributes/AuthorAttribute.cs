using Tanji.Core.Canvas;

namespace Tanji.Core.API;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AuthorAttribute : Attribute
{
    public string Name { get; init; }
    public HHotel Hotel { get; init; }
    public string? HabboName { get; init; }
    public string? ResourceUrl { get; init; }
    public string? ResourceName { get; init; }

    public AuthorAttribute(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}