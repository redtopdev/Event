// <copyright file="IEventQueryRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engaze.Core.DataContract;

namespace Evento.DataPersistance.Cassandra
{
    public interface IEventQueryRepository : IEventRepository
    {
        Task<IEnumerable<Event>> GetEventsByUserId(Guid userid);

        Task<IEnumerable<Event>> GetRunningEventsByUserId(Guid userid);
    }
}