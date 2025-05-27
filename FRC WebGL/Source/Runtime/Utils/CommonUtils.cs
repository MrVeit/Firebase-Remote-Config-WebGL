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

        public static bool IsSupportedPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return true;
#endif

            FRCWebLogger.LogWarning("Unsupported platform detected. " +
                "Build WebGL and try again to initialize the plugin.");

            return false;
        }
    }
}