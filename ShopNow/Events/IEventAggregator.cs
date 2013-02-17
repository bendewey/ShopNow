using System;

namespace ShopNow.Events
{
    public interface IEventAggregator
    {
        void Subscribe<T>(Action<T> callback) where T : IEvent;
        void Publish<T>(T @event) where T : IEvent;
    }
}