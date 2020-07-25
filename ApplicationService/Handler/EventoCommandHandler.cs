// <copyright file="EventoCommandHandler.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Event.ApplicationService.Command;
using Engaze.Event.ApplicationService.Core.Handler;
using Engaze.Event.DataPersistence;
using Engaze.Event.Domain.Entity;

namespace Engaze.Event.ApplicationService.Handler
{
    public class EventoCommandHandler : CommandHandler<Evento>
    {
        public EventoCommandHandler(IAggregateRespository<Evento> aggregateRespository)
            : base(aggregateRespository)
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
            await Repository.Save(engazeEvent);
        }

        protected async Task ProcessCommand(EndEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = await Repository.Get(command.Id) as Evento;
            evento.EndEvent();
            await Repository.Save(evento);
        }

        protected async Task ProcessCommand(LeaveEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = await Repository.Get(command.Id) as Evento;
            evento.LeaveEvento(command.ParticipantId);
            await Repository.Save(evento);
        }

        protected async Task ProcessCommand(ExtendEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = await Repository.Get(command.Id) as Evento;
            evento.ExtendEvento(command.ExtendedTime);
            await Repository.Save(evento);
        }

        protected async Task ProcessCommand(DeleteEvento command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = await Repository.Get(command.Id) as Evento;
            evento.DeleteEvent();
            await Repository.Save(evento);
        }

        protected async Task ProcessCommand(UpdateParticipantList command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = await Repository.Get(command.Id) as Evento;
            evento.UpdateParticipantList(command.ParticipantList);
            await Repository.Save(evento);
        }

        protected async Task ProcessCommand(UpdateParticipantState command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var evento = await Repository.Get(command.Id) as Evento;
            evento.UpdateParticipantState(command.ParticipantId, command.Status);
            await Repository.Save(evento);
        }
    }
}
