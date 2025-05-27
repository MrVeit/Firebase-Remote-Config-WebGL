using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FRCWebGL.Core;
using FRCWebGL.Core.Base;
using FRCWebGL.Data;

namespace FRCWebGL.Demo
{
    public class TestEntryPoint : MonoBehaviour
    {
        [SerializeField, Space] private Button[] _availableButtons;
        [SerializeField, Space] private Button _loadInt1Button;
        [SerializeField] private Button _loadInt2Button;
        [SerializeField, Space] private Button _loadString1Button;
        [SerializeField] private Button _loadString2Button;
        [SerializeField, Space] private Button _loadBool1Button;
        [SerializeField] private Button _loadBool2Button;
        [SerializeField, Space] private Button _loadJson1Button;
        [SerializeField] private Button _loadJson2Button;
        [SerializeField, Space] private TextMeshProUGUI _debugInfo;

        private IRemoteConfigService _remoteConfigService;

        private IRemoteConfigStorage _remoteStorage;

        private void OnDestroy()
        {
            if (_remoteConfigService == null)
            {
                return;
            }

            _remoteConfigService.OnInitialized -= OnRemoteConfigInitialized;
            _remoteConfigService.OnStorageActivated -= OnRemoteStorageActivated;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            DisableButtons();

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
                message = "Failed to fetch X-Powered-By"
            };

            var secondTestJsonItem = new TestJsonData()
            {
                code = 200,
                message = "Powered by Veittech"
            };

            var defaultConfig = new Dictionary<string, object>()
            {
                { "test_boolean_item_1", true },
                { "test_boolean_item_2", false },
                { "test_int_item_1", 1488 },
                { "test_int_item_2", 69 },
                { "test_string_item_1", "Powered by Veittech" },
                { "test_string_item_2", "Failed to fetch X-Powered-By" },
                { "test_json_item_1", JsonUtility.ToJson(firstTestJsonItem) },
                { "test_json_item_2", JsonUtility.ToJson(secondTestJsonItem) }
            };

            _remoteConfigService = RemoteConfigProvider.Instance;

            _remoteConfigService.OnInitialized += OnRemoteConfigInitialized;
            _remoteConfigService.OnStorageActivated += OnRemoteStorageActivated;

            _remoteConfigService.Init(true, initConfig, defaultConfig);

            _loadInt1Button.onClick.AddListener(LoadInt1);
            _loadInt2Button.onClick.AddListener(LoadInt2);

            _loadString1Button.onClick.AddListener(LoadString1);
            _loadString2Button.onClick.AddListener(LoadString2);

            _loadBool1Button.onClick.AddListener(LoadBool1);
            _loadBool2Button.onClick.AddListener(LoadBool2);

            _loadJson1Button.onClick.AddListener(LoadJson1);
            _loadJson2Button.onClick.AddListener(LoadJson2);
        }

        private void LoadInt1()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_int_item_1";

            var itemData = _remoteStorage.GetNumber(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'int' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void LoadInt2()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_int_item_2";

            var itemData = _remoteStorage.GetNumber(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'int' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void LoadString1()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_string_item_1";

            var itemData = _remoteStorage.GetString(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'string' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void LoadString2()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_string_item_2";

            var itemData = _remoteStorage.GetString(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'string' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void LoadBool1()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_boolean_item_1";

            var itemData = _remoteStorage.GetBoolean(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'bool' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void LoadBool2()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_boolean_item_2";

            var itemData = _remoteStorage.GetBoolean(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'bool' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void LoadJson1()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_json_item_1";

            var itemData = _remoteStorage.GetValue(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'json' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void LoadJson2()
        {
            if (_remoteStorage == null)
            {
                Debug.LogWarning("Remote storage is not initialized");

                return;
            }

            var key = "test_json_item_2";

            var itemData = _remoteStorage.GetValue(key);
            var dataSource = _remoteStorage.GetStorageType(key);

            WriteLog($"Loaded value by 'json' item with key: '{key}', " +
                $"storage type: '{dataSource}', data: {itemData}");
        }

        private void WriteLog(string message)
        {
            _debugInfo.text = $"DEBUG INFO: {message}";
        }

        private void EnableButtons()
        {
            foreach (var button in _availableButtons)
            {
                button.interactable = true;
            }
        }

        private void DisableButtons()
        {
            foreach (var button in _availableButtons)
            {
                button.interactable = false;
            }
        }

        private void OnRemoteConfigInitialized(bool isSuccess)
        {
            Debug.Log($"Remote config init status: {isSuccess}");

            if (isSuccess)
            {
                _remoteConfigService.FetchAndActivate();

                _remoteStorage = _remoteConfigService.Storage;

                EnableButtons();

                WriteLog("Remote config service initialized and ready for fetch");

                return;
            }

            WriteLog("Failed to initialize remote config service");
        }

        private void OnRemoteStorageActivated(bool isSuccess)
        {
            if (isSuccess)
            {
                var allItems = FRCWebBridge.GetAllItems();

                WriteLog($"Remote config successfully activated and " +
                    $"ready for use, cached data: {allItems}");

                return;
            }

            WriteLog("Failed to activate previous fetched remote config");
        }
    }
}