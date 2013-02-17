using System;
using System.Collections.Generic;
using System.Reflection;
using ShopNow.Events;

namespace ShopNow.Services
{
    public class ServiceLocator
    {
        private static Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        static ServiceLocator()
        {
            _singletons.Add(typeof(IEventAggregator), new EventAggregator());
            _singletons.Add(typeof(PushNotificationRegistrationService), new PushNotificationRegistrationService());
            _singletons.Add(typeof(TileUpdaterService), new TileUpdaterService());
        }

        /// <summary>
        /// A very simple ServiceLocator implementation.  If a singleton exists in the map return it, otherwise return a new instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            if (_singletons.ContainsKey(typeof(T)))
            {
                return (T)_singletons[typeof (T)];
            }
            return CreateInstance<T>();
        }

        private static T CreateInstance<T>()
        {
            var typeInfo = typeof (T).GetTypeInfo();
            if (typeInfo.IsInterface)
            {
                throw new ArgumentException("Unable to create a new instance of an interface type.  This simple ServiceLocator mapping interfaces as Singleton types.");
            }
            return Activator.CreateInstance<T>();
        }
    }
}
