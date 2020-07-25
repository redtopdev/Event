// <copyright file="LeaveEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.ApplicationService.Command
{
    public class LeaveEvento : BaseCommand
    {
        public LeaveEvento(Guid eventoId, Guid participantId)
            : base(eventoId)
        {
            this.ParticipantId = participantId;
        }

        public Guid ParticipantId { get; private set; }
    }
}
