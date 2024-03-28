using Tanji.Core.Generators.Identifiers;

using Microsoft.CodeAnalysis;

namespace Tanji.Core.Generators;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor FailedToParseMessageDefinitions = new(
        id: "TNJ0001",
        title: "Failed to parse message definitions",
        messageFormat: $"Cannot deserialize message definitions from input file \"{{0}}\"",
        category: typeof(IdentifiersGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Cannot parse message definitions from the specified input file.");
}