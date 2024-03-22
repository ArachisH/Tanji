using System.Text;
using System.Buffers;
using System.Text.Unicode;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

using Flazzy.IO;
using Flazzy.ABC;
using Flazzy.ABC.AVM2.Instructions;

namespace Tanji.Core.Habbo.Canvas.Flash;

public class FlashMessageHasher : IBufferWriter<byte>
{
    private readonly ArrayBufferWriter<byte> _buffer;

    private bool _isReadingMethodBodies = true;

    public FlashMessageHasher(int initialCapacity)
    {
        _buffer = new ArrayBufferWriter<byte>(initialCapacity);
    }

    public void Write(ASTrait trait)
    {
        Write(trait.Id);
        Write(trait.QName);
        Write(trait.IsStatic);
        Write((byte)trait.Kind);
        Write((byte)trait.Attributes);
        switch (trait.Kind)
        {
            case TraitKind.Slot:
            case TraitKind.Constant:
            {
                Write(trait.Type);
                if (trait.Value != null)
                {
                    Write(trait.ValueKind, trait.Value);
                }
                break;
            }
            case TraitKind.Method:
            case TraitKind.Getter:
            case TraitKind.Setter:
            {
                Write(trait.Method);
                break;
            }
        }
    }
    public void Write(ASMethod method)
    {
        Write((int)method.Flags);
        Write(method.IsAnonymous);

        Write(method.IsConstructor);
        if (!method.IsConstructor)
        {
            Write(method.ReturnType);
        }

        Write(method.Parameters.Count);
        foreach (ASParameter parameter in method.Parameters)
        {
            Write(parameter.Type);
            if (parameter.IsOptional)
            {
                Write(parameter.IsOptional);
                Write((byte)parameter.ValueKind);
                Write(parameter.ValueKind, parameter.Value);
            }
        }

        if (_isReadingMethodBodies)
        {
            // Utilizing ASCode in this scenario is very costly, simply read the instructions manually.
            Span<byte> counts = stackalloc byte[8];
            var codeReader = new SpanFlashReader(method.Body.Code);
            while (codeReader.IsDataAvailable)
            {
                var instruction = ASInstruction.Create(method.ABC, ref codeReader);
                switch (instruction.OP)
                {
                    case OPCode.PushString: counts[0]++; break;
                    case OPCode.GetLex: counts[1]++; break;
                    case OPCode.GetProperty: counts[2]++; break;
                    case OPCode.CallProperty: counts[3]++; break;
                    case OPCode.PushInt: counts[4]++; break;
                    case OPCode.SetProperty: counts[5]++; break;
                    case OPCode.Swap: counts[6]++; break;
                }
            }
            _buffer.Write(counts);
        }
    }
    public void Write(ASMultiname multiname)
    {
        if (multiname?.Namespace != null)
        {
            Write((byte)multiname.Namespace.Kind);
        }
        if (multiname?.Kind == MultinameKind.TypeName)
        {
            Write(multiname.QName);
            Write(multiname.TypeIndices.Count);
            foreach (ASMultiname type in multiname.GetTypes())
            {
                Write(type);
            }
        }
        else if (multiname == null) Write('*');
    }
    public void Write(ConstantKind kind, object value)
    {
        Write((byte)kind);
        switch (kind)
        {
            case ConstantKind.Double:
            Write((double)value);
            break;
            case ConstantKind.Integer:
            Write((int)value);
            break;
            case ConstantKind.UInteger:
            Write((uint)value);
            break;
            case ConstantKind.String:
            Write((string)value);
            break;
            case ConstantKind.Null:
            Write("null");
            break;
            case ConstantKind.True:
            Write(true);
            break;
            case ConstantKind.False:
            Write(false);
            break;
        }
    }
    public void Write(ASContainer container, bool includeTraits)
    {
        Write(container.IsStatic);
        if (includeTraits)
        {
            Write(container.Traits.Count);
            container.Traits.ForEach(t => Write(t));
        }
    }

    public void Write(int value)
    {
        BinaryPrimitives.WriteInt32LittleEndian(_buffer.GetSpan(sizeof(int)), value);
        _buffer.Advance(sizeof(int));
    }
    public void Write(bool value)
    {
        Write((byte)(value ? 1 : 0));
    }
    public void Write(byte value)
    {
        _buffer.GetSpan(1)[0] = value;
        _buffer.Advance(1);
    }
    public void Write(char value)
    {
        Write((byte)value);
    }
    public void Write(ulong value)
    {
        BinaryPrimitives.WriteUInt64LittleEndian(_buffer.GetSpan(sizeof(ulong)), value);
        _buffer.Advance(sizeof(ulong));
    }
    public void Write(double value)
    {
        BinaryPrimitives.WriteDoubleLittleEndian(_buffer.GetSpan(sizeof(double)), value);
        _buffer.Advance(sizeof(double));
    }
    public void Write(ReadOnlySpan<char> value)
    {
        if (value.Length > 0)
        {
            int encodedSize = Encoding.UTF8.GetByteCount(value);
            Utf8.FromUtf16(value, _buffer.GetSpan(encodedSize), out _, out int bytesWritten);
            _buffer.Advance(bytesWritten);
        }
    }

    public void Clear()
    {
        _buffer.Clear();
    }
    public uint ToUInt32()
    {
        using var md5 = MD5.Create();
        Span<byte> hashed = stackalloc byte[16];
        md5.TryComputeHash(_buffer.WrittenSpan, hashed, out int bytesWritten);

        int multiplier = sizeof(uint);
        var result = MemoryMarshal.Read<uint>(hashed);
        for (int i = multiplier * 1; i < hashed.Length / multiplier; i++)
        {
            result ^= MemoryMarshal.Read<uint>(hashed[(i * multiplier)..]);
        }
        return result;
    }

    #region IBufferWriter<byte> Implementation
    public void Advance(int count)
    {
        _buffer.Advance(count);
    }
    public Span<byte> GetSpan(int sizeHint = 0)
    {
        return _buffer.GetSpan(sizeHint);
    }
    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        return _buffer.GetMemory(sizeHint);
    }
    #endregion

    public static uint Generate(FlashMessageHasher hasher, FlashMessage flashMessage, int collisions = 0)
    {
        hasher.Clear();

        hasher.Write(flashMessage.IsOutgoing);
        if (collisions > 0)
        {
            hasher.Write(int.MaxValue - collisions);
        }

        hasher.Write(flashMessage.MessageClass.Instance, true);
        hasher.Write(flashMessage.MessageClass.Instance.Constructor);
        if (flashMessage.ParserClass != null)
        {
            hasher.Write(flashMessage.ParserClass.Constructor);
            hasher.Write(flashMessage.ParserClass.Instance, true);
        }

        hasher.Write(flashMessage.References.Count);
        foreach (FlashMessageReference reference in flashMessage.References)
        {
            hasher.Write(reference.ArgumentsUsed);
            hasher.Write(reference.OrderInMethod);
            if (reference.Method != null)
            {
                hasher.Write(reference.Method);
                if (reference.Method.Container != null)
                {
                    hasher._isReadingMethodBodies = false;
                    hasher.Write(reference.Method.Container, true);
                    hasher._isReadingMethodBodies = true;
                }
            }
            if (reference.Callback != null)
            {
                hasher.Write(reference.Callback);
            }
        }

        return hasher.ToUInt32();
    }
}