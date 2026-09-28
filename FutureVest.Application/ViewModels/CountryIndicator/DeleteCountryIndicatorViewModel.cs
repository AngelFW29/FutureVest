using FutureVest.Application.ViewModels.Country;
using FutureVest.Application.ViewModels.Macroindicator;

namespace FutureVest.Application.ViewModels.CountryIndicator
{
    public class DeleteCountryIndicatorViewModel
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int Year { get; set; }

        public CountryViewModel? Country { get; set; }
        public MacroindicatorViewModel? Macroindicator { get; set; }
    }
}