// <copyright file="Participant.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Core.DataContract;

namespace Engaze.Event.Domain.Entity
{
    public class Participant
    {
        public Participant()
        {
        }

        internal Participant(Guid userId, EventAcceptanceStatus acceptanceStatus)
        {
            this.UserId = userId;
            this.AcceptanceStatus = acceptanceStatus;
        }

        public Guid UserId { get; set; }

        public EventAcceptanceStatus AcceptanceStatus { get; set; }

        public void UpdateAcceptanceState(EventAcceptanceStatus acceptanceStatus)
        {
            this.AcceptanceStatus = acceptanceStatus;
        }
    }
}
