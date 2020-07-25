// <copyright file="IEventRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using DataContract = Engaze.Core.DataContract;

namespace Engaze.Event.DataPersistence.Cassandra
{
    public interface IEventRepository
    {
        Task<DataContract.Event> GetEvent(Guid eventId);
    }
}
