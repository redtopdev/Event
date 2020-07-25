// <copyright file="EventRecurrance.cs" company="RedTop">
// RedTop
// </copyright>

using System;

namespace Engaze.Event.Domain.Entity
{
    public class EventRecurrance
    {
        public Guid EventId { get; set; }

        public int RecurrenceFrequencyTypeId { get; set; }

        public int RecurrenceCount { get; set; }

        public int RecurrenceFrequency { get; set; }

        public string RecurrenceDaysOfWeek { get; set; }
    }
}
