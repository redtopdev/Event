// <copyright file="DeleteEvento.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using Engaze.Event.ApplicationService.Core.Command;

namespace Engaze.Event.ApplicationService.Command
{
    public class DeleteEvento : BaseCommand
    {
        public DeleteEvento(Guid eventoId)
            : base(eventoId)
        {
        }
    }
}
