// <copyright file="ChangeEventoLocation.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.ApplicationService.Command
{
    public class ChangeEventoLocation : BaseCommand
    {
        public ChangeEventoLocation(Guid eventoId)
            : base(eventoId)
        {
        }
    }
}
