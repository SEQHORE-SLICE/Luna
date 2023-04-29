using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
namespace Framework
{
    public static class Explorer
    {

        private static IEnumerable<IService> services => Boot.SystemServices;
        private static readonly Dictionary<Type, IService> ServicesCache = new Dictionary<Type, IService>();

        public static T TryGetService<T>()
        {
            var serviceType = typeof(T);
            if (ServicesCache.TryGetValue(serviceType, out var targetsValue))
            {
                return (T)targetsValue;
            }
            foreach (var service in services.Where(service => service.GetType() == serviceType))
            {
                ServicesCache.Add(service.GetType(), service);
                return (T)service;
            }
            throw new NullReferenceException();
        }

    }
}
