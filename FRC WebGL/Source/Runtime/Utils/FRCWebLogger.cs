using UnityEngine;
using FRCWebGL.Core.Base;
using FRCWebGL.Infrastructure;

namespace FRCWebGL.Utils
{
    public sealed class FRCWebLogger
    {
        private static readonly IRemoteConfigService _mainService =
            ServiceLocator.Get<IRemoteConfigService>();

        private static readonly bool _isEnabled = 
            _mainService != null ? _mainService.IsDebugMode : false;

        private static bool IsEnabled => _isEnabled;

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