using FutureVest.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureVest.Persistence.Entities
{
    public class Country : BasicEntity<int>
    {
        public required string IsoCode { get; set; }

        // Navigation property
        public ICollection<CountryIndicator>? Indicators { get; set; }
    }
}
