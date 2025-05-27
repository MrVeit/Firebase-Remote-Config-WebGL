using System.Collections.Generic;
using UnityEngine;
using FRCWebGL.Core;
using FRCWebGL.Data;
using Newtonsoft.Json;

namespace FRCWebGL.Demo
{
    public class TestEntryPoint : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_EDITOR
            Debug.LogWarning("Detected unsupported platform, " +
                "make WebGL build and try again");

            return;
#endif

            Init();
        }

        private void Init()
        {
            var initConfig = new FirebaseInitConfig()
            {
                apiKey = "AIzaSyDVyPidX_BeSEcwwM4tkZ325TBaqXp2P8s",
                authDomain = "frc-unity-webgl.firebaseapp.com",
                projectId = "frc-unity-webgl",
                appId = "1:67934057766:web:a4774b178bebe9f8b87de7",

                minFetchDelayPerMillis = 60000,
                fetchTimeoutMillis = 60000,
            };

            var firstTestJsonItem = new TestJsonData()
            {
                code = 403,
                message = "Hacked by Satoshi Nacamoto"
            };

            var secondTestJsonItem = new TestJsonData()
            {
                code = 200,
                message = "Powered by FRC WebGL"
            };

            var convertJson = JsonConvert.SerializeObject(firstTestJsonItem);

            Debug.Log($"Serialized object from JsonConvert: {convertJson}");

            var defaultConfig = new Dictionary<string, object>()
            {
                { "test_boolean_item_1", true },
                { "test_boolean_item_2", false },
                { "test_int_item_1", 1488 },
                { "test_int_item_2", 69 },
                { "test_string_item_1", "Powered by FRC WebGL" },
                { "test_string_item_2", "Hm, are u ready?" },
                { "test_json_item_1", JsonUtility.ToJson(firstTestJsonItem) },
                { "test_json_item_2", JsonUtility.ToJson(secondTestJsonItem) }
            };

            var convertDict = JsonConvert.SerializeObject(defaultConfig);

            Debug.Log($"Serialized dictionary from JsonConvert: {convertDict}");

            FRCWebBridge.Init(initConfig, defaultConfig, (isSuccess) =>
            {
                Debug.Log($"Web bridge successully initialized, status: {isSuccess}");

                if (isSuccess)
                {
                    var isReadtStatus = FRCWebBridge.IsReady();

                    Debug.Log($"Ready status from lib before load: {isReadtStatus}");

                    FetchConfig();

                    return;
                }

                Debug.LogWarning("Failed to initialize web bridge, something wrong...");
            });
        }

        private void FetchConfig()
        {
            FRCWebBridge.FetchAndActivate((isLoaded) =>
            {
                Debug.Log($"Remote config load status: {isLoaded}");

                var isReadtStatus = FRCWebBridge.IsReady();

                Debug.Log($"Ready status from lib before check load: {isReadtStatus}");

                if (isLoaded)
                {
                    ParseKeys();

                    return;
                }

                Debug.LogWarning("Failed to load remote config data");
            });
        }

        private void ParseKeys()
        {
            var boolItem1 = FRCWebBridge.GetBooleanItem("test_boolean_item_1");
            var boolitem2 = FRCWebBridge.GetBooleanItem("test_boolean_item_2");

            Debug.Log($"Loaded bool_1: {boolItem1}, bool_2: {boolitem2}");

            var intItem1 = FRCWebBridge.GetNumberItem("test_int_item_1");
            var intItem2 = FRCWebBridge.GetNumberItem("test_int_item_2");

            Debug.Log($"Loaded number_1: {intItem1}, number_2: {intItem2}");

            var stringItem1 = FRCWebBridge.GetStringItem("test_string_item_1");
            var stringItem2 = FRCWebBridge.GetStringItem("test_string_item_2");

            Debug.Log($"Loaded string_1: {stringItem1}, string_2: {stringItem2}");

            var jsonItem1 = FRCWebBridge.GetValueItem("test_json_item_1");
            var jsonItem2 = FRCWebBridge.GetValueItem("test_json_item_2");

            Debug.Log($"Loaded json_1: {jsonItem1}, json_2: {jsonItem2}");

            var allItems = FRCWebBridge.GetAllItems();

            Debug.Log($"Loaded all items: {allItems}");

            var bool1Source = FRCWebBridge.GetItemSource("test_boolean_item_1");

            Debug.Log($"bool_1 item source: {bool1Source}");

            var int1Source = FRCWebBridge.GetItemSource("test_int_item_1");

            Debug.Log($"int_1 item source: {int1Source}");

            var string1Source = FRCWebBridge.GetItemSource("test_string_item_1");

            Debug.Log($"string_1 item source: {string1Source}");

            var json1Source = FRCWebBridge.GetItemSource("test_json_item_1");

            Debug.Log($"json_1 item source: {json1Source}");
        }
    }
}