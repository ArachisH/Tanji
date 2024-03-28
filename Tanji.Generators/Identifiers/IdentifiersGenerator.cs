using System.Text;
using System.Text.Json;
using System.CodeDom.Compiler;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tanji.Core.Generators.Identifiers;

[Generator(LanguageNames.CSharp)]
public sealed class IdentifiersGenerator : IIncrementalGenerator
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        static IdentifiersFile CreateMessagesDefinition((AdditionalText text, AnalyzerConfigOptionsProvider options) tuple, CancellationToken cancellationToken = default)
        {
            tuple.options.GetOptions(tuple.text).TryGetValue("build_metadata.AdditionalFiles.IsOutgoing", out string? isOutgoingValue);
            bool.TryParse(isOutgoingValue, out bool isOutgoing); // TODO: Diagnostic

            return new IdentifiersFile(Path.GetFileNameWithoutExtension(tuple.text.Path), tuple.text, isOutgoing);
        }

        IncrementalValuesProvider<IdentifiersFile> identifiersFileProvider = context.AdditionalTextsProvider
            .Where(static (file) => file.Path.EndsWith(".json"))
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select(CreateMessagesDefinition);

        context.RegisterSourceOutput(identifiersFileProvider, CreateMessagesSourceOutput);
    }

    private static bool IsMessageInvalid(MessageItem message)
    {
        return string.IsNullOrWhiteSpace(message.Name) || message.Name.StartsWith("_-");
    }
    private static string Generate(MessageItem[] messages, bool isOutgoing)
    {
        string isOutgoingArgumentValue = isOutgoing ? "true" : "false";
        string sourceFileName = isOutgoing ? "Outgoing" : "Incoming";

        using var text = new StringWriter();
        using var indentedText = new IndentedTextWriter(text);

        indentedText.Write($$"""
            using System.Text.Json.Serialization;

            using Tanji.Core.Canvas;

            namespace Tanji.Core.Net.Messages;

            public sealed partial class {{sourceFileName}} : Identifiers
            {
            """);

        string backingFieldVariableName;
        foreach (MessageItem message in messages)
        {
            if (IsMessageInvalid(message)) continue;

            indentedText.WriteLine();
            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("/*");
            }

            backingFieldVariableName = $"_{char.ToLower(message.Name[0])}{message.Name.Substring(1)}";
            indentedText.WriteLine($$"""
                    private readonly HMessage {{backingFieldVariableName}};
                    public HMessage {{message.Name}}
                    {
                        get => {{backingFieldVariableName}};
                        init => Register(value, ref {{backingFieldVariableName}});
                    }
                """);

            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("*/");
            }
        }

        indentedText.WriteLine($$"""

                public {{sourceFileName}}() : base({{isOutgoingArgumentValue}}) { }
                public {{sourceFileName}}(IGame game) : base({{isOutgoingArgumentValue}})
                {
                    ReadOnlySpan<uint> postShuffleHashes = stackalloc uint[0];
            """);

        indentedText.Indent += 2;
        foreach (MessageItem message in messages)
        {
            if (IsMessageInvalid(message)) continue;

            indentedText.WriteLine();
            if (message.Name.StartsWith("_-"))
            {
                indentedText.Write("""
                    /*
                    *
                    """);
            }

            if (message.PostShuffleHashes.Length > 0)
            {
                indentedText.Write($"postShuffleHashes = stackalloc uint[{message.PostShuffleHashes.Length}] {{ ");
                for (int i = 0; i < message.PostShuffleHashes.Length; i++)
                {
                    indentedText.Write(message.PostShuffleHashes[i]);
                    if (i + 1 < message.PostShuffleHashes.Length)
                    {
                        indentedText.Write(", ");
                    }
                }
                indentedText.WriteLine(" };");
            }

            indentedText.Write($"{message.Name} = ResolveMessage(game, \"{message.Name}\", {message.UnityId}");
            if (message.PostShuffleHashes.Length > 0)
            {
                indentedText.Write(", postShuffleHashes");
            }
            indentedText.WriteLine(");");

            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("*/");
            }
        }

        indentedText.Indent--;
        indentedText.WriteLine('}');

        indentedText.Indent--;
        indentedText.Write('}');

        indentedText.Flush();
        return text.ToString();
    }
    private static void CreateMessagesSourceOutput(SourceProductionContext context, IdentifiersFile identifiersFile)
    {
        var messages = JsonSerializer.Deserialize<MessageItem[]?>(identifiersFile.File.GetText()!.ToString(), SerializerOptions);
        if (messages == null)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.FailedToParseMessageDefinitions, null, identifiersFile.File.Path));
            return;
        }

        string generatedSource = Generate(messages, identifiersFile.IsOutgoing);
        context.AddSource($"{identifiersFile.Name}.g.cs", SourceText.From(generatedSource, Encoding.UTF8));
    }
}