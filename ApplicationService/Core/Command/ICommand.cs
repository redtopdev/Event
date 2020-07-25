// <copyright file="ICommand.cs" company="RedTop">
// RedTop
// </copyright>

using System;

namespace Engaze.EventSourcing.Core
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
