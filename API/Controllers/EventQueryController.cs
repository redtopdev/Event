// <copyright file="EventQueryController.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Event.ApplicationService.Query;
using Microsoft.AspNetCore.Mvc;

namespace Engaze.Event.Service.Controllers
{
    [ApiController]
    public class EventQueryController : ControllerBase
    {
        private readonly IEventQueryManager eventQueryManager;

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