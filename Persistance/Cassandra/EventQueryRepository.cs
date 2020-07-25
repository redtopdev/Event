// <copyright file="EventQueryRepository.cs" company="RedTop">
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

namespace Evento.DataPersistance.Cassandra
{
    public class EventQueryRepository : EventRepository, IEventQueryRepository
    {
        public EventQueryRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandrConfig)
            : base(sessionCacheManager, cassandrConfig)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsByUserId(Guid userid)
        {
            return await GetEventsByEventIds(await GetEventIdsByUserId(userid));
        }

        public async Task<IEnumerable<Event>> GetRunningEventsByUserId(Guid userid)
        {
            return await GetRunningEventsByEventIds(await GetEventIdsByUserId(userid));
        }

        private async Task<IEnumerable<Guid>> GetEventIdsByUserId(Guid userid)
        {
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            string query = "SELECT EventId FROM EventParticipantMapping WHERE UserId=" + userid.ToString() + ";";
            var preparedStatement = sessionL.Prepare(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => row.GetValue<Guid>("eventid"));
        }

        private async Task<IEnumerable<Event>> GetEventsByEventIds(IEnumerable<Guid> eventIds)
        {
            string datetimeUtcIso8601 = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
            string query = "SELECT EventDetails FROM EventData WHERE EventId IN (" + string.Join(",", eventIds.ToList()) + ") " +
                "and EndTime > '" + datetimeUtcIso8601 + "' ALLOW FILTERING;";
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var preparedStatement = sessionL.Prepare(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => JsonConvert.DeserializeObject<Event>(row.GetValue<string>("eventdetails")));
        }

        private async Task<IEnumerable<Event>> GetRunningEventsByEventIds(IEnumerable<Guid> eventIds)
        {
            string datetimeUtcIso8601 = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
            string query = "SELECT EventDetails FROM EventData WHERE EventId IN (" + string.Join(",", eventIds.ToList()) + ") " +
                "and StartTime >= '" + datetimeUtcIso8601 +
                "and EndTime > '" + datetimeUtcIso8601 + "' ALLOW FILTERING;";
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var preparedStatement = sessionL.Prepare(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => JsonConvert.DeserializeObject<Event>(row.GetValue<string>("eventdetails")));
        }
    }
}
