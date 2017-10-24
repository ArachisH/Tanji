using System;
using System.Text;
using System.Numerics;
using System.Globalization;
using System.Security.Cryptography;

namespace Sulakore.Crypto
{
    public class HKeyExchange : IDisposable
    {
        private const int BLOCK_SIZE = 128;
        private readonly Random _numberGenerator;

        public BigInteger Modulus { get; }
        public BigInteger Exponent { get; }
        public BigInteger PrivateExponent { get; }
        public RSACryptoServiceProvider RSA { get; }

        public BigInteger DHPublic { get; private set; }
        public BigInteger DHPrivate { get; private set; }

        public BigInteger DHPrime { get; private set; }
        public BigInteger DHGenerator { get; private set; }

        public bool CanDecrypt => (PrivateExponent != BigInteger.Zero);
        public PKCSPadding Padding { get; set; } = PKCSPadding.MaxByte;

        private HKeyExchange()
        {
            _numberGenerator = new Random();
        }
        public HKeyExchange(int rsaKeySize)
            : this()
        {
            RSA = new RSACryptoServiceProvider(rsaKeySize);
            RSAParameters keys = RSA.ExportParameters(true);

            Modulus = new BigInteger(ReverseNull(keys.Modulus));
            Exponent = new BigInteger(ReverseNull(keys.Exponent));
            PrivateExponent = new BigInteger(ReverseNull(keys.D));

            GenerateDHPrimes(256);
            GenerateDHKeys(DHPrime, DHGenerator);
        }
        public HKeyExchange(int exponent, string modulus)
            : this(exponent, modulus, string.Empty)
        { }
        public HKeyExchange(int exponent, string modulus, string privateExponent)
            : this()
        {
            var keys = new RSAParameters();

            Exponent = new BigInteger(exponent);
            keys.Exponent = Exponent.ToByteArray();
            Array.Reverse(keys.Exponent);

            Modulus = BigInteger.Parse("0" + modulus, NumberStyles.HexNumber);
            keys.Modulus = Modulus.ToByteArray();
            Array.Reverse(keys.Modulus);

            if (!string.IsNullOrWhiteSpace(privateExponent))
            {
                PrivateExponent = BigInteger.Parse("0" + privateExponent, NumberStyles.HexNumber);
                keys.D = PrivateExponent.ToByteArray();
                Array.Reverse(keys.D);

                GenerateDHPrimes(256);
                GenerateDHKeys(DHPrime, DHGenerator);
            }

            RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(keys);
        }

        public virtual string GetSignedP()
        {
            return Sign(DHPrime);
        }
        public virtual string GetSignedG()
        {
            return Sign(DHGenerator);
        }
        public virtual string GetPublicKey()
        {
            return (CanDecrypt ?
                Sign(DHPublic) : Encrypt(DHPublic));
        }
        public virtual byte[] GetSharedKey(string publicKey)
        {
            BigInteger aKey = (CanDecrypt ?
                Decrypt(publicKey) : Verify(publicKey));

            byte[] sharedKey = BigInteger.ModPow(aKey,
                DHPrivate, DHPrime).ToByteArray();

            Array.Reverse(sharedKey);
            return sharedKey;
        }
        public virtual void VerifyDHPrimes(string p, string g)
        {
            DHPrime = Verify(p);
            if (DHPrime <= 2)
            {
                throw new ArgumentException(
                    "P cannot be less than, or equal to 2.\r\n" + DHPrime, nameof(DHPrime));
            }

            DHGenerator = Verify(g);
            if (DHGenerator >= DHPrime)
            {
                throw new ArgumentException(
                    $"G cannot be greater than, or equal to P.\r\n{DHPrime}\r\n{DHGenerator}", nameof(DHGenerator));
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

        protected virtual byte[] PKCSPad(byte[] data)
        {
            var buffer = new byte[BLOCK_SIZE - 1];
            int dataStartPos = (buffer.Length - data.Length);

            buffer[0] = (byte)Padding;
            Buffer.BlockCopy(data, 0, buffer, dataStartPos, data.Length);

            int paddingEndPos = (dataStartPos - 1);
            bool isRandom = (Padding == PKCSPadding.RandomByte);
            for (int i = 1; i < paddingEndPos; i++)
            {
                buffer[i] = (byte)(isRandom ?
                    _numberGenerator.Next(1, 256) : byte.MaxValue);
            }
            return buffer;
        }
        protected virtual byte[] PKCSUnpad(byte[] data)
        {
            Padding = (PKCSPadding)data[0];

            int position = 0;
            while (data[position++] != 0) ;

            var buffer = new byte[data.Length - position];
            Buffer.BlockCopy(data, position, buffer, 0, buffer.Length);

            return buffer;
        }

        protected byte[] HexToBytes(string value)
        {
            var data = new byte[value.Length / 2];
            for (int i = 0; i < value.Length; i += 2)
            {
                data[i / 2] = Convert.ToByte(
                    value.Substring(i, 2), 16);
            }
            return data;
        }
        protected string BytesToHex(byte[] value)
        {
            return BitConverter.ToString(value)
                .Replace("-", string.Empty);
        }
        protected BigInteger RandomInteger(int bitSize)
        {
            var integerData = new byte[bitSize / 8];
            _numberGenerator.NextBytes(integerData);

            integerData[integerData.Length - 1] &= 0x7f;
            return new BigInteger(integerData);
        }

        protected virtual void GenerateDHPrimes(int bitSize)
        {
            DHPrime = RandomInteger(bitSize);
            DHGenerator = RandomInteger(bitSize);

            if (DHGenerator > DHPrime)
            {
                BigInteger tempG = DHGenerator;
                DHGenerator = DHPrime;
                DHPrime = tempG;
            }
        }
        protected virtual void GenerateDHKeys(BigInteger p, BigInteger g)
        {
            DHPrivate = RandomInteger(256);
            DHPublic = BigInteger.ModPow(g, DHPrivate, p);
        }

        protected virtual string Encrypt(Func<BigInteger, BigInteger> calculator, BigInteger value)
        {
            byte[] valueData = Encoding.UTF8.GetBytes(value.ToString());
            valueData = PKCSPad(valueData);

            Array.Reverse(valueData);
            var paddedInteger = new BigInteger(valueData);

            BigInteger calculatedInteger = calculator(paddedInteger);
            byte[] paddedData = calculatedInteger.ToByteArray();
            Array.Reverse(paddedData);

            string encryptedValue = BytesToHex(paddedData).ToLower();
            return encryptedValue.StartsWith("00") ? encryptedValue.Substring(2) : encryptedValue;
        }
        protected virtual BigInteger Decrypt(Func<BigInteger, BigInteger> calculator, string value)
        {
            var signed = BigInteger.Parse("0" + value, NumberStyles.HexNumber);
            BigInteger paddedInteger = calculator(signed);

            byte[] valueData = paddedInteger.ToByteArray();
            Array.Reverse(valueData);

            valueData = PKCSUnpad(valueData);
            return BigInteger.Parse(Encoding.UTF8.GetString(valueData));
        }

        private byte[] ReverseNull(byte[] data)
        {
            bool isNegative = false;
            int newSize = data.Length;
            if (data[0] > 127)
            {
                newSize += 1;
                isNegative = true;
            }

            var reversed = new byte[newSize];
            for (int i = 0; i < data.Length; i++)
            {
                reversed[i] = data[data.Length - (i + 1)];
            }
            if (isNegative)
            {
                reversed[reversed.Length - 1] = 0;
            }
            return reversed;
        }

        public virtual BigInteger CalculatePublic(BigInteger value)
        {
            return BigInteger.ModPow(value, Exponent, Modulus);
        }
        public virtual BigInteger CalculatePrivate(BigInteger value)
        {
            return BigInteger.ModPow(value, PrivateExponent, Modulus);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RSA.Dispose();
            }
        }
    }
}