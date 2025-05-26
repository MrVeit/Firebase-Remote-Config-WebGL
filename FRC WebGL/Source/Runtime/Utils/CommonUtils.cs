using System;
using System.Runtime.InteropServices;

namespace FRCWebGL.Utils
{
    public static class CommonUtils
    {
        public static bool IntToBool(int successCode)
        {
            return successCode == 1;
        }

        public static string IntPtrToString(IntPtr ptr)
        {
            var convertedString = Marshal.PtrToStringAnsi(ptr);

            return ptr == IntPtr.Zero ? null : convertedString;
        }
    }
}