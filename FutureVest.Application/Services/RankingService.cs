using FutureVest.Application.Common;
using FutureVest.Application.Dtos.Country;
using FutureVest.Application.Dtos.CountryIndicator;
using FutureVest.Application.Dtos.Ranking;
using FutureVest.Persistence.Contexts;

namespace FutureVest.Application.Services
{
    public class RankingService
    {
        private readonly CountryService _countryService;
        private readonly CountryIndicatorService _countryIndicatorService;
        private readonly ReturnRateSettingService _returnRateSettingService;
        public RankingService(FutureVestContext futureVestContext)
        {
            _countryService = new CountryService(futureVestContext);
            _countryIndicatorService = new CountryIndicatorService(futureVestContext);
            _returnRateSettingService = new ReturnRateSettingService(futureVestContext);
        }

        public async Task<ServiceResponse<List<RankingResultDto>>> CalculateRanking(int year, List<RankingMacroindicatorDto> macroindicators)
        {
            try
            {
                decimal totalWeight = macroindicators.Sum(m => m.Weight);
                if (Math.Abs(totalWeight - 1) > 0.0001m)
                {
                    return ServiceResponse<List<RankingResultDto>>.Fail("Se deben ajustar los pesos de los macroindicadores registrado hasta que la suma de lo mismo sea igual a 1", ErrorType.InvalidWeights);
                }

                var allCountries = await _countryService.GetAll();
                var allIndicators = await _countryIndicatorService.GetAllInclude();
                var indicatorsForYear = allIndicators.Where(ci => ci.Year == year).ToList();

                var eligibleCountries = GetEligibleCountries(allCountries, indicatorsForYear, macroindicators);

                if (eligibleCountries.Count < 2)
                {
                    if (eligibleCountries.Count == 1)
                    {
                        return ServiceResponse<List<RankingResultDto>>.Fail(
                            $"No hay suficiente países para poder calcular el ranking y la tasa de retorno, el único país que cumple con los requisitos es {eligibleCountries[0].Name}, debe agregar más indicadores a los demás países en el año seleccionado", ErrorType.NotEnoughCountries);
                    }
                    return ServiceResponse<List<RankingResultDto>>.Fail(
                        "No hay suficientes países que cumplan los requisitos para calcular el ranking en el año seleccionado", ErrorType.NotEnoughCountries);
                }

                var minMaxByMacroindicator = GetMinMaxByMacroindicator(eligibleCountries, indicatorsForYear);

                var rankingResults = new List<RankingResultDto>();

                foreach (var country in eligibleCountries)
                {
                    decimal scoring = CalculateScoring(country, indicatorsForYear, macroindicators, minMaxByMacroindicator);
                    decimal estimatedReturnRate = await CalculateEstimatedReturnRate(scoring);

                    rankingResults.Add(new RankingResultDto
                    {
                        CountryName = country.Name,
                        IsoCode = country.IsoCode,
                        Scoring = scoring,
                        EstimatedReturnRate = estimatedReturnRate
                    });
                }

                var orderedResults = rankingResults.OrderByDescending(r => r.Scoring).ToList();

                return ServiceResponse<List<RankingResultDto>>.Ok(orderedResults);
            }
            catch (Exception)
            {
                return ServiceResponse<List<RankingResultDto>>.Fail("Ocurrió un error inesperado");
            }
        }

        private List<CountryDto> GetEligibleCountries(
            List<CountryDto> allCountries,
            List<CountryIndicatorDto> indicatorsForYear,
            List<RankingMacroindicatorDto> macroindicators)
        {
            var requiredMacroindicatorIds = macroindicators
                .Where(m => m.Weight > 0)
                .Select(m => m.MacroindicatorId)
                .ToList();

            var eligibleCountries = new List<CountryDto>();

            foreach (var country in allCountries)
            {
                var countryIndicatorIds = indicatorsForYear
                    .Where(ci => ci.CountryId == country.Id)
                    .Select(ci => ci.MacroindicatorId)
                    .ToList();

                bool hasAllRequired = requiredMacroindicatorIds.All(id => countryIndicatorIds.Contains(id));

                if (hasAllRequired)
                {
                    eligibleCountries.Add(country);
                }
            }
            return eligibleCountries;
        }

        private decimal NormalizeValue(decimal value, decimal min, decimal max, bool isHigherBetter)
        {
            if (min == max) return 0.5m;

            return isHigherBetter
                ? (value - min) / (max - min)
                : (max - value) / (max - min);
        }

        private Dictionary<int, (decimal Min, decimal Max)> GetMinMaxByMacroindicator(
            List<CountryDto> eligibleCountries,
            List<CountryIndicatorDto> indicatorsForYear)
        {
            var eligibleCountryIds = eligibleCountries.Select(c => c.Id).ToList();

            var eligibleIndicators = indicatorsForYear
                .Where(ci => eligibleCountryIds.Contains(ci.CountryId))
                .ToList();

            var minMaxByMacroindicator = eligibleIndicators
                .GroupBy(ci => ci.MacroindicatorId)
                .ToDictionary(
                    group => group.Key,
                    group => (Min: group.Min(ci => ci.Value), Max: group.Max(ci => ci.Value))
                );

            return minMaxByMacroindicator;
        }

        private decimal CalculateScoring(
            CountryDto country,
            List<CountryIndicatorDto> indicatorsForYear,
            List<RankingMacroindicatorDto> macroindicators,
            Dictionary<int, (decimal Min, decimal Max)> minMaxByMacroindicator)
        {
            decimal totalScore = 0;

            foreach (var macroindicator in macroindicators)
            {
                var countryIndicator = indicatorsForYear
                    .FirstOrDefault(ci => ci.CountryId == country.Id && ci.MacroindicatorId == macroindicator.MacroindicatorId);

                if (countryIndicator == null)
                {
                    continue;
                }

                var (min, max) = minMaxByMacroindicator[macroindicator.MacroindicatorId];

                decimal normalizedValue = NormalizeValue(countryIndicator.Value, min, max, macroindicator.IsHigherBetter);
                decimal subScore = normalizedValue * macroindicator.Weight;

                totalScore += subScore;
            }

            return totalScore;
        }

        private async Task<decimal> CalculateEstimatedReturnRate(decimal scoring)
        {
            var settings = await _returnRateSettingService.GetAll();

            decimal minRate = 2;
            decimal maxRate = 15;

            if (settings != null && settings.MinReturnRate != 0 && settings.MaxReturnRate != 0)
            {
                minRate = settings.MinReturnRate;
                maxRate = settings.MaxReturnRate;
            }

            decimal minRateFraction = minRate / 100;
            decimal maxRateFraction = maxRate / 100;

            return minRateFraction + (maxRateFraction - minRateFraction) * scoring;
        }
    }
}