using System;
using System.Collections.Generic;

namespace FRCWebGL.Core.Base
{
    public interface ILocalConfigStorage
    {
        event Action<bool> OnUpdated;

        void Add(string keyId, object value);
        void Remove(string keyId);

        Dictionary<string, object> Get();
    }
}