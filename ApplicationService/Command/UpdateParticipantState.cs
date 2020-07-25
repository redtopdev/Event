// <copyright file="UpdateParticipantState.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Core.DataContract;
using Engaze.EventSourcing.Core;

namespace Evento.ApplicationService.Command
{
    public class UpdateParticipantState : BaseCommand
    {
        public UpdateParticipantState(Guid eventoId, Guid participantId, EventAcceptanceState state)
            : base(eventoId)
        {
            this.ParticipantId = participantId;
            this.State = state;
        }

        public Guid ParticipantId { get; private set; }

        public EventAcceptanceState State { get; private set; }
    }
}
