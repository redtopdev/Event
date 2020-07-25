// <copyright file="EventWrapper.cs" company="RedTop">
// RedTop
// </copyright>

using Engaze.EventSourcing.Core;

namespace Evento.DataPersistance
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
