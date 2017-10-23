using System;
using System.Text;

namespace Sulakore.Protocol
{
    /// <summary>
    /// Converts base data types to an array of bytes in the big-endian byte order, and an array of bytes in the big-endian byte order to base data types.
    /// </summary>
    public static class BigEndian
    {
        public static int GetSize(int value) => 4;
        public static int GetSize(bool value) => 1;
        public static int GetSize(ushort value) => 2;
        public static int GetSize(string value)
        {
            return Encoding.UTF8.GetByteCount(value) + 2;
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes in the big-endian byte order.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns></returns>
        public static byte[] GetBytes(int value)
        {
            var buffer = new byte[4];
            buffer[0] = (byte)(value >> 24);
            buffer[1] = (byte)(value >> 16);
            buffer[2] = (byte)(value >> 8);
            buffer[3] = (byte)value;
            return buffer;
        }
        public static byte[] GetBytes(bool value)
        {
            var buffer = new byte[1] { 0 };
            buffer[0] = (byte)(value ? 1 : 0);

            return buffer;
        }
        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes in the big-endian byte order.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns></returns>
        public static byte[] GetBytes(ushort value)
        {
            var buffer = new byte[2];
            buffer[0] = (byte)(value >> 8);
            buffer[1] = (byte)value;
            return buffer;
        }
        public static byte[] GetBytes(string value)
        {
            byte[] stringData = Encoding.UTF8.GetBytes(value);
            byte[] lengthData = GetBytes((ushort)stringData.Length);

            var buffer = new byte[lengthData.Length + stringData.Length];
            Buffer.BlockCopy(lengthData, 0, buffer, 0, lengthData.Length);
            Buffer.BlockCopy(stringData, 0, buffer, lengthData.Length, stringData.Length);

            return buffer;
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array using the big-endian byte order.
        /// </summary>
        /// <param name="value">An array of bytes in the big-endian byte order.</param>
        /// <param name="startIndex">The starting position within the value.</param>
        /// <returns></returns>
        public static int ToInt32(byte[] value, int startIndex)
        {
            int result = (value[startIndex++] << 24);
            result += (value[startIndex++] << 16);
            result += (value[startIndex++] << 8);
            result += (value[startIndex]);
            return result;
        }
        public static bool ToBoolean(byte[] value, int startIndex)
        {
            return value[startIndex] == 1;
        }
        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array using the big-endian byte order.
        /// </summary>
        /// <param name="value">An array of bytes in the big-endian byte order.</param>
        /// <param name="startIndex">The starting position within the value.</param>
        /// <returns></returns>
        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            int result = (value[startIndex++] << 8);
            result += (value[startIndex]);
            return (ushort)result;
        }
        public static string ToString(byte[] value, int startIndex)
        {
            ushort stringLength =
                ToUInt16(value, startIndex);

            string result = Encoding.UTF8
                .GetString(value, startIndex + 2, stringLength);

            return result;
        }
    }
}