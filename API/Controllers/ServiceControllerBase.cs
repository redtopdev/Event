// <copyright file="ServiceControllerBase.cs" company="RedTop">
// RedTop
// </copyright>

using Engaze.Event.ApplicationService.Core.Dispatcher;
using Microsoft.AspNetCore.Mvc;

namespace Engaze.Event.Service.Controllers
{
    [ApiController]
    public class ServiceControllerBase : ControllerBase
    {
        public ServiceControllerBase(ICommandDispatcher commandDispatcher)
        {
            this.CommandDispatcher = commandDispatcher;
        }

        protected ICommandDispatcher CommandDispatcher { get; private set; }        
    }
}
