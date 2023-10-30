using Microsoft.CodeAnalysis;

namespace Tanji.Generators;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor FailedToParseMessageDefinitions = new(
        id: "SLKR0001",
        title: "Failed to parse message definitions",
        messageFormat: $"Cannot deserialize message definitions from input file \"{{0}}\"",
        category: typeof(MessageGenerator).FullName,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Cannot parse message definitions from the specified input file.");
}