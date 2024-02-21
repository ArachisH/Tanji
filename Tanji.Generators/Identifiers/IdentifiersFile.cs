using Microsoft.CodeAnalysis;

namespace Tanji.Core.Generators.Identifiers;

internal sealed record IdentifiersFile(string Name, AdditionalText File, bool IsOutgoing);