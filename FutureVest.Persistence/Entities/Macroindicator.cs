using FutureVest.Persistence.Common;

namespace FutureVest.Persistence.Entities
{
    public class Macroindicator : BasicEntity<int>
    {
        public required decimal Weight { get; set; }
        public required bool IsHigherBetter { get; set; }

        // Navigation property
        public ICollection<CountryIndicator>? Indicators { get; set; }
    }
}