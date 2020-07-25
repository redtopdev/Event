

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engaze.Evento.Core
{
    public abstract class CommandHandler<TCommand> where TCommand : class, ICommand
    {
        protected readonly IAggregateRespository<IAggregateRoot> repository;

        private readonly Dictionary<Type, Action<ICommand>> _handlers = new Dictionary<Type, Action<ICommand>>();
        public CommandHandler(IAggregateRespository<IAggregateRoot> aggregateRespository)
        {
            this.repository = aggregateRespository;
        }
        public virtual async Task Handle(TCommand command)
        {
            Validate(command);
            await ProcessCommand(command);           
        }
        public virtual bool Validate(TCommand command)
        {
            return true;
        }

        abstract protected Task ProcessCommand(TCommand command);

    }
}
