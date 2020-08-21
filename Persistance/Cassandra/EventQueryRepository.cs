// <copyright file="EventQueryRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engaze.Core.Persistance.Cassandra;
using DataContract = Engaze.Core.DataContract;

namespace Engaze.Event.DataPersistence.Cassandra
{
    public class EventQueryRepository : EventRepository, IEventQueryRepository
    {
        public EventQueryRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandraConfig)
            : base(sessionCacheManager, cassandraConfig)
        {
        }

        public async Task<IEnumerable<DataContract.Event>> GetEventsByUserId(Guid userid)
            => await GetEventsByEventIds(await GetEventIdsByUserId(userid));

        public async Task<IEnumerable<DataContract.Event>> GetRunningEventsByUserId(Guid userid)
            => await GetRunningEventsByEventIds(await GetEventIdsByUserId(userid));

        private async Task<IEnumerable<Guid>> GetEventIdsByUserId(Guid userid)
        {
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var query = "select eventid from UserEvent where userid=" + userid.ToString() + ";";
            var preparedStatement = await sessionL.PrepareAsync(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return resultSet.Select(row => row.GetValue<Guid>("eventid"));
        }

        private async Task<IEnumerable<DataContract.Event>> GetEventsByEventIds(IEnumerable<Guid> eventIds)
        {
            List<DataContract.Event> evnts = new List<DataContract.Event>();
            foreach (Guid eventId in eventIds)
            {
                var evnt = await GetEvent(eventId);
                if (evnt.EndTime > DateTime.UtcNow)
                {
                    evnts.Add(evnt);
                }
            }

            return evnts;
        }

        private async Task<IEnumerable<DataContract.Event>> GetRunningEventsByEventIds(IEnumerable<Guid> eventIds)
        {
            List<DataContract.Event> evnts = new List<DataContract.Event>();

            foreach (Guid eventId in eventIds)
            {
                var evnt = await GetEvent(eventId);
                if (evnt.EndTime > DateTime.UtcNow && evnt.StartTime >= DateTime.UtcNow)
                {
                    evnts.Add(evnt);
                }
            }

            return evnts;
        }
    }
}
