using System;
using EventStore.ClientAPI;

namespace EventStore.Reactive
{
    public static class EventStoreConnectionExtensions
    {
        public static IObservable<T> CreateSubscriptionObservable<T>(this IEventStoreConnection connection,
            bool resolveLinkTos) where T : class
        {
            return new SubscriptionObservable<T>(connection, resolveLinkTos);
        }

        public static IObservable<T> CreateStreamSubscriptionObservable<T>(this IEventStoreConnection connection,
            string streamName, bool resolveLinkTos) where T : class
        {
            return new SubscriptionStreamObservable<T>(connection, resolveLinkTos, streamName);
        }

        public static IObservable<T> CreateCatchUpSubscriptionObservable<T>(this IEventStoreConnection connection,
            Position? lastPosition, bool resolveLinkTos, Action<Position?> setLastPosition) where T : class
        {
            return new CatchUpSubscriptionObservable<T>(connection, lastPosition, resolveLinkTos, setLastPosition);
        }

        public static IObservable<T> CreateStreamCatchUpSubscriptionObservable<T>(this IEventStoreConnection connection,
            string streamName, int? lastPosition, bool resolveLinkTos, Action<long> setLastPosition) where T : class
        {
            return new CatchUpSubscriptionStreamObservable<T>(connection, lastPosition, resolveLinkTos, streamName,
                setLastPosition);
        }
    }
}