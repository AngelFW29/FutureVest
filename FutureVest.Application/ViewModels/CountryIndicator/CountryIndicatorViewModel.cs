using FutureVest.Application.ViewModels.Country;
using FutureVest.Application.ViewModels.Macroindicator;

namespace FutureVest.Application.ViewModels.CountryIndicator
{
    public class CountryIndicatorViewModel
    {
        public required int Id { get; set; }
        public required decimal Value { get; set; }
        public required int Year { get; set; }

        public int? CountryId { get; set; }
        public int? MacroindicatorId { get; set; }

        public CountryViewModel? Country { get; set; }
        public MacroindicatorViewModel? Macroindicator { get; set; }
    }
}
