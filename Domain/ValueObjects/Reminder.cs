// <copyright file="Reminder.cs" company="RedTop">
// RedTop
// </copyright>

using System.Collections.Generic;

namespace Evento.Domain.ValueObjects
{
    public class Reminder : ValueObject<Reminder>
    {
        public int TimeInterval { get; set; }

        public string Period { get; set; }

        public string NotificationType { get; set; }

        public long ReminderOffsetInMinute { get; set; }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                TimeInterval, Period, NotificationType, ReminderOffsetInMinute
            };
        }
    }
}
