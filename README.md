# FIREBASE REMOTE CONFIG WEBGL

[![Unity](https://img.shields.io/badge/Unity-2020.1+-2296F3.svg?color=318CE7&style=flat-square&logo=Unity&logoColor=E0FFFF)](https://unity.com/releases/editor/archive)
[![Version](https://img.shields.io/github/package-json/v/MrVeit/Firebase-Remote-Config-WebGL?color=318CE7&style=flat-square&logo=buffer&logoColor=E0FFFF)](package.json)
[![License](https://img.shields.io/github/license/MrVeit/Firebase-Remote-Config-WebGL?color=318CE7&style=flat-square&logo=github&logoColor=E0FFFF)](LICENSE)
![Last commit](https://img.shields.io/github/last-commit/MrVeit/Firebase-Remote-Config-WebGL/master?color=318CE7&style=flat-square&logo=alwaysdata&logoColor=E0FFFF)
![Last release](https://img.shields.io/github/release-date/MrVeit/Firebase-Remote-Config-WebGL?color=318CE7&style=flat-square&logo=Dropbox&logoColor=E0FFFF)
![Downloads](https://img.shields.io/github/downloads/MrVeit/Firebase-Remote-Config-WebGL/total?color=318CE7&style=flat-square&logo=codeigniter&logoColor=E0FFFF)

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/pluginMainBackground.png" alt="qr"/>
</p>

**FRC WEBGL** is a plugin for using Firebase Remote Config functionality in WebGL projects based on the Firebase JavaScript SDK built on Unity.

# Technical Demo

You can test the plugin without installation in a demo scene [via this link](https://mrveit.github.io/Firebase-Remote-Config-WebGL/).

# Installation

**[Download the latest version of the SDK via the .unityPackage file here](https://github.com/MrVeit/Firebase-Remote-Config-WebGL/releases).**

# Creating app

Before you start using this plugin - you need to create a configuration of your game.

This can be done in the Firebase console by following this link.

P.S: if you have already used this service for your mobile projects, then skip these steps [by going to this section](https://github.com/MrVeit/Firebase-Remote-Config-WebGL#initialization).

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/firebaseAppCreateStep_1.png" alt="qr"/>
</p>

After successfully creating the project, you need to go to the `Remote Config` section.
This can be done by the path `Run -> Remote Config`, having previously expanded the Run tab.

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/firebaseAppCreateStep_2.png" alt="qr"/>
</p>

Now you can create your test/real parameters, after clicking on the `Create configuration` button.

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/firebaseAppCreateStep_3.png" alt="qr"/>
</p>

To allow the Unity client to access these settings, after creating them, be sure to publish the changes using the `Publish Changes` button.

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/firebaseAppCreateStep_4.png" alt="qr"/>
</p>

Well, almost everything is ready! 
It remains to get the project configuration data to use it in the plugin.

To do this, go through the `gear` icon to `Project Settings` to create them.

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/firebaseAppCreateStep_5.png" alt="qr"/>
</p>

Now scroll down the page just below and click on the `</>` icon, which stands for web application, 
since my plugin, is built on top of their JavaScript SDK!

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/firebaseAppCreateStep_6.png" alt="qr"/>
</p>

Now switch to the `Config` tab in the same window and save these data, as we will need them in the code.

<p align="left">
 <img width="600px" src="https://github.com/MrVeit/Firebase-Remote-Config-WebGL/blob/master/Assets/firebaseAppCreateStep_7.png" alt="qr"/>
</p>

# Initialization

In plugin `version 1.0.0`, it is available **ONLY MANUAL INITIALIZATION** with code.

Below is a test example of what this initialization might look like:

```c#
public sealed class InitPluginExample: MonoBehaviour
{
    private IRemoteConfigService‎ _remoteConfig;
    private IRemoteConfigStorage _remoteStorage;

    private void Start()
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

        var defaultConfig = new Dictionary<string, object>()
        {
            { "test_boolean_item_1", true },
            { "test_boolean_item_2", false },
            { "test_int_item_1", 1488 },
            { "test_int_item_2", 69 },
            { "test_string_item_1", "Powered by Veittech" },
            { "test_string_item_2", "Failed to fetch X-Powered-By" }
        };

        _remoteConfigService = RemoteConfigProvider.Instance;

        _remoteConfigService.OnInitialized += OnRemoteConfigInitialized;

        _remoteConfigService.Init(true, initConfig, defaultConfig);
    }

    private void OnRemoteConfigInitialized(bool isSuccess)
    {
        Debug.Log($"Remote config init status: {isSuccess}");

        if (!isSuccess)
        {
            Debug.LogError("Failed to initialize remote config service");

            return;
        }

        _remoteStorage = _remoteConfigService.Storage;

        Debug.Log("Remote config service initialized and ready for fetch");
    }
}
```

In the `FirebaseInitConfig` object, we use the data we created earlier in the Firebase console for our project in this section.

As recommended by the official Remote Config documentation, the value of the `minFetchDelayPerMillis` variable should be set to a **MINIMUM 12 HOURS** in milliseconds when building a project **for production**.
**For testing** purposes, you can set the value to 1 minute if you do not make requests too often to avoid getting an `exceed limit` error.

It is strongly **RECOMMENDED** to create standard configs in `Dictionary<string, object>` format, which will be placed locally in `local storage`, after the WebGL build. 
You can also pass an empty dictionary with no values, but in that case the local data **MAY NOT BE AVAILABLE** from the storage unless you force the loading of the config values.

In case you do not specify values for variables `minFetchDelayPerMillis` and `fetchTimeoutMillis` in object `FirebaseInitConfig`, the plugin will do it by itself, specifying values for testing **EVALUATE 1 MINUTE** in milliseconds.

**P.S:** The keys and values must match those you previously created in the `Remote Config` console.

Initially, access to the `IRemoteConfigService` implementation, as a single instance, is provided through the `RemoteConfigProvider` proxy class. If you want to “register” a service yourself to access it in your project, the following are examples using `Service Locator/Zenject/VContainer`.

For example, through an implementation of the `Service Locator` pattern:
```c#
ServiceLocator.Bind<IRemoteConfigService>(new RemoteConfigService());

remoteConfig= ServiceLocator.Get<IRemoteConfigService>();
```

Via DI frameworks like `Zenject/VContainer`:
```c#
Container.Bind<IRemoteConfigService>().To<RemoteConfigService>().AsSingle();

IContainerBuilder builder = builder.Register<RemoteConfigService>(Lifetime.Singleton).As<IRemoteConfigService>();
```

# Usage Template

# Donations

Multichain Wallet (BTC/ETH/BNB/MATIC)
```
0x231803Df809C207FaA330646BB5547fD087FEcA1
```

**Thanks for your support!**

# Support

[![Email](https://img.shields.io/badge/-gmail-090909?style=for-the-badge&logo=gmail)](https://mail.google.com/mail/?view=cm&fs=1&to=misster.veit@gmail.com)
