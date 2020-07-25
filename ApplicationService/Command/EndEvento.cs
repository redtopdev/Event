// <copyright file="EndEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.ApplicationService.Command
{
    public class EndEvento : BaseCommand
    {
        public EndEvento(Guid eventoId)
            : base(eventoId)
        {
        }
    }
}
