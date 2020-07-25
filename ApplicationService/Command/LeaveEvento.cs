// <copyright file="LeaveEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
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
