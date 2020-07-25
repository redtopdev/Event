// <copyright file="IAggregateRoot.cs" company="RedTop">
// RedTop
// </copyright>

using System;

namespace Engaze.Event.Domain.Core.Aggregate
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
