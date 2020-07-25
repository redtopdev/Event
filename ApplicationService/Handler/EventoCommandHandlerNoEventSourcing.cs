// <copyright file="EventoCommandHandlerNoEventSourcing.cs" company="RedTop">
// RedTop
// </copyright>

namespace Evento.ApplicationService.Handler
{
    using System;
    using System.Threading.Tasks;
    using Engaze.Core.DataContract;
    using Engaze.EventSourcing.Core;
    using Evento.ApplicationService.Command;
    using Evento.DataPersistance;
    using Evento.Domain.Entity;
    using Newtonsoft.Json;

    public class EventoCommandHandlerNoEventSourcing : CommandHandler<Evento>
    {
        public EventoCommandHandlerNoEventSourcing(IEventCommandRepository nonEvenSourceRepository)
            : base(nonEvenSourceRepository)
        {
            Register<CreateEvento>(ProcessCommand);
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
            await NonEventSourceRepository.InsertAsync(engazeEvent.ToDataContractEvent());
        }

        protected async Task ProcessCommand(EndEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.EndEvent();
            await NonEventSourceRepository.UpdateEventEndDate(evento.Id, evento.EndTime);
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
            await NonEventSourceRepository.SaveEvent(evento.ToDataContractEvent());
        }

        protected async Task ProcessCommand(UpdateParticipantState command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.UpdateParticipantState(command.ParticipantId, command.State);
            await NonEventSourceRepository.SaveEvent(evento.ToDataContractEvent());
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "not needed as seperate class")]
    public static class DataContractConverter
    {
        public static Event ToDataContractEvent(this Evento evento)
            => JsonConvert.DeserializeObject<Event>(JsonConvert.SerializeObject(evento));

        public static Evento ToDomainEvent(this Event evnt)
            => JsonConvert.DeserializeObject<Evento>(JsonConvert.SerializeObject(evnt));
    }
}
