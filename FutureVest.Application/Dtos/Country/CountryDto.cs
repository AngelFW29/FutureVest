namespace FutureVest.Application.Dtos.Country
{
    public class CountryDto : BasicViewModel<int>
    {
        public required string IsoCode { get; set; }
    }
}
