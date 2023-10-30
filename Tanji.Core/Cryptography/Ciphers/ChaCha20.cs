using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tanji.Core.Cryptography.Ciphers;

public sealed class ChaCha20 : IStreamCipher
{
    private readonly uint[] _state;
    private readonly byte[] _block;

    private int _position = -1;

    public ChaCha20(ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, uint blockCount = 0)
    {
        if (key.Length != 32) throw new ArgumentException("The provided key is not 256-bits.");
        if (nonce.Length != 8 && nonce.Length != 12) throw new ArgumentException("The provided nonce can only be 96-bit or 64-bit.");

        // c = constant, k = key, b = blockCount, n = nonce

        // cccccccc cccccccc  cccccccc cccccccc
        _state = new uint[16];
        _state[0] = 0x61707865;
        _state[1] = 0x3320646e;
        _state[2] = 0x79622d32;
        _state[3] = 0x6b206574;

        // kkkkkkkk kkkkkkkk  kkkkkkkk kkkkkkkk
        // kkkkkkkk kkkkkkkk  kkkkkkkk kkkkkkkk
        MemoryMarshal.Cast<byte, uint>(key).CopyTo(_state.AsSpan(4));

        // bbbbbbbb nnnnnnnn  nnnnnnnn nnnnnnnn
        _state[12] = blockCount;

        MemoryMarshal.Cast<byte, uint>(nonce)
            .CopyTo(_state.AsSpan(_state.Length - (nonce.Length / sizeof(uint))));

        _block = new byte[64];

        if (blockCount > 0)
        {
            for (int i = 0; i < blockCount; i++)
            {
                RefreshBlock();
            }
            _position = 0; // Change from -1, as -1 will force the creation of a new block.
        }
    }

    public void Process(Span<byte> data) => Process(data, data);
    public void Process(ReadOnlySpan<byte> source, Span<byte> destination)
    {
        for (int i = 0; i < destination.Length; i++)
        {
            if (_position == -1 || _position == 64)
            {
                RefreshBlock();
                _position = 0;
            }
            destination[i] = (byte)(source[i] ^ _block[_position++]);
        }
    }

    // SAFETY: The temporary stack-allocation is initialized with a copy of _state
    private void RefreshBlock()
    {
        Span<uint> block = stackalloc uint[16];
        _state.CopyTo(block);

        for (int i = 0; i < 10; i++)
        {
            QuarterRound(block, 0, 4, 8, 12);
            QuarterRound(block, 1, 5, 9, 13);
            QuarterRound(block, 2, 6, 10, 14);
            QuarterRound(block, 3, 7, 11, 15);
            QuarterRound(block, 0, 5, 10, 15);
            QuarterRound(block, 1, 6, 11, 12);
            QuarterRound(block, 2, 7, 8, 13);
            QuarterRound(block, 3, 4, 9, 14);
        }

        for (int i = 0; i < 16; i++)
        {
            block[i] = unchecked(block[i] + _state[i]);
        }

        MemoryMarshal.Cast<uint, byte>(block).CopyTo(_block);
        _state[12]++;
    }

    // SAFETY: The arguments MUST BE indices inside the state table.
    private static void QuarterRound(Span<uint> state, int a, int b, int c, int d)
    {
        ref uint state_a = ref Unsafe.Add(ref MemoryMarshal.GetReference(state), a);
        ref uint state_b = ref Unsafe.Add(ref MemoryMarshal.GetReference(state), b);
        ref uint state_c = ref Unsafe.Add(ref MemoryMarshal.GetReference(state), c);
        ref uint state_d = ref Unsafe.Add(ref MemoryMarshal.GetReference(state), d);

        state_a = unchecked(state_a + state_b);
        state_d = BitOperations.RotateLeft(state_d ^ state_a, 16);

        state_c = unchecked(state_c + state_d);
        state_b = BitOperations.RotateLeft(state_b ^ state_c, 12);

        state_a = unchecked(state_a + state_b);
        state_d = BitOperations.RotateLeft(state_d ^ state_a, 8);

        state_c = unchecked(state_c + state_d);
        state_b = BitOperations.RotateLeft(state_b ^ state_c, 7);
    }

    public void Dispose()
    {
        Array.Clear(_state);
        Array.Clear(_block);
    }
}