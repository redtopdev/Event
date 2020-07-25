// <copyright file="EventoEnded.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.Domain.Event
{
    public class EventoEnded : EventBase
    {
        public EventoEnded(Guid id, DateTime endTime)
            : base(id) => this.EndTime = endTime;

        public DateTime EndTime { get; set; }
    }
}
