namespace Tanji.Core.API;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ModuleAttribute : Attribute
{
    public string Name { get; init; }
    public string Description { get; init; }

    public ModuleAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}