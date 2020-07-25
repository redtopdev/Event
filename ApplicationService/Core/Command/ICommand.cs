// <copyright file="ICommand.cs" company="RedTop">
// RedTop
// </copyright>

using System;

namespace Engaze.Event.ApplicationService.Core.Command
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
