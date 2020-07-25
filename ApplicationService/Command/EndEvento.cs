// <copyright file="EndEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
    public class EndEvento : BaseCommand
    {
        public EndEvento(Guid eventoId)
            : base(eventoId)
        {
        }
    }
}
