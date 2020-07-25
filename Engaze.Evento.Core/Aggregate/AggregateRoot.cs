
using System;
using System.Collections.Generic;


namespace Engaze.Evento.Domain
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public const long NewAggregateVersion = -1;

        private long version = NewAggregateVersion;

        private readonly Dictionary<Type, Action<IDomainEvent>> handlers = new Dictionary<Type, Action<IDomainEvent>>();

        private readonly List<IDomainEvent> events = new List<IDomainEvent>();

        public Guid Id  { get; set; }
        long IAggregateRoot.Version => version;

        void IAggregateRoot.Apply(IDomainEvent e)
        {
            Raise(e);
            this.version++;
        }

        void IAggregateRoot.ClearEvents()
        {
            events.Clear();
        }

        List<IDomainEvent> IAggregateRoot.GetEvents()
        {
            return events;
        }

        protected void Register<T>(Action<T> when)
        {
            handlers.Add(typeof(T), e => when((T)e));
        }

        protected void Raise(IDomainEvent e)
        {
            handlers[e.GetType()](e);            
            events.Add(e);
        }
    }
}
