// <copyright file="EventoDeleted.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.Domain.Core.Event;

namespace Engaze.Event.Domain.Event
{
    public class EventoDeleted : EventBase
    {
        public EventoDeleted(Guid id)
            : base(id)
        {
        }
    }
}
