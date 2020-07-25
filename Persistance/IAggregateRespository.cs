// <copyright file="IAggregateRespository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Event.Domain.Core.Aggregate;

namespace Engaze.Event.DataPersistence
{
    public interface IAggregateRespository<TAggregate>
        where TAggregate : IEventSourcingAggregate
    {
        Task Save(TAggregate aggregate);

        Task<TAggregate> Get(Guid id);
    }
}
