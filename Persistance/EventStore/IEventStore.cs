// <copyright file="IEventStore.cs" company="RedTop">
// RedTop
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Engaze.EventSourcing.Core;

namespace Evento.DataPersistance
{
    public interface IEventStore
    {
        Task<IEnumerable<EventWrapper>> ReadEventsAsync(string id);

        Task<AppendResult> AppendEventAsync(IDomainEvent @event);
    }
}
