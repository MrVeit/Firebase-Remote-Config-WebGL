const firebaseRCInstance = {

    // Class definition

    $frcInstance: {
        getAllocString: function(data)
        {
            if (typeof allocate === "undefined")
            {
                const length = lengthBytesUTF8(data) + 1;

                let ptr = _malloc(length);

                stringToUTF8(data, ptr, length);

                return ptr;
            }

            return allocate(intArrayFromString(data), 'i8', ALLOC_STACK);
        },

        sendToUnity: function(callId, callback, dataPtr)
        {
            dynCall(callId, callback, dataPtr);

            if (callId === 'v' || dataPtr === [0] || 
                dataPtr === [1])
            {
                return;
            }

            _free(dataPtr);
        },

        isAvailableLib: function()
        {
            if (typeof firebase === "undefined")
            {
                console.error("[FRC WebGL] Firebase Javascript SDK "+
                    "not loaded or not found, try again later!");

                return false;
            }

            return true;
        },

        tryGetLibInstance: function()
        {
            const remoteConfigInstance = window.FRCInstance;

            if (!remoteConfigInstance)
            {
                console.error("[FRC WebGL] Getting an instance of FRC "+
                    "failed, try initializing the plugin and trying again");
                
                return null;
            }

            return remoteConfigInstance;
        },

        init: function(instanceConfigPtr, 
            defaultConfigPtr, onInitialized)
        {
            if (!this.isAvailableLib())
            {
                this.sendToUnity('vi', onInitialized, [0]);

                return;
            }

            var instanceConfig = {};
            var defaultConfig = {};

            try
            {
                instanceConfig = JSON.parse(UTF8ToString(instanceConfigPtr));
                defaultConfig = JSON.parse(UTF8ToString(defaultConfigPtr));

                console.log(`[FRC WebGL] Parsed firebase init `+
                    `config: ${JSON.stringify(instanceConfig)}, `+
                    `default: ${JSON.stringify(defaultConfig)}`);
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to parse `+
                    `firebase configs for initialize plugin`);

                this.sendToUnity('vi', onInitialized, [0]);

                return;
            }

            const firebaseConfig =
            {
                apiKey: instanceConfig.apiKey,
                authDomain: instanceConfig.authDomain,
                projectId: instanceConfig.projectId,
                storageBucket: instanceConfig.storageBucket || null,
                messagingSenderId: instanceConfig.messagingSenderId || null,
                appId: instanceConfig.appId
            };

            try
            {
                const firebaseApp = firebase.initializeApp(firebaseConfig);
                const remoteConfigInstance = firebase.remoteConfig(firebaseApp);

                remoteConfigInstance.settings =
                {
                    minimumFetchIntervalMillis: instanceConfig.minFetchDelayPerMillis,
                    fetchTimeoutMillis: instanceConfig.fetchTimeoutMillis
                };

                remoteConfigInstance.defaultConfig = defaultConfig;

                window.FRCInstance = remoteConfigInstance;

                this.sendToUnity('vi', onInitialized, [1]);

                console.log("[FRC WebGL] Firebase Remote Config successfullly initialized!");
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to initialize plugin, reason: ${error}`);

                this.sendToUnity('vi', onInitialized, [0]);
            }
        },

        fetchConfig: function(onLoadedd)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                console.error(`[FRC WebGL] Failed to fetch remote config...`);

                this.sendToUnity('vi', onLoadedd, [0]);

                return;
            }

            remoteConfig.fetchConfig().then(() =>
            {
                console.log(`[FRC WebGL] Remote config successfully loaded`);

                this.sendToUnity('vi', onLoadedd, [1]);
            })
            .catch((error) =>
            {
                console.warn(`[FRC WebGL] Failed to load remote config, reason: ${error.message}`);

                this.sendToUnity('vi', onLoadedd, [0]);
            })
        },

        activate: function(onActivated)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                console.error(`[FRC WebGL] Failed to activate cached remote config...`);

                this.sendToUnity('vi', onActivated, [0]);
                
                return;
            }

            remoteConfig.activate().then((isActivated) =>
            {
                console.log(`[FRC WebGL] Target config activated with status: ${isActivated}`);

                const status = isActivated ? 1 : 0;

                this.sendToUnity('vi', onActivated, [status]);
            })
            .catch((error) =>
            {
                console.error(`[FRC WebGL] Failed to activate config, reasoN: ${error.message}`);

                this.sendToUnity('vi', onActivated, [0]);
            })
        },

        fetchAndActivate: function(onLoaded)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                this.sendToUnity('vi', onLoaded, [0]);
                
                return;
            }

            const valueId = UTF8ToString(valueIdPtr);

            console.log(`[FRC WebGL] Start fetching and activate remote config...`);

            remoteConfig.fetchAndActivate().then(() =>
            {
                console.log(`[FRC WebGL] Remote config successfully fetched and activated`);

                this.sendToUnity('vi', onLoaded, [1]);
            })
            .catch((error) =>
            {
                console.error(`[FRC WebGL] Failed to activate remote `+
                    `config, reason: ${error.message}`);

                this.sendToUnity('vi', onLoaded, [0]);
            });
        },
        
        isReady: function()
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                console.error(`[FRC WebGL] Remote config is not ready...`);

                return false;
            }

            remoteConfig.ensureInitialized().then(() =>
            {
                console.log(`[FRC WebGL] Remote config is ready, `+
                    `default/remote values available`);

                return true;
            })
            .catch((error) =>
            {
                console.error(`[FRC WebGL] Remote config is `+
                    `not ready, reason: ${error}`);

                return false;
            });
        },

        getBoolean: function(keyPtr)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                return 0;
            }

            var targetKey = UTF8ToString(keyPtr);

            try
            {
                const booleanItem = remoteConfig.getBoolean(targetKey) ? 1 : 0;

                console.log(`[FRC WebGL] Successfully parsed boolean item: `+
                    `key: ${targetKey}, value: ${booleanItem}`);

                return booleanItem;
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to get item with `+
                    `type 'boolean', reason: ${error.message}`);

                return 0;
            }
        },

        getNumber: function(keyPtr)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                return 0;
            }

            var targetKey = UTF8ToString(keyPtr);

            try
            {
                const numberItem = remoteConfig.getNumber(targetKey) || 0;

                console.log(`[FRC WebGL] Successfully parsed number item: `+
                    `key: ${targetKey}, value: ${numberItem}`);

                return numberItem;
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to get item with `+
                    `type 'number', reason: ${error.message}`);

                return 0;
            }
        },

        getString: function(keyPtr)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                return this.getAllocString('');
            }

            var targetKey = UTF8ToString(keyPtr);

            try
            {
                const stringItem = remoteConfig.getString(targetKey) || "";

                console.log(`[FRC WebGL] Successfully parsed string item: `+
                    `key: ${targetKey}, value: ${stringItem}`);

                return this.getAllocString(stringItem);
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to get item with `+
                    `type 'string', reason: ${error.message}`);

                return this.getAllocString('');
            }
        },

        getValue: function(keyPtr)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                return this.getAllocString('');
            }

            var targetKey = UTF8ToString(keyPtr);

            try
            {
                const objectItem = remoteConfig.getValue(targetKey).asString();

                console.log(`[FRC WebGL] Successfully parsed object item: `+
                    `key: ${targetKey}, value: ${objectItem}`);

                return this.getAllocString(objectItem);
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to get item with `+
                    `type 'object', reason: ${error.message}`);

                return this.getAllocString('');
            }
        },

        getSource: function(keyPtr)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                return this.getAllocString('');
            }

            var targetKey = UTF8ToString(keyPtr);

            try
            {
                const valueSource = remoteConfig.getValue(targetKey).getSource();

                console.log(`[FRC WebGL] Loaded value source: `+
                    `'${valueSource}' for item key: ${targetKey}`);

                return this.getAllocString(valueSource);
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to get value source `+
                    `with key: ${targetKey}, reason: ${error.message}`);

                return this.getAllocString('');
            }
        },

        getAll: function()
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                return this.getAllocString("");
            }

            try
            {
                const rawItems = remoteConfig.getAll();
                
                var plain = {};

                for (var key in rawItems)
                {
                    if (Object.prototype.hasOwnProperty.call(rawItems, key))
                    {
                        plain[key] = rawItems[key].asString();
                    }
                }

                var jsonItems = JSON.stringify({ dictionary: plain });

                console.log(`[FRC WebGL] Successfully parsed all items: ${jsonItems}`);
                
                return this.getAllocString(jsonItems);
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to get all `+
                    `items from config, reason: ${error.message}`);

                return this.getAllocString('{}');
            }
        },

        getLastFetchStatus: function()
        {
            
        },

        getFetchTimeMillis: function()
        {

        },

        setDefaultConfig: function(defaultConfigPtr, onUpdated)
        {
            const remoteConfig = this.tryGetLibInstance();
            const parsedConfig = JSON.parse(UTF8ToString(defaultConfigPtr));

            if (!remoteConfig || !parsedConfig)
            {
                console.error(`[FRC WebGL] Failed to update the `+
                    `plugin's default configuration...`);

                this.sendToUnity('vi', onUpdated, [0]);

                return;
            }

            try
            {
                remoteConfig.defaultConfig = parsedConfig;

                console.log(`[FRC WebGL] The default configuration `+
                    `has been successfully updated`);

                this.sendToUnity('vi', onUpdated, [1]);
            }
            catch (error)
            {
                console.error(`[FRC WebGL] Failed to update default `+
                    `config, reason: ${error.message}`);

                this.sendToUnity('vi', onUpdated, [0]);
            }
        },

        setLogLevel: function(logLevelPtr, onUpdated)
        {
            const remoteConfig = this.tryGetLibInstance();

            if (!remoteConfig)
            {
                console.error(`[FRC WebGL] Failed to set log level...`);

                this.sendToUnity('vi', onUpdated, [0]);

                return;
            }

            const logLevel = UTF8ToString(logLevelPtr);

            remoteConfig.setLogLevel(logLevel);

            console.log(`[FRC WebGL] Log level for native lib successfully updated`);

            this.sendToUnity('vi', onUpdated, [1]);
        },

        setCustomSignals: function(targetingDataPtr)
        {

        }
    },

    // External C# calls

    InitFRC: function(instanceConfigPtr, 
        defaultConfigPtr, onInitialized)
    {
        frcInstance.init(instanceConfigPtr, 
            defaultConfigPtr, onInitialized);
    },

    FetchConfigFRC: function(onLoaded)
    {
        frcInstance.fetchConfig(onLoaded);
    },

    ActivateFRC: function(onActivated)
    {
        frcInstance.activate(onActivated);
    },

    FetchAndActivateFRC: function(onLoaded)
    {
        frcInstance.fetchAndActivate(onLoaded);
    },

    SetDefaultConfigFRC: function(defaultConfigPtr, onUpdated)
    {
        frcInstance.setDefaultConfig(defaultConfigPtr, onUpdated);
    },

    SetLogLevelFRC: function(logLevelPtr, onUpdated)
    {
        frcInstance.setLogLevel(logLevelPtr, onUpdated);
    },

    SetCustomSignalsFRC: function(targetingDataPtr)
    {
        frcInstance.setCustomSignals(targetingDataPtr);
    },

    IsReadyFRC: function()
    {
        return frcInstance.isReady();
    },

    GetBooleanFRC: function(keyPtr)
    {
        return frcInstance.getBoolean(keyPtr);
    },

    GetNumberFRC: function(keyPtr)
    {
        return frcInstance.getNumber(keyPtr);
    },

    GetStringFRC: function(keyPtr)
    {
        return frcInstance.getString(keyPtr);
    },

    GetValueFRC: function(keyPtr)
    {
        return frcInstance.getValue(keyPtr);
    },

    GetAllFRC: function()
    {
        return frcInstance.getAll();
    },

    GetValueSourceFRC: function(keyPtr)
    {
        return frcInstance.getSource(keyPtr);
    }
};

autoAddDeps(firebaseRCInstance, '$frcInstance');
mergeInto(LibraryManager.library, firebaseRCInstance);