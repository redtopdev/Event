// <copyright file="UpdateParticipantList.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using Engaze.EventSourcing.Core;

namespace Evento.ApplicationService.Command
{
    public class UpdateParticipantList : BaseCommand
    {
        public UpdateParticipantList(Guid eventoId, ICollection<Guid> participantList)
            : base(eventoId)
        {
            this.ParticipantList = participantList;
        }

        public ICollection<Guid> ParticipantList { get; private set; }
    }
}
