// <copyright file="AggregateRoot.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace Engaze.EventSourcing.Core
{
    public abstract class AggregateRoot : IAggregateRoot, IEventSourcingAggregate
    {
        public const long NewAggregateVersion = -1;

        private readonly Dictionary<Type, Action<IDomainEvent>> handlers = new Dictionary<Type, Action<IDomainEvent>>();

        private readonly ICollection<IDomainEvent> uncommittedEvents = new LinkedList<IDomainEvent>();

        private long version = NewAggregateVersion;

        public Guid Id { get; protected set; }

        long IEventSourcingAggregate.Version => version;

        protected void Register<T>(Action<T> when)
        {
            handlers.Add(typeof(T), @event => when((T)@event));
        }

        protected void RaiseEvent<TEvent>(TEvent @event)
            where TEvent : EventBase
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            handlers[@event.GetType()](@event);
            IDomainEvent eventWithAggrgate = @event.WithAggregate(@event.AggregateId, version);
            uncommittedEvents.Add(eventWithAggrgate);
            this.version = version + 1;
        }

        void IEventSourcingAggregate.ApplyEvent(IDomainEvent @event, long version)
        {
            RaiseEvent((dynamic)@event);
        }

        IEnumerable<IDomainEvent> IEventSourcingAggregate.GetUncommittedEvents()
        {
            return uncommittedEvents.AsEnumerable();
        }

        void IEventSourcingAggregate.ClearUncommittedEvents()
        {
            uncommittedEvents.Clear();
        }
    }
}
