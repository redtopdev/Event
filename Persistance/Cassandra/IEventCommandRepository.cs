// <copyright file="IEventCommandRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Event.Domain.Entity;

namespace Engaze.Event.DataPersistence.Cassandra
{
    public interface IEventCommandRepository : IEventRepository
    {
        Task InsertAsync(Evento @event);

        Task DeleteAsync(Guid eventId);

        Task UpdateEvent(Evento @event, bool updateParticipants = false);        
    }
}
