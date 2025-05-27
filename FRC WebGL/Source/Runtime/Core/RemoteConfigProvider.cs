using FRCWebGL.Core.Base;
using FRCWebGL.Infrastructure;

namespace FRCWebGL.Core
{
    public static class RemoteConfigProvider
    {
        private static IRemoteConfigService _instance;

        public static IRemoteConfigService Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                ServiceLocator.Bind<IRemoteConfigService>(new RemoteConfigService());

                _instance = ServiceLocator.Get<IRemoteConfigService>();

                return _instance;
            }
        }
    }
}