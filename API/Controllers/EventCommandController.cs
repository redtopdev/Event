// <copyright file="EventCommandController.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Core.DataContract;
using Engaze.EventSourcing.Core;
using Evento.ApplicationService.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Service
{
    public class EventCommandController : ServiceControllerBase
    {
        public EventCommandController(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }

        [HttpPost(Routes.Evento)]
        public async Task<IActionResult> CreateEventAsync([FromBody]Event evento)
        {
            Guid eventId = Guid.NewGuid();
            await CommandDispatcher.Dispatch<Domain.Entity.Evento>(new CreateEvento(eventId, evento));
            return new ObjectResult(eventId) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut(Routes.EndEvento)]
        public async Task<IActionResult> EndEventAsync([FromRoute]Guid eventId)
        {
            await CommandDispatcher.Dispatch<Domain.Entity.Evento>(new EndEvento(eventId));
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPut(Routes.ExtendEvento)]
        public async Task<IActionResult> ExtendEventAsync([FromRoute]Guid eventId, [FromRoute]DateTime endTime)
        {
            await CommandDispatcher.Dispatch<Domain.Entity.Evento>(new ExtendEvento(eventId, endTime));
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpDelete(Routes.DeleteEvento)]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid eventId)
        {
            await CommandDispatcher.Dispatch<Domain.Entity.Evento>(new DeleteEvento(eventId));
            return new StatusCodeResult(StatusCodes.Status202Accepted);
        }
    }
}