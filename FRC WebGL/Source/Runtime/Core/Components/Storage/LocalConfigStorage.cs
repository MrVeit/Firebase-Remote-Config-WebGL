using System;
using System.Collections.Generic;
using FRCWebGL.Core.Base;
using FRCWebGL.Utils;

namespace FRCWebGL.Core.Components
{
    public sealed class LocalConfigStorage : ILocalConfigStorage
    {
        private readonly Dictionary<string, object> _values = new();

        public event Action<bool> OnUpdated;

        public LocalConfigStorage(
            Dictionary<string, object> defaultValues)
        {
            _values = defaultValues;
        }

        public void Add(string keyId, object keyValue)
        {
            if (_values.ContainsKey(keyId))
            {
                FRCWebLogger.LogWarning(
                    $"Value by key {keyId} already exist");

                return;
            }

            _values.Add(keyId, keyValue);

            UpdateNativeConfig();
        }

        public void Remove(string keyId)
        {
            if (!_values.ContainsKey(keyId))
            {
                FRCWebLogger.LogWarning(
                    $"Value by key {keyId} not found");

                return;
            }

            _values.Remove(keyId);

            UpdateNativeConfig();
        }

        public Dictionary<string, object> Get()
        {
            return _values.Count > 1 ? _values : null;
        }

        private void UpdateNativeConfig()
        {
            FRCWebBridge.SetDefaultConfig(
                _values, (isSuccess) =>
            {
                OnUpdated?.Invoke(isSuccess);
            });
        }
    }
}