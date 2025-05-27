using FRCWebGL.Utils;

namespace FRCWebGL.Core.Components
{
    public sealed class RemoteConfigSettings
    {
        private readonly long _minFetchDelayPerMillis;
        private readonly long _fetchTimeoutMillis;

        public long? MinFetchDelayPerMillis => _minFetchDelayPerMillis;
        public long? FetchTimeoutMillis => _fetchTimeoutMillis;

        public RemoteConfigSettings(
            long? minFetchDelayPerMillis, long? fetchTimeoutMillis)
        {
            if (minFetchDelayPerMillis <= 0)
            {
                minFetchDelayPerMillis = 60000;

                FRCWebLogger.LogWarning("The min fetch delay cannot be zero or low");
            }

            if (fetchTimeoutMillis <= 0)
            {
                fetchTimeoutMillis = 60000;

                FRCWebLogger.LogWarning("The fetch timeout cannot be zero or low");
            }

            _minFetchDelayPerMillis = (long)minFetchDelayPerMillis;
            _fetchTimeoutMillis = (long)fetchTimeoutMillis;
        }
    }
}