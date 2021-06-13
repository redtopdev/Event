// <copyright file="NotificationManager.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Engaze.Core.DataContract;
using Engaze.Event.Domain.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace PushNotificationHelper
{
    public class NotificationManager : INotificationManager
    {
        private IConfiguration configuration;

        public NotificationManager(IConfiguration config)
        {
            this.configuration = config;
        }

        public async System.Threading.Tasks.Task NotifyParticipantsAsync(Notification.EventWithUserIds @event, OccuredEventType eventType)
        {
            var notificationUrl = configuration["PushNotificationSettings:NotificationUrl"];
            var notification = new Notification()
            { 
                NotificationType = eventType,
                Event = @event
            };
            
            using (var client = new HttpClient())
            {

                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                string requestBody = JsonConvert.SerializeObject(notification);

                var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
                try
                {
                    var response = await client.PostAsync(notificationUrl, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var stringData = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception("Notification failed.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Notification failed." + ex);
                }
                finally 
                {
                    httpContent.Dispose();
                }
            }
        }
    }
}
