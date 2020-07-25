// <copyright file="ServiceControllerBase.cs" company="RedTop">
// RedTop
// </copyright>

using Engaze.EventSourcing.Core;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Service
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
