// <copyright file="IEventQueryManager.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataContract = Engaze.Core.DataContract;

namespace Engaze.Event.ApplicationService.Query
{
    public interface IEventQueryManager
    {
        Task<DataContract.Event> GetEvent(Guid eventId);
       
        Task<IEnumerable<DataContract.Event>> GetEventsByUserId(Guid userId);
       
        Task<IEnumerable<DataContract.Event>> GetRunningEventsByUserId(Guid userId);
    }
}
