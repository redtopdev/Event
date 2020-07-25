// <copyright file="UpdateParticipantState.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Core.DataContract;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
    public class UpdateParticipantState : BaseCommand
    {
        public UpdateParticipantState(Guid eventoId, Guid participantId, EventAcceptanceStatus status)
            : base(eventoId)
        {
            this.ParticipantId = participantId;
            this.Status = status;
        }

        public Guid ParticipantId { get; private set; }

        public EventAcceptanceStatus Status { get; private set; }
    }
}
