// <copyright file="ParticipantsListUpdated.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using Engaze.EventSourcing.Core;

namespace Evento.Domain.Event
{
    public class ParticipantsListUpdated : EventBase
    {
        public ParticipantsListUpdated(Guid id, ICollection<Guid> participantList)
            : base(id) => this.ParticipantList = participantList;

        public ICollection<Guid> ParticipantList { get; set; }
    }
}
