// <copyright file="EventCommandController.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Data.Common;
using System.Threading.Tasks;
using Engaze.Event.ApplicationService.Command;
using Engaze.Event.ApplicationService.Core.Dispatcher;
using Engaze.Event.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Engaze.Event.Service.Controllers
{
    public class EventCommandController : ServiceControllerBase
    {
        public EventCommandController(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }

        [HttpPost(Routes.Evento)]
        public async Task<IActionResult> CreateEventAsync([FromBody]Engaze.Core.DataContract.Event @event)
        {
            var eventId = Guid.NewGuid();
            await CommandDispatcher.Dispatch<Evento>(new CreateEvento(eventId, @event));
            return new ObjectResult(new { id = eventId }) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut(Routes.EndEvento)]
        public async Task<IActionResult> EndEventAsync([FromRoute]Guid eventId)
        {
            await CommandDispatcher.Dispatch<Evento>(new EndEvento(eventId));
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPut(Routes.Evento)]
        public async Task<IActionResult>UpdateEventAsync([FromBody]Engaze.Core.DataContract.Event evento)
        {
            await CommandDispatcher.Dispatch<Evento>(new UpdateEvent(evento));
            return new OkResult();
        }

        [HttpPut(Routes.ExtendEvento)]
        public async Task<IActionResult> ExtendEventAsync([FromRoute]Guid eventId, [FromRoute]DateTime endTime)
        {
            await CommandDispatcher.Dispatch<Evento>(new ExtendEvento(eventId, endTime));
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpDelete(Routes.DeleteEvento)]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid eventId)
        {
            await CommandDispatcher.Dispatch<Evento>(new DeleteEvento(eventId));
            return new StatusCodeResult(StatusCodes.Status202Accepted);
        }
    }
}