// <copyright file="DestinationUpdated.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Engaze.Core.DataContract;
using Engaze.Event.Domain.Core.Event;
using Engaze.Event.Domain.Entity;
using Newtonsoft.Json;

namespace Engaze.Event.Domain.Event
{
    public class DestinationUpdated : EventBase
    {
        public DestinationUpdated(Guid id, Location location)
            : base(id) => this.Destination = location;

        public Location Destination { get; private set; }
    }
}
