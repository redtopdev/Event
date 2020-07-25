// <copyright file="CommandHandler.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.DataPersistance;

namespace Engaze.EventSourcing.Core
{
    public abstract class CommandHandler<TDomain>
        where TDomain : IEventSourcingAggregate
    {
        public CommandHandler(IAggregateRespository<TDomain> aggregateRespository)
        {
            this.Repository = aggregateRespository;
        }

        public CommandHandler(IEventCommandRepository repository)
        {
            this.NonEventSourceRepository = repository;
        }

        protected IAggregateRespository<TDomain> Repository { get; private set; }

        protected IEventCommandRepository NonEventSourceRepository { get; private set; }

        public Dictionary<Type, Func<BaseCommand, Task>> Handlers { get; private set; } = new Dictionary<Type, Func<BaseCommand, Task>>();

        public virtual bool Validate<TCommand>(TCommand command)
            where TCommand : BaseCommand
        {
            return true;
        }

        protected void Register<TCommand>(Func<TCommand, Task> processCommand)
            where TCommand : BaseCommand
        {
            Handlers.Add(typeof(TCommand), async command =>
            {
                Validate(command);
                await processCommand((TCommand)command);
            });
        }

        public async Task Handle<TCommand>(TCommand command)
            where TCommand : BaseCommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await Handlers[command.GetType()](command);
        }
    }
}