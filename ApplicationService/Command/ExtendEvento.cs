// <copyright file="ExtendEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.ApplicationService.Command
{
    public class ExtendEvento : BaseCommand
    {
        public ExtendEvento(Guid eventoId, DateTime extendedTime)
            : base(eventoId)
        {
            this.ExtendedTime = extendedTime;
        }

        public DateTime ExtendedTime { get; private set; }
    }
}
