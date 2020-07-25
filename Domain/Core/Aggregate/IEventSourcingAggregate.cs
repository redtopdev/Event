// <copyright file="IEventSourcingAggregate.cs" company="RedTop">
// RedTop
// </copyright>

using System.Collections.Generic;
using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.Domain.Core.Aggregate
{
    public interface IEventSourcingAggregate
    {
        long Version { get; }

        void ApplyEvent(IDomainEvent @event, long version);

        IEnumerable<IDomainEvent> GetUncommittedEvents();

        void ClearUncommittedEvents();
    }
}
