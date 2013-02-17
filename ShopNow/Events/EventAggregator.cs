using System;
using System.Collections.Generic;

namespace ShopNow.Events
{
    /// <summary>
    /// This is an over-simplified implementation of an event aggregator. This does not take into account multi-treading or numerous other concerns.
    /// </summary>
    class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<WeakReference>> _subscribers = new Dictionary<Type, List<WeakReference>>();

        public void Subscribe<T>(Action<T> callback) where T : IEvent
        {
            var eventType = typeof (T);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers.Add(eventType, new List<WeakReference>());
            }
            _subscribers[eventType].Add(new WeakReference(callback));
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            if (!_subscribers.ContainsKey(typeof (T)))
            {
                // nobody to publish to.
                return;
            }

            var eventSubscribers = _subscribers[typeof (T)];
            foreach (var subscriber in eventSubscribers)
            {
                if (subscriber.IsAlive)
                {
                    var callback = (Action<T>) subscriber.Target;
                    callback(@event);
                }
            }
        }
    }
}
