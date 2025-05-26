using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using FRCWebGL.Common;
using FRCWebGL.Data;
using FRCWebGL.Utils;

namespace FRCWebGL.Core
{
    internal static class FRCWebBridge
    {
        private static Action<bool> _onInitialized;

        private static Action<bool> _onConfigLoaded;
        private static Action<bool> _onConfigActivated;
        private static Action<bool> _onConfigLoadFinished;

        private static Action<bool> _onDefaultConfigChanged;

#region ExternalCalls

        [DllImport("__Internal")]
        private static extern void InitFRC(string instanceConfigJson,
            string defaultConfigJson, Action<int> onInitialized);

        [DllImport("__Internal")]
        private static extern void FetchConfigFRC(Action<int> onLoaded);

        [DllImport("__Internal")]
        private static extern void ActivateFRC(Action<int> onActivated);

        [DllImport("__Internal")]
        private static extern void FetchAndActivateFRC(Action<int> onLoaded);

        [DllImport("__Internal")]
        private static extern void SetDefaultConfigFRC(
            string defaultConfigJson, Action<int> onUpdated);

        [DllImport("__Internal")]
        private static extern int IsReadyFRC();

        [DllImport("__Internal")]
        private static extern int GetBooleanFRC(string itemKey);

        [DllImport("__Internal")]
        private static extern int GetNumberFRC(string itemKey);

        [DllImport("__Internal")]
        private static extern IntPtr GetStringFRC(string itemKey);

        [DllImport("__Internal")]
        private static extern IntPtr GetValueFRC(string itemKey);

        [DllImport("__Internal")]
        private static extern IntPtr GetAllFRC();

        [DllImport("__Internal")]
        private static extern IntPtr GetValueSourceFRC(string itemKey);

#endregion

        #region NativeCallbacks

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void OnInitialize(int successCode)
        {
            _onInitialized?.Invoke(ToBoolean(successCode));

            _onInitialized = null;
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void OnConfigLoad(int successCode)
        {
            _onConfigLoaded?.Invoke(ToBoolean(successCode));

            _onConfigLoaded = null;
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void OnConfigActive(int successCode)
        {
            _onConfigActivated?.Invoke(ToBoolean(successCode));

            _onConfigActivated = null;
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void OnConfigLoadFinish(int successCode)
        {
            _onConfigLoadFinished?.Invoke(ToBoolean(successCode));

            _onConfigLoadFinished = null;
        }

        [MonoPInvokeCallback(typeof(Action<int>))]
        private static void OnDefaultConfigUpdate(int successCode)
        {
            _onDefaultConfigChanged?.Invoke(ToBoolean(successCode));

            _onDefaultConfigChanged = null;
        }

#endregion

        public static void Init(FirebaseInitConfig instanceConfig, 
            Dictionary<string, string> defaultConfig, Action<bool> onInitialized)
        {
            if (instanceConfig == null)
            {
                FRCWebLogger.LogWarning("Instance config is empty, init cancelled");

                onInitialized?.Invoke(false);

                return;
            }

            _onInitialized = onInitialized;

            var instanceConfigJson = JsonUtility.ToJson(instanceConfig);
            var defaultConfigJson = JsonUtility.ToJson(defaultConfig);

            InitFRC(instanceConfigJson, defaultConfigJson, OnInitialize);
        }

        public static void FetchConfig(Action<bool> onLoaded)
        {
            _onConfigLoaded = onLoaded;

            FetchConfigFRC(OnConfigLoad);
        }

        public static void Activate(Action<bool> onActivated)
        {
            _onConfigActivated = onActivated;

            ActivateFRC(OnConfigActive);
        }

        public static void FetchAndActivate(Action<bool> onLoadFinished)
        {
            _onConfigLoadFinished = onLoadFinished;

            FetchAndActivateFRC(OnConfigLoadFinish);
        }

        public static bool IsReady()
        {
            return ToBoolean(IsReadyFRC());
        }

        public static bool GetBooleanItem(string itemKey)
        {
            return ToBoolean(GetBooleanFRC(itemKey));
        }

        public static int GetNumberItem(string itemKey)
        {
            return GetNumberFRC(itemKey);
        }

        public static string GetStringItem(string itemKey)
        {
            return ToString(GetStringFRC(itemKey));
        }

        public static string GetValueItem(string itemKey)
        {
            return ToString(GetValueFRC(itemKey));
        }

        public static Dictionary<string, string> GetAllItems()
        {
            var ptrData = GetAllFRC();

            if (ptrData == IntPtr.Zero)
            {
                FRCWebLogger.LogWarning("Loaded remote items was not available");

                return new Dictionary<string, string>();
            }

            var jsonPtrData = ToString(ptrData);

            FRCWebLogger.Log($"Parsed converted items list to json: {jsonPtrData}");

            var itemsList = JsonUtility.FromJson<
                SerializableDictionary>(jsonPtrData).ToDictionary();

            return itemsList;
        }

        public static ValueSources GetItemSource(string itemKey)
        {
            var ptr = GetValueSourceFRC(itemKey);

            if (ptr == IntPtr.Zero)
            {
                return ValueSources.Unknown;
            }

            return ToString(ptr) switch
            {
                "static" => ValueSources.Static,
                "default" => ValueSources.Default,
                "remote" => ValueSources.Remote,
                _ => ValueSources.Unknown,
            };
        }

        public static void SetDefaultConfig(
            Dictionary<string, string> defaultConfig, Action<bool> onUpdated)
        {
            _onDefaultConfigChanged = onUpdated;

            var jsonConfig = JsonUtility.ToJson(defaultConfig);

            FRCWebLogger.Log($"Default local config for update: {jsonConfig}");

            SetDefaultConfigFRC(jsonConfig, OnDefaultConfigUpdate);
        }

        private static string ToString(IntPtr ptr)
        {
            return CommonUtils.IntPtrToString(ptr);
        }

        private static bool ToBoolean(int statusCode)
        {
            return CommonUtils.IntToBool(statusCode);
        }
    }
}