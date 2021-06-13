// <copyright file="Notification.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Engaze.Event.Domain.Model
{
    public class Notification
    {
        public Engaze.Core.DataContract.OccuredEventType NotificationType { get; set; }

        public EventWithUserIds Event { get; set; }

        public class EventWithUserIds : Engaze.Core.DataContract.Event
        {
            public ICollection<Guid> AddedUserIds { get; set; }

            public ICollection<Guid> RemovedUserIds { get; set; }

            public int ExtendDuration { get; set; }

            public Guid? RequesterId { get; set; }

            public string RequesterName { get; set; }

            public Engaze.Core.DataContract.EventAcceptanceStatus EventAcceptanceState { get; set; }
        }
    }
}
