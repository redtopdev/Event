// <copyright file="EventoCommandHandlerNoEventSourcing.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Event.ApplicationService.Command;
using Engaze.Event.ApplicationService.Core.Handler;
using Engaze.Event.DataPersistence.Cassandra;
using Engaze.Event.Domain.Entity;
using Engaze.Event.Domain.Model;
using Newtonsoft.Json;
using PushNotificationHelper;

namespace Engaze.Event.ApplicationService.Handler
{
    public class EventoCommandHandlerNoEventSourcing : CommandHandler<Evento>
    {
        INotificationManager notificationManager;

        public EventoCommandHandlerNoEventSourcing(IEventCommandRepository nonEvenSourceRepository,  INotificationManager notificationMgr)
            : base(nonEvenSourceRepository)
        {
            Register<CreateEvento>(ProcessCommand);
            Register<UpdateDestination>(ProcessCommand);
            Register<EndEvento>(ProcessCommand);
            Register<LeaveEvento>(ProcessCommand);
            Register<DeleteEvento>(ProcessCommand);
            Register<ExtendEvento>(ProcessCommand);
            Register<UpdateEvento>(ProcessCommand);
            Register<UpdateParticipantList>(ProcessCommand);
            Register<UpdateParticipantState>(ProcessCommand);
            this.notificationManager = notificationMgr;
        }

        protected async Task ProcessCommand(CreateEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var engazeEvent = new Evento(command.Id, command.EventoContract);
            await NonEventSourceRepository.InsertAsync(engazeEvent);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)command.EventoContract, Engaze.Core.DataContract.OccuredEventType.EventoCreated);
        }

        protected async Task ProcessCommand(UpdateEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var engazeEvent = new Evento(command.Id, command.EventoContract);
            await NonEventSourceRepository.UpdateEvent(engazeEvent);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)command.EventoContract, Engaze.Core.DataContract.OccuredEventType.EventoUpdated);
        }

        protected async Task ProcessCommand(UpdateDestination command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.UpdateDestination(command.Location);
            await NonEventSourceRepository.UpdateEvent(evento);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)evento.ToDataContractEvent(), Engaze.Core.DataContract.OccuredEventType.DestinationUpdated);
        }

        protected async Task ProcessCommand(EndEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.EndEvent();
            await NonEventSourceRepository.UpdateEvent(evento);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)evento.ToDataContractEvent(), Engaze.Core.DataContract.OccuredEventType.EventoEnded);
        }

        protected async Task ProcessCommand(LeaveEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.LeaveEvento(command.ParticipantId);
            await NonEventSourceRepository.UpdateEvent(evento, true);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)evento.ToDataContractEvent(), Engaze.Core.DataContract.OccuredEventType.ParticipantLeft);
        }

        protected async Task ProcessCommand(ExtendEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.ExtendEvento(command.ExtendedTime);
            await NonEventSourceRepository.UpdateEvent(evento);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)evento.ToDataContractEvent(), Engaze.Core.DataContract.OccuredEventType.EventoExtended);
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
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)evento.ToDataContractEvent(), Engaze.Core.DataContract.OccuredEventType.EventoDeleted);
        }

        protected async Task ProcessCommand(UpdateParticipantList command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.UpdateParticipantList(command.ParticipantList);
            await NonEventSourceRepository.UpdateEvent(evento, true);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)evento.ToDataContractEvent(), Engaze.Core.DataContract.OccuredEventType.ParticipantsListUpdated);
        }

        protected async Task ProcessCommand(UpdateParticipantState command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = (await NonEventSourceRepository.GetEvent(command.Id)).ToDomainEvent();
            evento.UpdateParticipantState(command.ParticipantId, command.Status);
            await NonEventSourceRepository.UpdateEvent(evento);
            await notificationManager.NotifyParticipantsAsync((Notification.EventWithUserIds)evento.ToDataContractEvent(), Engaze.Core.DataContract.OccuredEventType.ParticipantStateUpdated);
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
