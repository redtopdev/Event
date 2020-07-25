// <copyright file="EventCommandRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Engaze.Core.DataContract;
using Engaze.Core.Persistance.Cassandra;
using Newtonsoft.Json;

namespace Evento.DataPersistance
{
    public class EventCommandRepository : EventRepository, IEventCommandRepository
    {
        public EventCommandRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandrConfig)
            : base(sessionCacheManager, cassandrConfig)
        {
        }

        public async Task InsertAsync(Event @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            await InsertAsyncEventData(@event);
            InsertEventParticipantMapping(@event);
        }

        private async Task InsertAsyncEventData(Event @event)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            string eventJson = JsonConvert.SerializeObject(@event);
            string insertEventData = "INSERT INTO EventData " +
            "(EventId, StartTime, EndTime, EventDetails)" +
            "values " +
            "(" + @event.EventId + "," + @event.StartTime + "," + @event.EndTime + ",'" + eventJson + "');";
            var ips = session.Prepare(insertEventData);
            var statement = ips.Bind();
            await session.ExecuteAsync(statement);
        }

        private void InsertEventParticipantMapping(Event @event)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            List<Guid> participantList = GetEventParticipantsList(@event).ToList();
            participantList.ForEach(async participant =>
            {
                string insertEventParticipantMapping = "INSERT INTO EventParticipantMapping " +
                         "(UserId ,EventId)" +
                        "values " +
                        "(" + participant + "," + @event.EventId + ");";
                var ips = session.Prepare(insertEventParticipantMapping);
                string eventJson = JsonConvert.SerializeObject(@event);
                var statement = ips.Bind();
                await session.ExecuteAsync(statement);
            });
        }

        public async Task DeleteAsync(Guid eventId)
        {
            await DeleteAsyncEventData(eventId);
            DeleteEventParticipantMapping(eventId);
        }

        public async Task DeleteAsyncEventData(Guid eventId)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            string eventDeleteStatement = "Delete from EventData where EventId=" + eventId + ";";
            await session.ExecuteAsync(session.Prepare(eventDeleteStatement).Bind(eventId));
        }

        public void DeleteEventParticipantMapping(Guid eventId)
        {
            List<Guid> participantList = GetEventParticipantsList(eventId).ToList();
            var session = SessionCacheManager.GetSession(KeySpace);
            participantList.ForEach(async participant =>
            {
                string deleteEventParticipantMappings = "Delete from EventParticipantMapping where UserId = " + participant + " AND EventId=" + eventId + ";";
                var ips = session.Prepare(deleteEventParticipantMappings);
                var statement = ips.Bind(participant, eventId);
                await session.ExecuteAsync(statement);
            });
        }

        private IEnumerable<Guid> GetEventParticipantsList(Guid eventId)
        {
            var session = SessionCacheManager.GetSession(KeySpace);
            string query = "SELECT UserId FROM EventParticipantMapping WHERE EventId=" + eventId.ToString() + "ALLOW FILTERING;";
            var preparedStatement = session.Prepare(query);
            var resultSet = session.Execute(preparedStatement.Bind());
            return resultSet.Select(row => row.GetValue<Guid>("userid"));
        }

        public async Task UpdateEventEndDate(Guid eventId, DateTime endTime)
        {
            string datetimeUtcIso8601 = endTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
            string extendEventStatement = "UPDATE EventData SET endtime = '" + datetimeUtcIso8601 + "' WHERE EventID=" + eventId + ";";
            var session = SessionCacheManager.GetSession(KeySpace);
            await session.ExecuteAsync(session.Prepare(extendEventStatement).Bind());
        }

        public async Task SaveEvent(Event @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            string updateParticipantStateStatement = "UPDATE EventData SET EventDetails = '" + JsonConvert.SerializeObject(@event) + "' WHERE EventID=" + @event.EventId + ";";
            var session = SessionCacheManager.GetSession(KeySpace);
            await session.ExecuteAsync(session.Prepare(updateParticipantStateStatement).Bind());
        }

        private IEnumerable<Guid> GetEventParticipantsList(Event @event)
        {
            IEnumerable<Guid> participantsList = @event.Participants.Select(x => x.UserId).Append(@event.InitiatorId);
            return participantsList.Distinct();
        }

        public Task LeaveEvent(Guid id, Guid participantId)
        {
            throw new NotImplementedException();
        }
    }
}
