using FutureVest.Application.Dtos.Ranking;
using FutureVest.Application.Dtos.SimulationMacroindicator;
using FutureVest.Application.Services;
using FutureVest.Application.ViewModels.Macroindicator;
using FutureVest.Application.ViewModels.Ranking;
using FutureVest.Application.ViewModels.SimulationMacroindicator;
using FutureVest.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FutureVest.Web.Controllers
{
    public class SimulationMacroindicatorController : Controller
    {
        private readonly SimulationMacroindicatorService _simulationMacroindicatorService;
        private readonly RankingService _rankingService;
        private readonly CountryIndicatorService _countryIndicatorService;
        public SimulationMacroindicatorController(FutureVestContext futureVestContext)
        {
            _simulationMacroindicatorService = new SimulationMacroindicatorService(futureVestContext);
            _rankingService = new RankingService(futureVestContext);
            _countryIndicatorService = new CountryIndicatorService(futureVestContext);
        }

        private async Task LoadAvailableMacroindicatorsAsync()
        {
            var availableDtos = await _simulationMacroindicatorService.GetAvailableMacroindicators();
            ViewBag.Macroindicators = availableDtos.Select(m => new MacroindicatorViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Weight = m.Weight,
                IsHigherBetter = m.IsHigherBetter
            }).ToList();
        }

        public async Task<IActionResult> Index()
        {
            var simulationMacroindicatorDtos = await _simulationMacroindicatorService.GetAllInclude();

            var availableMacroindicators = await _simulationMacroindicatorService.GetAvailableMacroindicators();
            decimal currentWeight = simulationMacroindicatorDtos.Sum(s => s.Weight);

            ViewBag.CanAddMore = availableMacroindicators.Any() && currentWeight < 1;

            var listSimulationMacroindicatorVms = simulationMacroindicatorDtos.Select(s => new SimulationMacroindicatorViewModel
            {
                Id = s.Id,
                MacroindicatorId = s.MacroindicatorId,
                Weight = s.Weight,
                Macroindicator = s.Macroindicator == null ? null : new MacroindicatorViewModel
                {
                    Id = s.Macroindicator.Id,
                    Name = s.Macroindicator.Name,
                    Weight = s.Macroindicator.Weight,
                    IsHigherBetter = s.Macroindicator.IsHigherBetter
                }
            }).ToList();

            var years = await _countryIndicatorService.GetAvailableYearsAsync();

            var rankingVm = new RankingViewModel
            {
                AvailableYears = years,
                SelectedYear = years.FirstOrDefault()
            };

            ViewBag.RankingViewModel = rankingVm;

            return View(listSimulationMacroindicatorVms);
        }

        public async Task<IActionResult> Create()
        {
            await LoadAvailableMacroindicatorsAsync();
            return View("Save", new SaveSimulationMacroindicatorViewModel { Id = 0, Weight = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSimulationMacroindicatorViewModel scivm)
        {
            if (!ModelState.IsValid)
            {
                await LoadAvailableMacroindicatorsAsync();
                return View("Save", scivm);
            }

            SimulationMacroindicatorDto simulationMacroindicatorDto = new()
            {
                Id = 0,
                MacroindicatorId = scivm.MacroindicatorId ?? 0,
                Weight = scivm.Weight
            };

            var result = await _simulationMacroindicatorService.AddAsync(simulationMacroindicatorDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                await LoadAvailableMacroindicatorsAsync();
                return View("Save", scivm);
            }

            return RedirectToRoute(new { controller = "SimulationMacroindicator", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;

            var simulationMacroindicatorDto = await _simulationMacroindicatorService.GetById(id);
            if (simulationMacroindicatorDto == null)
            {
                return RedirectToRoute(new { controller = "SimulationMacroindicator", action = "Index" });
            }

            ViewBag.CurrentMacroindicatorName = simulationMacroindicatorDto.Macroindicator?.Name;

            SaveSimulationMacroindicatorViewModel scivm = new()
            {
                Id = simulationMacroindicatorDto.Id,
                MacroindicatorId = simulationMacroindicatorDto.MacroindicatorId,
                Weight = simulationMacroindicatorDto.Weight
            };

            return View("Save", scivm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSimulationMacroindicatorViewModel scivm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Save", scivm);
            }

            SimulationMacroindicatorDto simulationMacroindicatorDto = new()
            {
                Id = scivm.Id,
                MacroindicatorId = scivm.MacroindicatorId ?? 0,
                Weight = scivm.Weight
            };

            var result = await _simulationMacroindicatorService.UpdateAsync(simulationMacroindicatorDto);

            if (!result.Success)
            {
                ViewBag.EditMode = true;
                ModelState.AddModelError("", result.ErrorMessage!);
                return View("Save", scivm);
            }

            return RedirectToRoute(new { controller = "SimulationMacroindicator", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var simulationMacroindicatorDto = await _simulationMacroindicatorService.GetById(id);
            if (simulationMacroindicatorDto == null)
            {
                return RedirectToRoute(new { controller = "SimulationMacroindicator", action = "Index" });
            }

            DeleteSimulationMacroindicatorViewModel scivm = new()
            {
                Id = simulationMacroindicatorDto.Id,
                MacroindicatorId = simulationMacroindicatorDto.MacroindicatorId,
                Weight = simulationMacroindicatorDto.Weight,
                Macroindicator = simulationMacroindicatorDto.Macroindicator == null ? null : new MacroindicatorViewModel
                {
                    Id = simulationMacroindicatorDto.Macroindicator.Id,
                    Name = simulationMacroindicatorDto.Macroindicator.Name,
                    Weight = simulationMacroindicatorDto.Macroindicator.Weight,
                    IsHigherBetter = simulationMacroindicatorDto.Macroindicator.IsHigherBetter
                }
            };

            return View(scivm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteSimulationMacroindicatorViewModel scivm)
        {
            var result = await _simulationMacroindicatorService.DeleteAsync(scivm.Id);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View(scivm);
            }

            return RedirectToRoute(new { controller = "SimulationMacroindicator", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> SimulateRanking(int selectedYear)
        {
            var simulationDtos = await _simulationMacroindicatorService.GetAllInclude();

            var rankingMacroindicators = simulationDtos.Select(s => new RankingMacroindicatorDto
            {
                MacroindicatorId = s.MacroindicatorId,
                Weight = s.Weight,
                IsHigherBetter = s.Macroindicator?.IsHigherBetter ?? false
            }).ToList();

            var result = await _rankingService.CalculateRanking(selectedYear, rankingMacroindicators);

            var years = await _countryIndicatorService.GetAvailableYearsAsync();

            var rankingVm = new RankingViewModel
            {
                AvailableYears = years,
                SelectedYear = selectedYear
            };

            if (!result.Success)
            {
                rankingVm.ErrorMessage = result.ErrorMessage;
                rankingVm.ErrorType = result.ErrorType;
            }
            else
            {
                rankingVm.ListRanking = result.Data!.Select(r => new RankingResultViewModel
                {
                    CountryName = r.CountryName,
                    IsoCode = r.IsoCode,
                    Scoring = r.Scoring,
                    EstimatedReturnRate = r.EstimatedReturnRate
                }).ToList();
            }

            ViewBag.RankingViewModel = rankingVm;

            var simulationMacroindicatorDtos = await _simulationMacroindicatorService.GetAllInclude();
            var listSimulationMacroindicatorVms = simulationMacroindicatorDtos.Select(s => new SimulationMacroindicatorViewModel
            {
                Id = s.Id,
                MacroindicatorId = s.MacroindicatorId,
                Weight = s.Weight,
                Macroindicator = s.Macroindicator == null ? null : new MacroindicatorViewModel
                {
                    Id = s.Macroindicator.Id,
                    Name = s.Macroindicator.Name,
                    Weight = s.Macroindicator.Weight,
                    IsHigherBetter = s.Macroindicator.IsHigherBetter
                }
            }).ToList();

            return View("Index", listSimulationMacroindicatorVms);
        }
    }
}