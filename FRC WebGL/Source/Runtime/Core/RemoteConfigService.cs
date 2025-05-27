using System;
using System.Collections.Generic;
using FRCWebGL.Core.Base;
using FRCWebGL.Core.Components;
using FRCWebGL.Data;
using FRCWebGL.Utils;

namespace FRCWebGL.Core
{
    public sealed class RemoteConfigService : IRemoteConfigService
    {
        public IRemoteConfigStorage Storage { get; private set; }
        public ILocalConfigStorage DefaultConfig { get; private set; }

        public RemoteConfigSettings Settings { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsDebugMode { get; private set; }

        public bool IsLoaded { get; private set; }
        public bool IsActivated { get; private set; }

        public event Action<bool> OnInitialized;

        public event Action<bool> OnStorageFetched;
        public event Action<bool> OnStorageActivated;

        public void Init(bool isDebugMode,
            FirebaseInitConfig instanceConfig,
            Dictionary<string, object> defaultConfig)
        {
            if (instanceConfig == null ||
                defaultConfig == null ||
                defaultConfig.Count <= 0)
            {
                FRCWebLogger.LogWarning("Missing required params");

                OnInitialized?.Invoke(false);
            }

            if (!CommonUtils.IsSupportedPlatform())
            {
                OnInitialized?.Invoke(false);

                return;
            }

            if (IsInitialized)
            {
                FRCWebLogger.LogWarning("The plugin is already initialized");

                return;
            }

            IsDebugMode = isDebugMode;

            FRCWebBridge.Init(instanceConfig,
                defaultConfig, (isSuccess) =>
                {
                    FRCWebLogger.Log($"Plugin initialized with status: {isSuccess}");

                    if (isSuccess)
                    {
                        BindOptions(instanceConfig, defaultConfig);

                        OnInitialized?.Invoke(true);

                        return;
                    }

                    OnInitialized?.Invoke(false);

                    FRCWebLogger.LogError($"Failed to initialize " +
                        $"plugin, something wrong...");
                });
        }

        public void FetchConfig()
        {
            if (!CommonUtils.IsSupportedPlatform())
            {
                OnStorageFetched?.Invoke(false);

                return;
            }

            if (!IsInitialized)
            {
                FRCWebLogger.LogWarning("Plugin is not initialized");

                OnStorageFetched?.Invoke(false);

                return;
            }

            FRCWebBridge.FetchConfig((isLoaded) =>
            {
                OnStorageFetched?.Invoke(isLoaded);

                if (isLoaded)
                {
                    FRCWebLogger.Log($"Remote config successfully loaded!");

                    IsLoaded = isLoaded;

                    return;
                }

                FRCWebLogger.LogWarning("Fetch remote config failed");
            });
        }

        public void Activate()
        {
            if (!CommonUtils.IsSupportedPlatform())
            {
                OnStorageActivated?.Invoke(false);

                return;
            }

            if (!IsInitialized)
            {
                FRCWebLogger.LogWarning("Plugin is not initialized");

                OnStorageActivated?.Invoke(false);

                return;
            }

            if (!IsLoaded)
            {
                FRCWebLogger.LogError("Remote config is not loaded");

                OnStorageActivated?.Invoke(false);

                return;
            }

            FRCWebBridge.Activate((isActivated) =>
            {
                OnStorageActivated?.Invoke(isActivated);

                if (isActivated)
                {
                    IsActivated = true;

                    FRCWebLogger.Log("Remote config activated");

                    return;
                }

                FRCWebLogger.LogWarning("Remote config activation failed");
            });
        }

        public void FetchAndActivate()
        {
            if (!CommonUtils.IsSupportedPlatform())
            {
                OnStorageFetched?.Invoke(false);
                OnStorageActivated?.Invoke(false);

                return;
            }

            if (!IsInitialized)
            {
                FRCWebLogger.LogWarning("Plugin is not initialized");

                return;
            }

            FRCWebBridge.FetchAndActivate((isReady) =>
            {
                OnStorageFetched?.Invoke(isReady);
                OnStorageActivated?.Invoke(isReady);

                if (isReady)
                {
                    FRCWebLogger.Log("Remote config ready for use");

                    IsLoaded = true;
                    IsActivated = true;

                    return;
                }

                FRCWebLogger.LogWarning("Remote config is not ready");
            });
        }

        private void BindOptions(FirebaseInitConfig instanceConfig,
            Dictionary<string, object> defaultConfig)
        {
            var minFetchDelay = instanceConfig.minFetchDelayPerMillis;
            var fetchTimeout = instanceConfig.fetchTimeoutMillis;

            Settings = new RemoteConfigSettings(minFetchDelay, fetchTimeout);
            DefaultConfig = new LocalConfigStorage(defaultConfig);
            Storage = new RemoteConfigStorage();

            IsInitialized = true;

            FRCWebLogger.Log("Required components successfully created");
        }
    }
}