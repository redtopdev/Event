// <copyright file="IEventStore.cs" company="RedTop">
// RedTop
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.DataPersistence.EventStore
{
    public interface IEventStore
    {
        Task<IEnumerable<EventWrapper>> ReadEventsAsync(string id);

        Task<AppendResult> AppendEventAsync(IDomainEvent @event);
    }
}
