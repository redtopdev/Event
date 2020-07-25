// <copyright file="BaseCommand.cs" company="RedTop">
// RedTop
// </copyright>

using System;

namespace Engaze.Event.ApplicationService.Core.Command
{
    public class BaseCommand : ICommand
    {
        public BaseCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}
