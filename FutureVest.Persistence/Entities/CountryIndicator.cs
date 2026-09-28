namespace FutureVest.Persistence.Entities
{
    public class CountryIndicator
    {
        public required int Id { get; set; }
        public required decimal Value { get; set; }
        public required int Year { get; set; }

        // Foreign keys
        public required int CountryId { get; set; }
        public required int MacroindicatorId { get; set; }

        // Navigation property
        public Country? Country { get; set; }
        public Macroindicator? Macroindicator { get; set; }
    }
}