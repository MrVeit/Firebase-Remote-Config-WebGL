using System;
using System.Collections.Generic;

namespace FRCWebGL.Infrastructure
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Bind<T>(T service) where T : class
        {
            var type = typeof(T);

            if (_services.ContainsKey(type))
            {
                _services[type] = service;

                return;
            }

            _services.Add(type, service);
        }

        public static void Dispose<T>() where T : class
        {
            var type = typeof(T);

            if (!_services.ContainsKey(type))
            {
                return;
            }

            _services.Remove(type);
        }

        public static T Get<T>() where T : class
        {
            if (_services.TryGetValue(
                typeof(T), out object service))
            {
                return service as T;
            }

            throw new InvalidOperationException(
                $"Service for type {typeof(T)} not binded");
        }
    }
}