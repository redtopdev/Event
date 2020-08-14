// <copyright file="UpdateEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
    public class UpdateEvento : BaseCommand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "not needed")]
        public UpdateEvento(Engaze.Core.DataContract.Event eventoContract)
            : base(eventoContract.EventId)
        {
            this.EventoContract = eventoContract;
        }

        public Engaze.Core.DataContract.Event EventoContract { get; private set; }
    }
}