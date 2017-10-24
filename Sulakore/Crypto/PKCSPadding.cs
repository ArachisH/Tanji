namespace Sulakore.Crypto
{
    public enum PKCSPadding
    {
        /// <summary>
        /// Represents a padding type that will fill a byte array's empty indices with the maximum value of a <see cref="byte"/>.
        /// </summary>
        MaxByte = 1,
        /// <summary>
        /// Represents a padding type that will fill a byte array's empty indices with random byte values.
        /// </summary>
        RandomByte = 2
    }
}