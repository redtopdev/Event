// <copyright file="Duration.cs" company="RedTop">
// RedTop
// </copyright>

using System;
using System.Collections.Generic;

namespace Evento.Domain.ValueObjects
{
    public class Duration : ValueObject<Duration>
    {
        public int TimeInterval { get; set; }

        public string Period { get; set; }

        public bool Enabled { get; set; }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            { TimeInterval, Period };
        }
    }
}
