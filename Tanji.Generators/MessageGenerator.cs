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
        var deserializedMessages = JsonSerializer.Deserialize<Dictionary<string, Message[]>>(messageFile.File.GetText().ToString(), _serializerOptions);
        if (deserializedMessages == null)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.FailedToParseMessageDefinitions, null, messageFile.File.Path));
            return;
        }

        string isOutgoingString = messageFile.IsOutgoing.ToString().ToLowerInvariant();
        foreach (var values in deserializedMessages)
        {
            string className = values.Key;
            var messages = values.Value;

            using var text = new StringWriter();
            using var indentedText = new IndentedTextWriter(text);

            indentedText.WriteLine("using Tanji.Core.Habbo.Canvas;");
            indentedText.WriteLine();

            indentedText.WriteLine("namespace Tanji.Core.Habbo;");
            indentedText.WriteLine();

            indentedText.WriteLine($"public sealed partial class {className} : Identifiers");
            indentedText.WriteLine('{');
            indentedText.Indent++;

            foreach (Message message in messages)
            {
                indentedText.WriteLine();
                message.BackingFieldName = $"_{char.ToLower(message.Name[0]) + message.Name.Substring(1)}";
                indentedText.WriteLine($"private HMessage {message.BackingFieldName};");
                indentedText.WriteLine($"public HMessage {message.Name}");
                indentedText.WriteLine('{');

                indentedText.Indent++;
                indentedText.WriteLine($"get => {message.BackingFieldName};");
                indentedText.WriteLine($"init => Register(value, nameof({message.Name}), ref {message.BackingFieldName});");
                indentedText.Indent--;

                indentedText.WriteLine('}');
            }

            indentedText.WriteLine();
            indentedText.WriteLine($"public {className}() : base({isOutgoingString}) {{ }}");
            indentedText.WriteLine($"public {className}(IGame game) : base({messages.Length}, {isOutgoingString})");
            indentedText.WriteLine('{');
            indentedText.Indent++;

            foreach (Message message in messages)
            {
                indentedText.Write($"{message.BackingFieldName} = ResolveMessage(game, nameof({message.Name}), {message.UnityId}, ");
                if (string.IsNullOrWhiteSpace(message.UnityStructure))
                {
                    indentedText.Write("null");
                }
                else indentedText.Write("\"" + message.UnityStructure + "\"");

                foreach (uint postShuffleHash in message.PostShuffleHashes)
                {
                    indentedText.Write(", ");
                    indentedText.Write(postShuffleHash);
                }
                indentedText.WriteLine(");");
            }

            indentedText.Indent--;
            indentedText.WriteLine('}');

            indentedText.Indent--;
            indentedText.WriteLine('}');

            indentedText.Flush();

            context.AddSource($"{messageFile.Name}.g.cs", SourceText.From(text.ToString(), Encoding.UTF8));
        }
    }
}