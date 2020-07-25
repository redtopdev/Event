// <copyright file="EventoCreated.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Engaze.Core.DataContract;
using Engaze.EventSourcing.Core;
using Newtonsoft.Json;

namespace Evento.Domain.Event
{
    public class EventoCreated : EventBase
    {
        public EventoCreated(Guid aggregateId, Engaze.Core.DataContract.Event eventoContract)
            : base(aggregateId)
        {
            if (eventoContract == null)
            {
                throw new ArgumentNullException(nameof(eventoContract));
            }

            this.Name = eventoContract.Name;
            this.EventType = eventoContract.EventType;
            this.Description = eventoContract.Description;
            this.InitiatorId = eventoContract.InitiatorId;
            this.InitiatorName = eventoContract.InitiatorName;
            this.StartTime = eventoContract.StartTime;
            this.EndTime = eventoContract.EndTime;
            if (eventoContract.Participants != null)
            {
                this.Participants = new List<Participant>();
                eventoContract.Participants.ToList().ForEach(participant => this.Participants.Add(new Participant(participant.UserId, EventAcceptanceState.Pending)));
            }

            if (eventoContract.Destination != null)
            {
                this.Destination = JsonConvert.DeserializeObject<Location>(JsonConvert.SerializeObject(eventoContract.Destination));
            }

            if (eventoContract.Duration != null)
            {
                this.Duration = JsonConvert.DeserializeObject<Duration>(JsonConvert.SerializeObject(eventoContract.Duration));
            }

            if (eventoContract.Tracking != null)
            {
                this.Tracking = JsonConvert.DeserializeObject<Duration>(JsonConvert.SerializeObject(eventoContract.Tracking));
            }

            if (eventoContract.Reminder != null)
            {
                this.Reminder = JsonConvert.DeserializeObject<Reminder>(JsonConvert.SerializeObject(eventoContract.Reminder));
            }

            if (eventoContract.Recurrence != null)
            {
                this.Recurrence = JsonConvert.DeserializeObject<Recurrence>(JsonConvert.SerializeObject(eventoContract.Recurrence));
            }

            this.EventState = eventoContract.EventState;
        }

        public string Name { get; private set; }

        public EventType EventType { get; private set; }

        public string Description { get; private set; }

        public Guid InitiatorId { get; private set; }

        public string InitiatorName { get; private set; }

        public EventState EventState { get; private set; }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public Location Destination { get; private set; }

        public List<Participant> Participants { get; private set; }

        public Recurrence Recurrence { get; private set; }

        public Duration Duration { get; private set; }

        public Reminder Reminder { get; private set; }

        public Duration Tracking { get; private set; }
    }
}
