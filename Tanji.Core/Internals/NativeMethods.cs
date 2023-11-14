using System.Runtime.InteropServices;

namespace Tanji.Core;

internal static partial class NativeMethods
{
    [LibraryImport("Kernel32.dll", EntryPoint = "CreateHardLinkA", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
}