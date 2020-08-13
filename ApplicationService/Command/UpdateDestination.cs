// <copyright file="CreateEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
    public class UpdateDestination : BaseCommand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "not needed")]
        public UpdateDestination(Guid eventoId, Engaze.Core.DataContract.Location location)
            : base(eventoId)
        {
            this.Location = location;
        }

        public Engaze.Core.DataContract.Location Location { get; private set; }
    }
}