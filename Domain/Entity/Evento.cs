// <copyright file="Evento.cs" company="RedTop">
// RedTop
// </copyright>

namespace Evento.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Engaze.Core.DataContract;
    using Engaze.EventSourcing.Core;
    using global::Evento.Domain.Event;
    using Newtonsoft.Json;

    public class Evento : AggregateRoot
    {
        public Evento(Guid id, Event eventoContract)
            : this()
        {
            var @event = new EventoCreated(id, eventoContract);
            RaiseEvent(@event);
        }

        public Evento()
        {
            // domain events registered (event sourcing pattern)
            Register<EventoCreated>(When);
            Register<EventoEnded>(When);
            Register<ParticipantsListUpdated>(When);
            Register<EventoDeleted>(When);
            Register<EventoExtended>(When);
            Register<ParticipantLeft>(When);
            Register<ParticipantStateUpdated>(When);
        }

        public string Name { get; private set; }

        public EventType EventType { get; private set; }

        public string Description { get; private set; }

        public Guid InitiatorId { get; private set; }

        public string InitiatorName { get; set; }

        public EventState EventState { get; private set; }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public ICollection<Participant> ParticipantList { get; private set; }

        public ValueObjects.Location Destination { get; private set; }

        public ValueObjects.Recurrence Recurrence { get; private set; }

        public ValueObjects.Duration Duration { get; set; }

        public ValueObjects.Duration Tracking { get; set; }

        public ValueObjects.Reminder Reminder { get; set; }

        public bool IsDeleted { get; private set; } = false;

        public void DeleteEvent()
        {
            var @event = new EventoDeleted(Id);
            RaiseEvent(@event);
        }

        public void EndEvent()
        {
            var @event = new EventoEnded(Id, DateTime.UtcNow);
            RaiseEvent(@event);
        }

        public void ExtendEvento(DateTime extendedTime)
        {
            var @event = new EventoExtended(Id, extendedTime);
            RaiseEvent(@event);
        }

        public void LeaveEvento(Guid participantId)
        {
            var @event = new ParticipantLeft(Id, participantId);
            RaiseEvent(@event);
        }

        public void UpdateParticipantList(ICollection<Guid> participantList)
        {
            var @event = new ParticipantsListUpdated(Id, participantList);
            RaiseEvent(@event);
        }

        public void UpdateParticipantState(Guid participantId, EventAcceptanceState newState)
        {
            var @event = new ParticipantStateUpdated(Id, participantId, newState);
            RaiseEvent(@event);
        }

        // domain event handler
        private void When(EventoEnded e)
        {
            this.EndTime = e.EndTime;
            this.Id = e.AggregateId;
        }

        private void When(ParticipantStateUpdated e)
        {
            this.ParticipantList.Where(p => p.UserId == e.ParticipantId).FirstOrDefault().UpdateAcceptanceState(e.NewState);
            this.Id = e.AggregateId;
        }

        private void When(ParticipantLeft e)
        {
            this.ParticipantList.Remove(this.ParticipantList.Where(p => p.UserId == e.ParticipantId).FirstOrDefault());
            this.Id = e.AggregateId;
        }

        private void When(EventoExtended e)
        {
            this.EndTime = e.EndTime;
            this.Id = e.AggregateId;
        }

        private void When(EventoDeleted e)
        {
            this.IsDeleted = true;
        }

        private void When(ParticipantsListUpdated e)
        {
            this.ParticipantList.ToList().RemoveAll(participant => !e.ParticipantList.Contains(participant.UserId));
            e.ParticipantList.ToList().Except(ParticipantList.Select(p => p.UserId)).ToList()
                .ForEach(p => this.ParticipantList.Add(new Participant(p, EventAcceptanceState.Pending)));
            this.Id = e.AggregateId;
        }

        // domain event handler
        private void When(EventoCreated e)
        {
            Id = e.AggregateId;
            this.InitiatorId = e.InitiatorId;
            this.InitiatorName = e.InitiatorName;
            this.Description = e.Description;
            this.Name = e.Name;
            this.StartTime = e.StartTime;
            this.EndTime = e.EndTime;
            this.EventState = e.EventState;
            this.EventType = e.EventType;

            if (e.Participants != null)
            {
                this.ParticipantList = JsonConvert.DeserializeObject<List<Participant>>(JsonConvert.SerializeObject(e.Participants));
            }

            if (e.Destination != null)
            {
                this.Destination = JsonConvert.DeserializeObject<ValueObjects.Location>(JsonConvert.SerializeObject(e.Destination));
            }

            if (e.Recurrence != null)
            {
                this.Recurrence = JsonConvert.DeserializeObject<ValueObjects.Recurrence>(JsonConvert.SerializeObject(e.Recurrence));
            }

            if (e.Duration != null)
            {
                this.Duration = JsonConvert.DeserializeObject<ValueObjects.Duration>(JsonConvert.SerializeObject(e.Duration));
            }

            if (e.Tracking != null)
            {
                this.Tracking = JsonConvert.DeserializeObject<ValueObjects.Duration>(JsonConvert.SerializeObject(e.Tracking));
            }

            if (e.Reminder != null)
            {
                this.Reminder = JsonConvert.DeserializeObject<ValueObjects.Reminder>(JsonConvert.SerializeObject(e.Reminder));
            }
        }
    }
}
