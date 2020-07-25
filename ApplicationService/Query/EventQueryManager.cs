// <copyright file="EventQueryManager.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engaze.Event.DataPersistence.Cassandra;
using DataContract = Engaze.Core.DataContract;

namespace Engaze.Event.ApplicationService.Query
{
    public class EventQueryManager : IEventQueryManager
    {
        private readonly IEventQueryRepository repo;

        public EventQueryManager(IEventQueryRepository repo)
        {
            this.repo = repo;
        }
        
        public async Task<DataContract.Event> GetEvent(Guid eventId)
        {
            return await repo.GetEvent(eventId);
        }
      
        public async Task<IEnumerable<DataContract.Event>> GetEventsByUserId(Guid userId)
        {
            return await repo.GetEventsByUserId(userId);
        }

        public async Task<IEnumerable<DataContract.Event>> GetRunningEventsByUserId(Guid userId)
        {
            return await repo.GetRunningEventsByUserId(userId);
        }
    }
}
