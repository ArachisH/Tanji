using Microsoft.CodeAnalysis;

namespace Tanji.Generators.Identifiers;

internal sealed record IdentifiersFile(string Name, AdditionalText File, bool IsOutgoing);