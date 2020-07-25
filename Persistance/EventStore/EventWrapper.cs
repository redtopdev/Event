// <copyright file="EventWrapper.cs" company="RedTop">
// RedTop
// </copyright>

using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.DataPersistence.EventStore
{
    public class EventWrapper
    {
        public EventWrapper(IDomainEvent domainEvent, long eventNumber)
        {
            DomainEvent = domainEvent;
            EventNumber = eventNumber;
        }

        public long EventNumber { get; }

        public IDomainEvent DomainEvent { get; }
    }
}
