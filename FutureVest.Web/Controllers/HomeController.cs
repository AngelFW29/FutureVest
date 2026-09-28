using FutureVest.Application.Dtos.Ranking;
using FutureVest.Application.Services;
using FutureVest.Application.ViewModels.Ranking;
using FutureVest.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FutureVest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly RankingService _rankingService;
        private readonly MacroindicatorService _macroindicatorService;
        private readonly CountryIndicatorService _countryIndicatorService;
        public HomeController(FutureVestContext futureVestContext)
        {
            _rankingService = new RankingService(futureVestContext);
            _macroindicatorService = new MacroindicatorService(futureVestContext);
            _countryIndicatorService = new CountryIndicatorService(futureVestContext);
        }

        public async Task<IActionResult> Index()
        {
            var years = await _countryIndicatorService.GetAvailableYearsAsync();

            var rvm = new RankingViewModel
            {
                AvailableYears = years,
                SelectedYear = years.FirstOrDefault()
            };
            return View(rvm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(RankingViewModel rvm)
        {
            rvm.AvailableYears = await _countryIndicatorService.GetAvailableYearsAsync();

            var macroindicatorDto = await _macroindicatorService.GetAll();

            var rankingMacroindicators = macroindicatorDto.Select(m =>
            new RankingMacroindicatorDto
            {
                MacroindicatorId = m.Id,
                Weight = m.Weight,
                IsHigherBetter = m.IsHigherBetter
            }).ToList();

            var result = await _rankingService.CalculateRanking(rvm.SelectedYear, rankingMacroindicators);

            if (!result.Success)
            {
                rvm.ErrorMessage = result.ErrorMessage;
                rvm.ErrorType = result.ErrorType;
                return View(rvm);
            }

            rvm.ListRanking = result.Data!.Select(r =>
            new RankingResultViewModel
            {
                CountryName = r.CountryName,
                IsoCode = r.IsoCode,
                Scoring = r.Scoring,
                EstimatedReturnRate = r.EstimatedReturnRate,
            }).ToList();

            return View(rvm);
        }
    }
}
