// <copyright file="CreateEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
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