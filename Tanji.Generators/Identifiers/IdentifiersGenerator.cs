using System.Text;
using System.Text.Json;
using System.CodeDom.Compiler;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tanji.Generators.Identifiers;

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

        indentedText.WriteLine("using Tanji.Core.Habbo.Canvas;");
        indentedText.WriteLine();

        indentedText.WriteLine("namespace Tanji.Core.Habbo;");
        indentedText.WriteLine();

        indentedText.WriteLine($"public sealed partial class {sourceFileName} : Identifiers");
        indentedText.Write('{');
        indentedText.Indent++;

        foreach (MessageItem message in messages)
        {
            if (IsMessageInvalid(message)) continue;

            indentedText.WriteLine();
            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("/*");
            }

            message.BackingFieldName = $"_{char.ToLower(message.Name[0]) + message.Name.Substring(1)}";
            indentedText.WriteLine($"private HMessage {message.BackingFieldName};");
            indentedText.WriteLine($"public HMessage {message.Name}");
            indentedText.WriteLine('{');

            indentedText.Indent++;
            indentedText.WriteLine($"get => {message.BackingFieldName};");
            indentedText.WriteLine($"init => Register(value, ref {message.BackingFieldName});");
            indentedText.Indent--;

            indentedText.WriteLine('}');
            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("*/");
            }
        }

        indentedText.WriteLine();
        indentedText.WriteLine($"public {sourceFileName}() : base({isOutgoingArgumentValue}) {{ }}");
        indentedText.WriteLine($"public {sourceFileName}(IGame game) : base({isOutgoingArgumentValue})");
        indentedText.WriteLine('{');
        indentedText.Indent++;

        indentedText.WriteLine("ReadOnlySpan<uint> postShuffleHashes = stackalloc uint[0];");
        indentedText.WriteLine();

        foreach (MessageItem message in messages)
        {
            if (IsMessageInvalid(message)) continue;

            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("/*");
                indentedText.Write('*');
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
            indentedText.WriteLine();

            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("*/");
            }
        }

        indentedText.Indent--;
        indentedText.WriteLine('}');

        indentedText.Indent--;
        indentedText.WriteLine('}');

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