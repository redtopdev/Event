// <copyright file="EventQueryRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Engaze.Core.Persistance.Cassandra;
using Newtonsoft.Json;
using DataContract= Engaze.Core.DataContract;

namespace Engaze.Event.DataPersistence.Cassandra
{
    public class EventQueryRepository : EventRepository, IEventQueryRepository
    {
        public EventQueryRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandraConfig)
            : base(sessionCacheManager, cassandraConfig)
        {
        }

        public async Task<IEnumerable<DataContract.Event>> GetEventsByUserId(Guid userid)
        {
            return await GetEventsByEventIds(await GetEventIdsByUserId(userid));
        }

        public async Task<IEnumerable<DataContract.Event>> GetRunningEventsByUserId(Guid userid)
        {
            return await GetRunningEventsByEventIds(await GetEventIdsByUserId(userid));
        }

        private async Task<IEnumerable<Guid>> GetEventIdsByUserId(Guid userid)
        {
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var query = "SELECT EventId FROM EventParticipantMapping WHERE UserId=" + userid.ToString() + ";";
            var preparedStatement = await sessionL.PrepareAsync(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => row.GetValue<Guid>("eventid"));
        }

        private async Task<IEnumerable<DataContract.Event>> GetEventsByEventIds(IEnumerable<Guid> eventIds)
        {
            var datetimeUtcIso8601 = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
            var query = "SELECT EventDetails FROM EventData WHERE EventId IN (" + string.Join(",", eventIds.ToList()) + ") " +
                        "and EndTime > '" + datetimeUtcIso8601 + "' ALLOW FILTERING;";
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var preparedStatement = await sessionL.PrepareAsync(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => JsonConvert.DeserializeObject<DataContract.Event>(row.GetValue<string>("eventdetails")));
        }

        private async Task<IEnumerable<DataContract.Event>> GetRunningEventsByEventIds(IEnumerable<Guid> eventIds)
        {
            var datetimeUtcIso8601 = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
            var query = "SELECT EventDetails FROM EventData WHERE EventId IN (" + string.Join(",", eventIds.ToList()) + ") " +
                        "and StartTime >= '" + datetimeUtcIso8601 +
                        "and EndTime > '" + datetimeUtcIso8601 + "' ALLOW FILTERING;";
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var preparedStatement = await sessionL.PrepareAsync(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => JsonConvert.DeserializeObject<DataContract.Event>(row.GetValue<string>("eventdetails")));
        }
    }
}
