// <copyright file="IEventRepository.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Threading.Tasks;
using Engaze.Core.DataContract;

namespace Evento.DataPersistance
{
    public interface IEventRepository
    {
        Task<Event> GetEvent(Guid eventId);
    }
}
