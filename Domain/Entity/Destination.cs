// <copyright file="Destination.cs" company="RedTop">
// RedTop
// </copyright>

using System;

namespace Engaze.Event.Domain.Entity
{
    public class Destination
    {
        public Guid EventId { get; set; }

        public decimal DestinationLatitude { get; set; }

        public decimal DestinationLongitude { get; set; }

        public string DestinationName { get; set; }

        public string DestinationAddress { get; set; }
    }
}
