// <copyright file="Participant.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Core.DataContract;

namespace Evento.Domain.Entity
{
    public class Participant
    {
        public Participant()
        {
        }

        internal Participant(Guid userId, EventAcceptanceState acceptanceState)
        {
            this.UserId = userId;
            this.AcceptanceState = acceptanceState;
        }

        public Guid UserId { get; private set; }

        public EventAcceptanceState AcceptanceState { get; private set; }

        public void UpdateAcceptanceState(EventAcceptanceState acceptanceState)
        {
            this.AcceptanceState = acceptanceState;
        }
    }
}
