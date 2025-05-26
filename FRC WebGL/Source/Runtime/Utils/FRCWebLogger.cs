using UnityEngine;

namespace FRCWebGL.Utils
{
    public class FRCWebLogger : MonoBehaviour
    {
        private static bool IsEnabled => true;

        public const string PREFIX = "FRC WebGL";

        public static void Log(object message)
        {
            if (IsEnabled)
            {
                Debug.Log($"[{PREFIX}] {message}");
            }
        }

        public static void LogWarning(object message)
        {
            if (IsEnabled)
            {
                Debug.LogWarning($"[{PREFIX}] {message}");
            }
        }

        public static void LogError(object message)
        {
            if (IsEnabled)
            {
                Debug.LogError($"[{PREFIX}] {message}");
            }
        }
    }

}