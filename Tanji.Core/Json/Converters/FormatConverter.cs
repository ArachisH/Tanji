using System.Text.Json;
using System.Text.Json.Serialization;

using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Json;

public sealed class FormatConverter : JsonConverter<IHFormat>
{
    private const string EVAWIRE_NAME = "EvaWire";
    private const string WEDGIE_IN_NAME = "Wedgie-In";
    private const string WEDGIE_OUT_NAME = "Wedgie-Out";
    private const string EVAWIRE_UNITY_NAME = "EvaWire-Unity";

    public override void Write(Utf8JsonWriter writer, IHFormat value, JsonSerializerOptions options)
    {
        string name = "Unknown";
        if (value == IHFormat.EvaWire) name = EVAWIRE_NAME;
        else if (value == IHFormat.EvaWireUnity) name = EVAWIRE_UNITY_NAME;
        else if (value == IHFormat.WedgieIn) name = WEDGIE_IN_NAME;
        else if (value == IHFormat.WedgieOut) name = WEDGIE_OUT_NAME;
        writer.WriteStringValue(name);
    }
    public override IHFormat? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetString() switch
    {
        EVAWIRE_NAME => IHFormat.EvaWire,
        EVAWIRE_UNITY_NAME => IHFormat.EvaWireUnity,

        WEDGIE_IN_NAME => IHFormat.WedgieIn,
        WEDGIE_OUT_NAME => IHFormat.WedgieOut,

        _ => null,
    };
}