using System.Collections.Generic;
using FRCWebGL.Common;
using FRCWebGL.Core.Base;

namespace FRCWebGL.Core.Components
{
    public sealed class RemoteConfigStorage : IRemoteConfigStorage
    {
        public int GetNumber(string itemKey)
        {
            return FRCWebBridge.GetNumberItem(itemKey);
        }

        public bool GetBoolean(string itemKey)
        {
            return FRCWebBridge.GetBooleanItem(itemKey);
        }

        public string GetString(string itemKey)
        {
            return FRCWebBridge.GetStringItem(itemKey);
        }

        public object GetValue(string itemKey)
        {
            return FRCWebBridge.GetValueItem(itemKey);
        }

        public Dictionary<string, object> GetAll()
        {
            return FRCWebBridge.GetAllItems();
        }

        public ValueSources GetStorageType(string itemKey)
        {
            return FRCWebBridge.GetItemSource(itemKey);
        }
    }
}