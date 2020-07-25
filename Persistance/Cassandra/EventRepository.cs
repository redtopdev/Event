// <copyright file="EventRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Engaze.Core.DataContract;
using Engaze.Core.Persistance.Cassandra;
using Newtonsoft.Json;

namespace Evento.DataPersistance
{
    public abstract class EventRepository : IEventRepository
    {
        public EventRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandrConfig)
        {
            if (cassandrConfig == null)
            {
                throw new ArgumentNullException(nameof(cassandrConfig));
            }

            this.SessionCacheManager = sessionCacheManager;
            this.KeySpace = cassandrConfig.KeySpace;
        }

        protected CassandraSessionCacheManager SessionCacheManager { get; private set; }

        protected string KeySpace { get; private set; }        

        public async Task<Event> GetEvent(Guid eventId)
        {
            string query = "SELECT EventDetails from EventData WHERE EventId=" + eventId.ToString() + ";";
            var sessionL = SessionCacheManager.GetSession(KeySpace);
            var preparedStatement = sessionL.Prepare(query);
            var resultSet = await sessionL.ExecuteAsync(preparedStatement.Bind());
            return JsonConvert.DeserializeObject<Event>(resultSet.First().GetValue<string>("eventdetails"));
        }
    }
}
