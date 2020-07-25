// <copyright file="EventoEnded.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.Domain.Event
{
    public class EventoEnded : EventBase
    {
        public EventoEnded(Guid id, DateTime endTime)
            : base(id) => this.EndTime = endTime;

        public DateTime EndTime { get; set; }
    }
}
