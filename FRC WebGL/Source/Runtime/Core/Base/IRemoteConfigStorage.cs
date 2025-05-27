using System.Collections.Generic;
using FRCWebGL.Common;

namespace FRCWebGL.Core.Base
{
    public interface IRemoteConfigStorage
    {
        bool GetBoolean(string itemKey);
        int GetNumber(string itemKey);
        string GetString(string itemKey);
        object GetValue(string itemKey);

        Dictionary<string, object> GetAll();

        ValueSources GetStorageType(string itemKey);
    }
}