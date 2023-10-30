namespace Tanji.Core.Cryptography.Ciphers;

public interface IStreamCipher : IDisposable
{
    void Process(Span<byte> data);
    void Process(ReadOnlySpan<byte> source, Span<byte> destination);
}