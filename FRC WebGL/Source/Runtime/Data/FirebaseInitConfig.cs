using System;

namespace FRCWebGL.Data
{
    [Serializable]
    public sealed class FirebaseInitConfig
    {
        public string apiKey;
        public string authDomain;
        public string projectId;
        public string appId;

        public string? storageBucket;
        public string? messagingSenderId;

        public long? minFetchDelayPerMillis;
        public long? fetchTimeoutMillis;
    }
}