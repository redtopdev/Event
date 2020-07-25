// <copyright file="DeleteEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.ApplicationService.Command
{
    public class DeleteEvento : BaseCommand
    {
        public DeleteEvento(Guid eventoId)
            : base(eventoId)
        {
        }
    }
}
