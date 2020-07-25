// <copyright file="EventStoreEventStore.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Engaze.EventSourcing.Core;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using Newtonsoft.Json;

namespace Evento.DataPersistance
{
    public class EventStoreEventStore : IEventStore
    {
        private readonly IEventStoreConnection connection;

        public EventStoreEventStore(IEventStoreConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<EventWrapper>> ReadEventsAsync(string id)
        {
            try
            {
                var ret = new List<EventWrapper>();
                StreamEventsSlice currentSlice;
                long nextSliceStart = StreamPosition.Start;

                do
                {
                    currentSlice = await connection.ReadStreamEventsForwardAsync(id.ToString(), nextSliceStart, 200, false);
                    if (currentSlice.Status != SliceReadStatus.Success)
                    {
                        throw new EventStoreAggregateNotFoundException($"Aggregate {id.ToString()} not found");
                    }

                    nextSliceStart = currentSlice.NextEventNumber;
                    foreach (var resolvedEvent in currentSlice.Events)
                    {
                        ret.Add(new EventWrapper(Deserialize(resolvedEvent.Event.EventType, resolvedEvent.Event.Data), resolvedEvent.Event.EventNumber));
                    }
                } 
                while (!currentSlice.IsEndOfStream);

                return ret;
            }
            catch (EventStoreConnectionException ex)
            {
                throw new EventStoreCommunicationException($"Error while reading events for aggregate {id}", ex);
            }
        }

        public async Task<AppendResult> AppendEventAsync(IDomainEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            try
            {
                var eventData = new EventData(
                    @event.EventId,
                    @event.GetType().AssemblyQualifiedName,
                    true,
                    Serialize(@event),
                    Encoding.UTF8.GetBytes("{}"));

                var writeResult = await connection.AppendToStreamAsync(
                    @event.AggregateIdAsString,
                    @event.AggregateVersion == AggregateRoot.NewAggregateVersion ? ExpectedVersion.NoStream : @event.AggregateVersion,
                    eventData);

                return new AppendResult(writeResult.NextExpectedVersion);
            }
            catch (EventStoreConnectionException ex)
            {
                throw new EventStoreCommunicationException($"Error while appending event {@event.EventId} for aggregate {@event.AggregateId}", ex);
            }
        }

        private IDomainEvent Deserialize(string eventType, byte[] data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            return (IDomainEvent)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType(eventType), settings);
        }

        private byte[] Serialize(IDomainEvent @event)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
        }
    }
}