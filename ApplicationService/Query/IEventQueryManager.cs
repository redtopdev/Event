// <copyright file="IEventQueryManager.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engaze.Core.DataContract;

namespace EventQuery.Service
{
    public interface IEventQueryManager
    {
        Task<Event> GetEvent(Guid eventid);
       
        Task<IEnumerable<Event>> GetEventsByUserId(Guid userId);
       
        Task<IEnumerable<Event>> GetRunningEventsByUserId(Guid userId);
    }
}
