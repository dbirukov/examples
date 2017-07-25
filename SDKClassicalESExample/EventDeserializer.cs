using System;
using System.IO;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace SDKClassicalESExample
{
    internal static class EventDeserializer
    {
        public static T Deserialize<T>(ResolvedEvent resolvedEvent) where T : class
        {
            var genType = typeof(T);
            var eventType = resolvedEvent.Event.EventType; //make sense to move this information to metadata
            var resEventType = Type.GetType(eventType, false);
            if (genType != resEventType)
            {
                return null;
            }

            var resolvedEventData = resolvedEvent.Event.Data;
            using (var stream = new MemoryStream(resolvedEventData))
            {
                using (var reader = new StreamReader(stream))
                {
                    return JsonSerializer.Create().Deserialize(reader, typeof(T)) as T;
                }
            }
        }
    }
}

