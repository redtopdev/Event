// <copyright file="IEventSourcingAggregate.cs" company="RedTop">
// RedTop
// </copyright>

namespace Engaze.EventSourcing.Core
{
    using System.Collections.Generic;

    public interface IEventSourcingAggregate
    {
        long Version { get; }

        void ApplyEvent(IDomainEvent @event, long version);

        IEnumerable<IDomainEvent> GetUncommittedEvents();

        void ClearUncommittedEvents();
    }
}
