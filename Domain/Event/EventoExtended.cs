// <copyright file="EventoExtended.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.EventSourcing.Core;

namespace Evento.Domain.Event
{
    public class EventoExtended : EventBase
    {
        public EventoExtended(Guid id, DateTime endTime)
            : base(id) => this.EndTime = endTime;

        public DateTime EndTime { get; set; }
    }
}
