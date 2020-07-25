// <copyright file="ParticipantController.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engaze.Core.DataContract;
using Engaze.Event.ApplicationService.Command;
using Engaze.Event.ApplicationService.Core.Dispatcher;
using Engaze.Event.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Engaze.Event.Service.Controllers
{
    public class ParticipantController : ServiceControllerBase
    {
        public ParticipantController(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }

        [HttpPut(Routes.EventoParticipants)]
        public async Task<IActionResult> UpdateParticipantListAsync([FromRoute]Guid eventId, ICollection<Guid> participants)
        {
            await CommandDispatcher.Dispatch<Domain.Entity.Evento>(new UpdateParticipantList(eventId, participants));
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPut(Routes.LeaveEvento)]
        public async Task<IActionResult> LeaveEvent([FromRoute]Guid eventId, [FromRoute]Guid participantId)
        {
            await CommandDispatcher.Dispatch<Evento>(new LeaveEvento(eventId, participantId));
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPut(Routes.EventoParticipantState)]
        public async Task<IActionResult> UpdateParticipantState([FromRoute]Guid eventId, [FromRoute]Guid participantId, [FromRoute]EventAcceptanceStatus status)
        {
            await CommandDispatcher.Dispatch<Domain.Entity.Evento>(new UpdateParticipantState(eventId, participantId, status));
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }
    }
}