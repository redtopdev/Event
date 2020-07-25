// <copyright file="EventRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Engaze.Core.Persistance.Cassandra;
using Newtonsoft.Json;
using DataContract = Engaze.Core.DataContract;

namespace Engaze.Event.DataPersistence.Cassandra
{
    public abstract class EventRepository : IEventRepository
    {
        protected EventRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandraConfig)
        {
            if (cassandraConfig == null)
            {
                throw new ArgumentNullException(nameof(cassandraConfig));
            }

            this.SessionCacheManager = sessionCacheManager;
            this.KeySpace = cassandraConfig.KeySpace;
        }

        protected CassandraSessionCacheManager SessionCacheManager { get; private set; }

        protected string KeySpace { get; private set; }        

        public async Task<DataContract.Event> GetEvent(Guid eventId)
        {
            var query = "SELECT EventDetails from EventData WHERE EventId=" + eventId.ToString() + ";";
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var preparedStatement = await sessionL.PrepareAsync(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return JsonConvert.DeserializeObject<DataContract.Event>(resultSet.First().GetValue<string>("eventdetails"));
        }
    }
}
