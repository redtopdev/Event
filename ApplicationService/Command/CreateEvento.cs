// <copyright file="CreateEvento.cs" company="RedTop">
// RedTop
// </copyright>

namespace Evento.ApplicationService.Command
{
    using System;
    using Engaze.EventSourcing.Core;

    public class CreateEvento : BaseCommand
    {
        public CreateEvento(Guid eventId, Engaze.Core.DataContract.Event eventoContract)
            : base(eventId)
        {
            this.EventoContract = eventoContract;
        }

        public Engaze.Core.DataContract.Event EventoContract { get; private set; }
    }
}