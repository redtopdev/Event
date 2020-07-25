// <copyright file="ParticipantLeft.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.Domain.Event
{
    public class ParticipantLeft : EventBase
    {
        public ParticipantLeft(Guid id, Guid participantId)
           : base(id) => this.ParticipantId = participantId;

        public Guid ParticipantId { get; set; }
    }
}
