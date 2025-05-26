using System;

namespace FRCWebGL.Data
{
    [Serializable]
    public sealed class FirebaseInitConfig
    {
        public string apiKey { get; set; }
        public string authDomain { get; set; }
        public string projectId { get; set; }
        public string appId { get; set; }

        public string? storageBucket { get; set; }
        public string? messagingSenderId { get; set; }

        public long? minFetchDelayPerMillis { get; set; }
        public long? fetchTimeoutMillis { get; set; }
    }
}