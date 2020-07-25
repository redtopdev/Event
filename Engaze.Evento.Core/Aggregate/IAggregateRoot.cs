
namespace Engaze.Evento.Core
{
    using System;
    using System.Collections.Generic;
    public interface IAggregateRoot
    {
        Guid Id { get; set; }
        long Version { get; }
        void ClearEvents();
        List<IDomainEvent> GetEvents();
        void Apply(IDomainEvent e);

    }
}
