// <copyright file="ParticipantLeft.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.Domain.Event
{
    public class ParticipantLeft : EventBase
    {
        public ParticipantLeft(Guid id, Guid participantId)
           : base(id) => this.ParticipantId = participantId;

        public Guid ParticipantId { get; set; }
    }
}
