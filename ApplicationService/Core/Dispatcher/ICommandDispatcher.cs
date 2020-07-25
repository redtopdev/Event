// <copyright file="ICommandDispatcher.cs" company="RedTop">
// RedTop
// </copyright>

using System.Threading.Tasks;

namespace Engaze.EventSourcing.Core
{
    public interface ICommandDispatcher
    {
        void Register<TAggregate>(CommandHandler<TAggregate> handler)
            where TAggregate : IEventSourcingAggregate;

        Task Dispatch<TAggregate>(ICommand command)
            where TAggregate : IEventSourcingAggregate;
    }
}
