using System;
using System.Collections.Generic;
using FRCWebGL.Common;

namespace FRCWebGL.Base
{
    public interface IRemoteConfig
    {
        void Init(string instanceConfigJson,
            string defaultConfigJson, Action<bool> onInitialized);

        void FetchConfig(Action<bool> onLoaded);
        void Activate(Action<bool> onActivated);

        void FetchAndActivate(Action<bool> onActivated);

        void SetDefaultConfig(string defaultConfigJson, Action<bool> onUpdated);
        void SetLogLevel(Action<bool> onUpdated);

        bool IsReady();

        bool GetBooleanItem(string itemKey);
        int GetNumberItem(string itemKey);
        string GetStringItem(string itemKey);
        string GetValueItem(string itemKey);

        Dictionary<string, string> GetAllItems(string itemKey);

        ValueSources GetItemSource(string itemKey);
    }
}