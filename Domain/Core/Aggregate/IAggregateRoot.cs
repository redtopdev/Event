// <copyright file="IAggregateRoot.cs" company="RedTop">
// RedTop
// </copyright>

namespace Engaze.EventSourcing.Core
{
    using System;

    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
