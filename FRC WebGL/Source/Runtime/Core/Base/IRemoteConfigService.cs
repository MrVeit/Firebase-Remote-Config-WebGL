using System;
using System.Collections.Generic;
using FRCWebGL.Core.Components;
using FRCWebGL.Data;

namespace FRCWebGL.Core.Base
{
    public interface IRemoteConfigService
    {
        RemoteConfigSettings Settings { get; }
        ILocalConfigStorage DefaultConfig { get; }
        IRemoteConfigStorage Storage { get; }

        bool IsInitialized { get; }
        bool IsDebugMode { get; }

        bool IsLoaded { get; }

        event Action<bool> OnInitialized;

        event Action<bool> OnStorageFetched;
        event Action<bool> OnStorageActivated;

        void Init(bool isDebugMode,
            FirebaseInitConfig instanceConfig,
            Dictionary<string, object> defaultConfig);

        void FetchConfig();
        void Activate();

        void FetchAndActivate();
    }
}