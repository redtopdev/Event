// <copyright file="ChangeEventoLocation.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
    public class ChangeEventoLocation : BaseCommand
    {
        public ChangeEventoLocation(Guid eventoId)
            : base(eventoId)
        {
        }
    }
}
