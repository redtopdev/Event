// <copyright file="EventoExtended.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.Domain.Event
{
    public class EventoExtended : EventBase
    {
        public EventoExtended(Guid id, DateTime endTime)
            : base(id) => this.EndTime = endTime;

        public DateTime EndTime { get; set; }
    }
}
