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
            await InsertEventParticipantMapping(@event);
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

        private async Task InsertEventParticipantMapping(Evento @event)
        {
            var session = SessionCacheManager.GetSession(KeySpace);

            var participantList = @event.Participants.Select(prticipant => prticipant.UserId).ToList();
            participantList.Add(@event.InitiatorId);
            participantList.ForEach(async participant =>
            {
                var insertEventParticipantMapping = "INSERT INTO EventParticipantMapping " +
                                                    "(UserId ,EventId)" +
                                                    "values " +
                                                    "(" + participant + "," + @event.Id + ");";
                var ips = await session.PrepareAsync(insertEventParticipantMapping);
                var eventJson = JsonConvert.SerializeObject(@event);
                var statement = ips.Bind();
                await session.ExecuteAsync(statement);
            });
        }

        public async Task DeleteAsync(Guid eventId)
        {
            await DeleteAsyncEventData(eventId);
            await DeleteEventParticipantMapping(eventId);
        }

        public async Task DeleteAsyncEventData(Guid eventId)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            var eventDeleteStatement = "Delete from EventData where EventId=" + eventId + ";";
            await session.ExecuteAsync((await session.PrepareAsync(eventDeleteStatement)).Bind(eventId));
        }

        public async Task DeleteEventParticipantMapping(Guid eventId)
        {
            var participantList = (await GetEventParticipantsList(eventId)).ToList();
            var session = SessionCacheManager.GetSession(KeySpace);
            participantList.ForEach(async participant =>
            {
                var deleteEventParticipantMappings = "Delete from EventParticipantMapping where UserId = " + participant + " AND EventId=" + eventId + ";";
                var ips = await session.PrepareAsync(deleteEventParticipantMappings);
                var statement = ips.Bind(participant, eventId);
                await session.ExecuteAsync(statement);
            });
        }

        private async Task<IEnumerable<Guid>> GetEventParticipantsList(Guid eventId)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            var query = "SELECT UserId FROM EventParticipantMapping WHERE EventId=" + eventId.ToString() + "ALLOW FILTERING;";
            var preparedStatement = await session.PrepareAsync(query);
            var resultSet = await session.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => row.GetValue<Guid>("userid"));
        }

        public async Task UpdateEventEndDate(Guid eventId, DateTime endTime)
        {
            var datetimeUtcIso8601 = endTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
            var extendEventStatement = "UPDATE EventData SET endtime = '" + datetimeUtcIso8601 + "' WHERE EventID=" + eventId + ";";
            var session = SessionCacheManager.GetSession(KeySpace);
            await session.ExecuteAsync((await session.PrepareAsync(extendEventStatement)).Bind());
        }

        public async Task SaveEvent(Evento @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            var updateParticipantStateStatement = "UPDATE EventData SET EventDetails = '" + JsonConvert.SerializeObject(@event) + "' WHERE EventID=" + @event.Id + ";";
            var session = SessionCacheManager.GetSession(KeySpace);
            await session.ExecuteAsync((await session.PrepareAsync(updateParticipantStateStatement)).Bind());
        }

        public Task LeaveEvent(Guid id, Guid participantId)
        {
            throw new NotImplementedException();
        }
    }
}
