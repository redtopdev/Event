// <copyright file="EventQueryController.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using EventQuery.Service;
using Microsoft.AspNetCore.Mvc;

namespace Evento.Service.Controllers
{
    [ApiController]
    public class EventQueryController : ControllerBase
    {
        private IEventQueryManager eventQueryManager;

        public EventQueryController(IEventQueryManager eventManager)
        {
            this.eventQueryManager = eventManager;
        }

        [HttpGet("events/{eventId:guid}")]
        public async Task<IActionResult> Get(Guid eventId)
        {
            return Ok(await eventQueryManager.GetEvent(eventId));
        }

        [HttpGet("events/user/{userId:guid}")]
        public async Task<IActionResult> GetEventsByUserId(Guid userId)
        {
            return Ok(await eventQueryManager.GetEventsByUserId(userId));
        }

        [HttpGet("events/running/user/{userId:guid}")]
        public async Task<IActionResult> GetRunningEventsByUserId(Guid userId)
        {
            return Ok(await eventQueryManager.GetRunningEventsByUserId(userId));
        }
    }
}