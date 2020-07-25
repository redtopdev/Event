// <copyright file="IEventQueryRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataContract = Engaze.Core.DataContract;

namespace Engaze.Event.DataPersistence.Cassandra
{
    public interface IEventQueryRepository : IEventRepository
    {
        Task<IEnumerable<DataContract.Event>> GetEventsByUserId(Guid userid);

        Task<IEnumerable<DataContract.Event>> GetRunningEventsByUserId(Guid userid);
    }
}