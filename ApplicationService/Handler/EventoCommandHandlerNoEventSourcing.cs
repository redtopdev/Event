// <copyright file="EventoCommandHandlerNoEventSourcing.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Event.ApplicationService.Command;
using Engaze.Event.ApplicationService.Core.Handler;
using Engaze.Event.DataPersistence.Cassandra;
using Engaze.Event.Domain.Entity;
using Newtonsoft.Json;

namespace Engaze.Event.ApplicationService.Handler
{
    public class EventoCommandHandlerNoEventSourcing : CommandHandler<Evento>
    {
        public EventoCommandHandlerNoEventSourcing(IEventCommandRepository nonEvenSourceRepository)
            : base(nonEvenSourceRepository)
        {
            Register<CreateEvento>(ProcessCommand);
            Register<UpdateEvent>(ProcessCommand);
            Register<EndEvento>(ProcessCommand);
            Register<LeaveEvento>(ProcessCommand);
            Register<DeleteEvento>(ProcessCommand);
            Register<ExtendEvento>(ProcessCommand);
            Register<UpdateParticipantList>(ProcessCommand);
            Register<UpdateParticipantState>(ProcessCommand);
        }

        protected async Task ProcessCommand(CreateEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var engazeEvent = new Evento(command.Id, command.EventoContract);
            await NonEventSourceRepository.InsertAsync(engazeEvent);
        }

        protected async Task ProcessCommand(UpdateEvent command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var engazeEvent = new Evento(command.Id, command.EventContract);
            await NonEventSourceRepository.UpdateEventAsync(engazeEvent);
        }

        protected async Task ProcessCommand(EndEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.EndEvent();
            await NonEventSourceRepository.UpdateEventEndDate(evento.Id, DateTime.UtcNow);
        }

        protected async Task ProcessCommand(LeaveEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.LeaveEvento(command.ParticipantId);
            await NonEventSourceRepository.LeaveEvent(evento.Id, command.ParticipantId);
        }

        protected async Task ProcessCommand(ExtendEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.ExtendEvento(command.ExtendedTime);
            await NonEventSourceRepository.UpdateEventEndDate(evento.Id, evento.EndTime);
        }

        protected async Task ProcessCommand(DeleteEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.DeleteEvent();
            await NonEventSourceRepository.DeleteAsync(evento.Id);
        }

        protected async Task ProcessCommand(UpdateParticipantList command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.UpdateParticipantList(command.ParticipantList);
            await NonEventSourceRepository.SaveEvent(evento);
        }

        protected async Task ProcessCommand(UpdateParticipantState command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.UpdateParticipantState(command.ParticipantId, command.Status);
            await NonEventSourceRepository.SaveEvent(evento);
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "not needed as seperate class")]
    public static class DataContractConverter
    {
        public static Engaze.Core.DataContract.Event ToDataContractEvent(this Evento evento)
            => JsonConvert.DeserializeObject<Engaze.Core.DataContract.Event>(JsonConvert.SerializeObject(evento));

        public static Evento ToDomainEvent(this Engaze.Core.DataContract.Event evnt)
            => JsonConvert.DeserializeObject<Evento>(JsonConvert.SerializeObject(evnt));
    }
}
