// <copyright file="CreateEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
    public class UpdateEvent : BaseCommand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "not needed")]
        public UpdateEvent(Engaze.Core.DataContract.Event @event)
            : base(@event.EventId)
        {
            this.EventContract = @event;
        }

        public Engaze.Core.DataContract.Event EventContract { get; private set; }
    }
}