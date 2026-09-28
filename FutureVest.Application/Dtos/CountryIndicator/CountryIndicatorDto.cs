using FutureVest.Application.Dtos.Country;
using FutureVest.Application.Dtos.Macroindicator;

namespace FutureVest.Application.Dtos.CountryIndicator
{
    public class CountryIndicatorDto
    {
        public required int Id { get; set; }
        public required decimal Value { get; set; }
        public required int Year { get; set; }
        public required int CountryId { get; set; }
        public required int MacroindicatorId { get; set; }
        public CountryDto? Country { get; set; }
        public MacroindicatorDto? Macroindicator { get; set; }
    }
}