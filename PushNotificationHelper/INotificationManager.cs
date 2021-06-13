// <copyright file="INotificationManager.cs" company="RedTop">
// RedTop
// </copyright>

using Engaze.Core.DataContract;
using Engaze.Event.Domain.Model;

namespace PushNotificationHelper
{
    public interface INotificationManager
    {
        System.Threading.Tasks.Task NotifyParticipantsAsync(Notification.EventWithUserIds @event, OccuredEventType eventType);
    }
}
