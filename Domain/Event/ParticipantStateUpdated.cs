// <copyright file="ParticipantStateUpdated.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Core.DataContract;
using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.Domain.Event
{
    public class ParticipantStateUpdated : EventBase
    {
        public ParticipantStateUpdated(Guid id, Guid participantId, EventAcceptanceStatus newStatus)
            : base(id)
        {
            this.ParticipantId = participantId;
            this.NewStatus = newStatus;
        }

        public Guid ParticipantId { get; set; }

        public EventAcceptanceStatus NewStatus { get; set; }
    }
}
