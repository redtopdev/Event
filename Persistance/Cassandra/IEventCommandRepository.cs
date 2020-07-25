// <copyright file="IEventCommandRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Core.DataContract;

namespace Evento.DataPersistance
{
    public interface IEventCommandRepository : IEventRepository
    {
        Task InsertAsync(Event @event);

        Task DeleteAsync(Guid eventId);

        Task UpdateEventEndDate(Guid eventId, DateTime endTime);

        Task LeaveEvent(Guid id, Guid participantId);

        Task SaveEvent(Event @event);
    }
}
