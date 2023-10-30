using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Tanji.Core.Cryptography;

public class HKeyExchange : IDisposable
{
    private const int BLOCK_SIZE = 256;

    public RSA RSA { get; }
    public BigInteger Modulus { get; }
    public BigInteger Exponent { get; }
    public BigInteger PrivateExponent { get; }

    public BigInteger DHPublic { get; private set; }
    public BigInteger DHPrivate { get; private set; }

    public BigInteger DHPrime { get; private set; }
    public BigInteger DHGenerator { get; private set; }

    public bool CanDecrypt => PrivateExponent != BigInteger.Zero;
    public PKCSPadding Padding { get; set; } = PKCSPadding.MaxByte;

    public HKeyExchange(int rsaKeySize)
    {
        RSA = RSA.Create(rsaKeySize);
        RSAParameters keys = RSA.ExportParameters(true);

        Modulus = new BigInteger(keys.Modulus, true, true);
        Exponent = new BigInteger(keys.Exponent, true, true);
        PrivateExponent = new BigInteger(keys.D, true, true);

        GenerateDHPrimes(256);
        GenerateDHKeys(DHPrime, DHGenerator);
    }
    public HKeyExchange(int exponent, string modulus)
        : this(exponent, modulus, string.Empty)
    { }
    public HKeyExchange(int exponent, string modulus, string privateExponent)
    {
        Exponent = new BigInteger(exponent);
        Modulus = BigInteger.Parse("0" + modulus, NumberStyles.HexNumber);

        var keys = new RSAParameters
        {
            Exponent = Exponent.ToByteArray(isBigEndian: true),
            Modulus = Modulus.ToByteArray(isBigEndian: true)
        };

        if (!string.IsNullOrWhiteSpace(privateExponent))
        {
            PrivateExponent = BigInteger.Parse("0" + privateExponent, NumberStyles.HexNumber);
            keys.D = PrivateExponent.ToByteArray(isBigEndian: true);

            GenerateDHPrimes(256);
            GenerateDHKeys(DHPrime, DHGenerator);
        }

        RSA = RSA.Create(keys);
    }

    public virtual string GetSignedP() => Sign(DHPrime);
    public virtual string GetSignedG() => Sign(DHGenerator);

    public virtual string GetPublicKey()
        => CanDecrypt ? Sign(DHPublic) : Encrypt(DHPublic);
    public virtual byte[] GetSharedKey(string publicKey)
    {
        BigInteger bigInteger = CanDecrypt ? Decrypt(publicKey) : Verify(publicKey);

        byte[] sharedKey = BigInteger.ModPow(bigInteger, DHPrivate, DHPrime)
            .ToByteArray(isBigEndian: true);

        return sharedKey;
    }
    public virtual void VerifyDHPrimes(string p, string g)
    {
        DHPrime = Verify(p);
        if (DHPrime <= 2)
        {
            throw new ArgumentException($"P cannot be less than, or equal to 2.\r\n{DHPrime}");
        }

        DHGenerator = Verify(g);
        if (DHGenerator >= DHPrime)
        {
            throw new ArgumentException($"G cannot be greater than, or equal to P.\r\n{DHPrime}\r\n{DHGenerator}");
        }

        GenerateDHKeys(DHPrime, DHGenerator);
    }

    protected string Sign(BigInteger value)
    {
        return Encrypt(CalculatePrivate, value);
    }
    protected BigInteger Verify(string value)
    {
        return Decrypt(CalculatePublic, value);
    }

    protected string Encrypt(BigInteger value)
    {
        return Encrypt(CalculatePublic, value);
    }
    protected BigInteger Decrypt(string value)
    {
        return Decrypt(CalculatePrivate, value);
    }

    protected void PKCSPad(Span<byte> data, int padLength)
    {
        data[0] = (byte)Padding;
        Span<byte> paddingData = data[1..padLength];

        if (Padding == PKCSPadding.RandomByte)
        {
            for (int i = 0; i < paddingData.Length; i++)
            {
                paddingData[i] = (byte)RandomNumberGenerator.GetInt32(1, 256);
            }
        }
        else paddingData.Fill(byte.MaxValue);

        paddingData[^1] = 0;
    }
    protected void PKCSUnpad(ref Span<byte> data)
    {
        Padding = (PKCSPadding)data[0];

        int dataStart = data.IndexOf((byte)0) + 1;
        data = data[dataStart..];
    }

    protected BigInteger RandomInteger(int bitSize)
    {
        Span<byte> integerData = stackalloc byte[bitSize / 8];
        RandomNumberGenerator.Fill(integerData);

        integerData[^1] &= 0x7f;
        return new BigInteger(integerData);
    }

    protected virtual void GenerateDHPrimes(int bitSize)
    {
        DHPrime = RandomInteger(bitSize);
        DHGenerator = RandomInteger(bitSize);

        if (DHGenerator > DHPrime)
            (DHGenerator, DHPrime) = (DHPrime, DHGenerator);
    }
    protected virtual void GenerateDHKeys(BigInteger p, BigInteger g)
    {
        DHPrivate = RandomInteger(256);
        DHPublic = BigInteger.ModPow(g, DHPrivate, p);
    }

    protected virtual string Encrypt(Func<BigInteger, BigInteger> calculator, BigInteger value)
    {
        Span<byte> valueData = stackalloc byte[BLOCK_SIZE - 1];
        string valueStr = value.ToString();

        // Writes the value string to the end of the buffer
        int written = Encoding.UTF8.GetBytes(valueStr,
            valueData[Index.FromEnd(valueStr.Length)..]);

        // PKCS pad rest of the buffer
        PKCSPad(valueData, valueData.Length - written);

        var paddedInteger = new BigInteger(valueData, isBigEndian: true);
        BigInteger calculatedInteger = calculator(paddedInteger);

        return calculatedInteger.ToString("x").TrimStart('0');
    }
    protected virtual BigInteger Decrypt(Func<BigInteger, BigInteger> calculator, string value)
    {
        var signed = BigInteger.Parse("0" + value, NumberStyles.HexNumber);
        BigInteger paddedInteger = calculator(signed);

        Span<byte> valueData = stackalloc byte[paddedInteger.GetByteCount()];
        paddedInteger.TryWriteBytes(valueData, out int bytesWritten, isBigEndian: true);

        PKCSUnpad(ref valueData);
        return BigInteger.Parse(Encoding.UTF8.GetString(valueData));
    }

    public virtual BigInteger CalculatePublic(BigInteger value)
        => BigInteger.ModPow(value, Exponent, Modulus);
    public virtual BigInteger CalculatePrivate(BigInteger value)
        => BigInteger.ModPow(value, PrivateExponent, Modulus);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            RSA.Dispose();
        }
    }
}