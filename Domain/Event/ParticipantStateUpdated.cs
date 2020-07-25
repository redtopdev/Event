// <copyright file="ParticipantStateUpdated.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Core.DataContract;
using Engaze.EventSourcing.Core;

namespace Evento.Domain.Event
{
    public class ParticipantStateUpdated : EventBase
    {
        public ParticipantStateUpdated(Guid id, Guid participantId, EventAcceptanceState newState)
            : base(id)
        {
            this.ParticipantId = participantId;
            this.NewState = newState;
        }

        public Guid ParticipantId { get; set; }

        public EventAcceptanceState NewState { get; set; }
    }
}
