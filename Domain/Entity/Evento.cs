// <copyright file="Evento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Engaze.Core.DataContract;
using Engaze.Event.Domain.Core.Aggregate;
using Engaze.Event.Domain.Event;
using Newtonsoft.Json;

namespace Engaze.Event.Domain.Entity
{
    public class Evento : AggregateRoot
    {
        public Evento(Guid id, Engaze.Core.DataContract.Event eventoContract)
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

        public ICollection<Participant> Participants { get; private set; }

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

        public void UpdateParticipantState(Guid participantId, EventAcceptanceStatus newStatus)
        {
            var @event = new ParticipantStateUpdated(Id, participantId, newStatus);
            RaiseEvent(@event);
        }

        // domain event handler
        private void When(EventoEnded e)
        {
            EndTime = e.EndTime;
            Id = e.AggregateId;
        }

        private void When(ParticipantStateUpdated e)
        {
            Participants.Where(p => p.UserId == e.ParticipantId).FirstOrDefault().UpdateAcceptanceState(e.NewStatus);
            Id = e.AggregateId;
        }

        private void When(ParticipantLeft e)
        {
            Participants.Remove(Participants.Where(p => p.UserId == e.ParticipantId).FirstOrDefault());
            Id = e.AggregateId;
        }

        private void When(EventoExtended e)
        {
            EndTime = e.EndTime;
            Id = e.AggregateId;
        }

        private void When(EventoDeleted e)
        {
            IsDeleted = true;
        }

        private void When(ParticipantsListUpdated e)
        {
            Participants.ToList().RemoveAll(participant => !e.ParticipantList.Contains(participant.UserId));
            e.ParticipantList.ToList().Except(Participants.Select(p => p.UserId)).ToList()
                .ForEach(p => Participants.Add(new Participant(p, EventAcceptanceStatus.Pending)));
            Id = e.AggregateId;
        }

        // domain event handler
        private void When(EventoCreated e)
        {
            Id = e.AggregateId;
            InitiatorId = e.InitiatorId;
            InitiatorName = e.InitiatorName;
            Description = e.Description;
            Name = e.Name;
            StartTime = e.StartTime;
            EndTime = e.EndTime;
            EventState = e.EventState;
            EventType = e.EventType;

            if (e.Participants != null)
            {
                Participants = JsonConvert.DeserializeObject<List<Participant>>(JsonConvert.SerializeObject(e.Participants));
            }

            if (e.Destination != null)
            {
                Destination = JsonConvert.DeserializeObject<ValueObjects.Location>(JsonConvert.SerializeObject(e.Destination));
            }

            if (e.Recurrence != null)
            {
                Recurrence = JsonConvert.DeserializeObject<ValueObjects.Recurrence>(JsonConvert.SerializeObject(e.Recurrence));
            }

            if (e.Duration != null)
            {
                Duration = JsonConvert.DeserializeObject<ValueObjects.Duration>(JsonConvert.SerializeObject(e.Duration));
            }

            if (e.Tracking != null)
            {
                Tracking = JsonConvert.DeserializeObject<ValueObjects.Duration>(JsonConvert.SerializeObject(e.Tracking));
            }

            if (e.Reminder != null)
            {
                Reminder = JsonConvert.DeserializeObject<ValueObjects.Reminder>(JsonConvert.SerializeObject(e.Reminder));
            }
        }
    }
}
