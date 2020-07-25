// <copyright file="ICommandDispatcher.cs" company="RedTop">
// RedTop
// </copyright>

using System.Threading.Tasks;
using Engaze.Event.ApplicationService.Core.Command;
using Engaze.Event.ApplicationService.Core.Handler;
using Engaze.Event.Domain.Core.Aggregate;

namespace Engaze.Event.ApplicationService.Core.Dispatcher
{
    public interface ICommandDispatcher
    {
        void Register<TAggregate>(CommandHandler<TAggregate> handler)
            where TAggregate : IEventSourcingAggregate;

        Task Dispatch<TAggregate>(ICommand command)
            where TAggregate : IEventSourcingAggregate;
    }
}
