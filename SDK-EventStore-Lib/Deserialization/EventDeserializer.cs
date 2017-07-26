using System;
using System.IO;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using SDKClassicalLib.Events;

namespace SDK_EventStore_Lib.Deserialization
{
    internal static class EventDeserializer
    {
        public static T Deserialize<T>(ResolvedEvent resolvedEvent) where T : class
        {
            var genType = typeof(T);
            var eventType = DeserializeMetadata(resolvedEvent).EventType;
            var resEventType = Type.GetType(eventType, false);
            if (genType != resEventType)
            {
                return null;
            }

            var resolvedEventData = resolvedEvent.Event.Data;

            return DeserializeBytes<T>(resolvedEventData);
        }

        private static Metadata DeserializeMetadata(ResolvedEvent resolvedEvent)
        {
            var metadata = resolvedEvent.Event.Metadata;

            return DeserializeBytes<Metadata>(metadata);
        }

        private static T DeserializeBytes<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new StreamReader(stream))
                {
                    return JsonSerializer.Create().Deserialize(reader, typeof(T)) as T;
                }
            }

        }

    }
}

