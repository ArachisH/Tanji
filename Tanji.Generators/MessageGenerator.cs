using System.Text;
using System.Text.Json;
using System.CodeDom.Compiler;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tanji.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class MessageGenerator : IIncrementalGenerator
{
    record MessageFile(string Name, AdditionalText File, bool IsOutgoing);

    private static readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        static MessageFile CreateMessagesDefinition((AdditionalText, AnalyzerConfigOptionsProvider) tuple, CancellationToken cancellationToken = default)
        {
            var (additionalText, optionProvider) = tuple;

            optionProvider.GetOptions(additionalText).TryGetValue("build_metadata.AdditionalFiles.IsOutgoing", out string? isOutgoingValue);
            bool.TryParse(isOutgoingValue, out bool isOutgoing); // TODO: Diagnostic

            return new MessageFile(Path.GetFileNameWithoutExtension(additionalText.Path), additionalText, isOutgoing);
        }

        IncrementalValuesProvider<MessageFile> messageFileProvider = context.AdditionalTextsProvider
            .Where(static (file) => file.Path.EndsWith(".json"))
            .Combine(context.AnalyzerConfigOptionsProvider)
            .Select(CreateMessagesDefinition);

        context.RegisterSourceOutput(messageFileProvider, CreateMessagesSourceOutput);
    }

    private static void CreateMessagesSourceOutput(SourceProductionContext context, MessageFile messageFile)
    {
        // Try deserialize the message file
        var deserializedMessages = JsonSerializer.Deserialize<Message[]>(messageFile.File.GetText().ToString(), _serializerOptions);

        if (deserializedMessages == null)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.FailedToParseMessageDefinitions, null, messageFile.File.Path));
            return;
        }

        string isOutgoingString = messageFile.IsOutgoing.ToString().ToLowerInvariant();
        string className = messageFile.IsOutgoing ? "Outgoing" : "Incoming";

        using var text = new StringWriter();
        using var indentedText = new IndentedTextWriter(text);

        indentedText.WriteLine("using Tanji.Core.Habbo.Canvas;");
        indentedText.WriteLine();

        indentedText.WriteLine("namespace Tanji.Core.Habbo;");
        indentedText.WriteLine();

        indentedText.WriteLine($"public sealed partial class {className} : Identifiers");
        indentedText.Write('{');
        indentedText.Indent++;

        foreach (Message message in deserializedMessages)
        {
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
            indentedText.WriteLine($"init => Register(value, \"{message.Name}\", ref {message.BackingFieldName});");
            indentedText.Indent--;

            indentedText.WriteLine('}');
            if (message.Name.StartsWith("_-"))
            {
                indentedText.WriteLine("*/");
            }
        }

        indentedText.WriteLine();
        indentedText.WriteLine($"public {className}() : base({isOutgoingString}) {{ }}");
        indentedText.WriteLine($"public {className}(IGame game) : base({deserializedMessages.Length}, {isOutgoingString})");
        indentedText.WriteLine('{');
        indentedText.Indent++;

        indentedText.WriteLine("ReadOnlySpan<uint> postShuffleHashes = stackalloc uint[0];");
        indentedText.WriteLine();

        foreach (Message message in deserializedMessages)
        {
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

            indentedText.Write($"{message.BackingFieldName} = ResolveMessage(game, \"{message.Name}\", {message.UnityId}, ");
            if (string.IsNullOrWhiteSpace(message.UnityStructure))
            {
                indentedText.Write("default");
            }
            else indentedText.Write($"\"{message.UnityStructure}\"");

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

        context.AddSource($"{messageFile.Name}.g.cs", SourceText.From(text.ToString(), Encoding.UTF8));
    }
}