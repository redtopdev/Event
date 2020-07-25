// <copyright file="AggregateRespository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Engaze.Event.Domain.Core.Aggregate;

namespace Engaze.Event.DataPersistence.EventStore
{
    public class AggregateRespository<TAggregate> : IAggregateRespository<TAggregate>
        where TAggregate : class, IEventSourcingAggregate
    {
        private readonly IEventStore eventStore;

        public AggregateRespository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task<TAggregate> Get(Guid id)
        {
            try
            {
                var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
                var events = await eventStore.ReadEventsAsync($"{typeof(TAggregate).Name}-{id.ToString()}");
                events.ToList().ForEach(@event => aggregate.ApplyEvent(@event.DomainEvent, @event.EventNumber));
                aggregate.ClearUncommittedEvents();
                return aggregate;
            }
            catch (EventStoreAggregateNotFoundException)
            {
                throw;
            }
            catch (EventStoreCommunicationException)
            {
                // throw new RepositoryException("Unable to access persistence layer", ex);
                throw;
            }
        }

        public async Task Save(TAggregate aggregate)
        {
            try
            {
                foreach (var @event in aggregate?.GetUncommittedEvents())
                {
                    await eventStore.AppendEventAsync(@event);

                    // await publisher.PublishAsync((dynamic)@event);
                }

                aggregate.ClearUncommittedEvents();
            }
            catch (EventStoreCommunicationException)
            {
                // throw new RepositoryException("Unable to access persistence layer", ex);
                throw;
            }
        }
    }
}
