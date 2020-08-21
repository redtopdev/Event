// <copyright file="EventCommandRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Engaze.Core.Persistance.Cassandra;
using Engaze.Event.Domain.Entity;
using Newtonsoft.Json;

namespace Engaze.Event.DataPersistence.Cassandra
{
    public class EventCommandRepository : EventRepository, IEventCommandRepository
    {
        public EventCommandRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandraConfig)
            : base(sessionCacheManager, cassandraConfig)
        {
        }

        public async Task InsertAsync(Evento @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            await InsertAsyncEventData(@event);
            if (@event.Participants?.Any() ?? false)
            {
                await InsertEventParticipantMapping(@event.Id, @event.Participants.Select(participant => participant.UserId));
            }
        }

        public async Task DeleteAsync(Guid eventId)
        {
            var existingParticipants = (await GetEvent(eventId)).Participants?.Select(participant => participant.UserId);
            await DeleteAsyncEventData(eventId);
            await DeleteEventParticipantMapping(eventId, existingParticipants);
        }

        public async Task UpdateEvent(Evento @event, bool updateParticipants = false)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (updateParticipants)
            {
                await UpdateParticipants(@event);
            }

            var updateParticipantStateStatement = $"UPDATE EventData SET StartTime = '{@event.StartTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}',EndTime='{@event.EndTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}', EventDetails = '" + JsonConvert.SerializeObject(@event) + "' WHERE EventID=" + @event.Id + ";";
            var session = SessionCacheManager.GetSession(KeySpace);
            await session.ExecuteAsync((await session.PrepareAsync(updateParticipantStateStatement)).Bind());
        }

        private async Task UpdateParticipants(Evento @event)
        {
            var existingParticipants = (await GetEvent(@event.Id)).Participants?.Select(participant => participant.UserId);
            var newParticipants = @event.Participants?.Select(participant => participant.UserId);
            if (newParticipants.Any() && existingParticipants.Any())
            {
                var insertList = newParticipants.Except(existingParticipants);
                if (insertList.Any())
                {
                    await InsertEventParticipantMapping(@event.Id, insertList);
                }

                var deleteList = existingParticipants.Except(newParticipants);
                if (deleteList.Any())
                {
                    await DeleteEventParticipantMapping(@event.Id, deleteList);
                }
            }

            if (newParticipants.Any())
            {
                await InsertEventParticipantMapping(@event.Id, newParticipants);
            }
            else
            {
                await DeleteEventParticipantMapping(@event.Id, existingParticipants);
            }
        }

        private async Task InsertAsyncEventData(Evento @event)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            var eventJson = JsonConvert.SerializeObject(@event);
            var insertEventData =
                $"INSERT INTO EventData (EventId, StartTime, EndTime, EventDetails) values ({@event.Id},'{@event.StartTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}','{@event.EndTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}','{eventJson}');";
            var ips = await session.PrepareAsync(insertEventData);
            var statement = ips.Bind();
            await session.ExecuteAsync(statement);
        }

        private async Task InsertEventParticipantMapping(Guid eventId, IEnumerable<Guid> participantIds)
        {
            var session = SessionCacheManager.GetSession(KeySpace);

            participantIds?.ToList().ForEach(async participantId =>
            {
                var insertEventParticipantMapping = $"INSERT INTO UserEvent (UserId ,EventId) values ({participantId},{eventId});";
                var ips = await session.PrepareAsync(insertEventParticipantMapping);
                var statement = ips.Bind();
                await session.ExecuteAsync(statement);
            });
        }

        private async Task DeleteAsyncEventData(Guid eventId)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            var eventDeleteStatement = $"Delete from EventData where EventId={eventId};";
            await session.ExecuteAsync((await session.PrepareAsync(eventDeleteStatement)).Bind());
        }

        private async Task DeleteEventParticipantMapping(Guid eventId, IEnumerable<Guid> participantIds)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            participantIds?.ToList().ForEach(async participant =>
            {
                var deleteEventParticipantMappings = $"Delete from UserEvent where UserId ={participant} AND EventId={eventId};";
                var ips = await session.PrepareAsync(deleteEventParticipantMappings);
                var statement = ips.Bind();
                await session.ExecuteAsync(statement);
            });
        }
    }
}
