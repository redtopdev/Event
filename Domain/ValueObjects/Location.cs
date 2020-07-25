// <copyright file="Location.cs" company="RedTop">
// RedTop
// </copyright>

using System.Collections.Generic;

namespace Evento.Domain.ValueObjects
{
    public class Location : ValueObject<Location>
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            { Latitude, Longitude };
        }
    }
}
