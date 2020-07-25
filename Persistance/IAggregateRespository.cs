// <copyright file="IAggregateRespository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.EventSourcing.Core;

namespace Evento.DataPersistance
{
    public interface IAggregateRespository<TAggregate>
        where TAggregate : IEventSourcingAggregate
    {
        Task Save(TAggregate aggregate);

        Task<TAggregate> Get(Guid id);
    }
}
