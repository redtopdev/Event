// <copyright file="EventQueryManager.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engaze.Core.DataContract;
using Evento.DataPersistance.Cassandra;

namespace EventQuery.Service
{
    public class EventQueryManager : IEventQueryManager
    {
        private IEventQueryRepository repo;

        public EventQueryManager(IEventQueryRepository repo)
        {
            this.repo = repo;
        }
        
        public async Task<Event> GetEvent(Guid eventId)
        {
            return await repo.GetEvent(eventId);
        }
      
        public async Task<IEnumerable<Event>> GetEventsByUserId(Guid userId)
        {
            return await repo.GetEventsByUserId(userId);
        }

        public async Task<IEnumerable<Event>> GetRunningEventsByUserId(Guid userId)
        {
            return await repo.GetRunningEventsByUserId(userId);
        }
    }
}
