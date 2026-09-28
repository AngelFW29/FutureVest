using FutureVest.Application.Dtos.CountryIndicator;
using FutureVest.Application.Services;
using FutureVest.Application.ViewModels.Country;
using FutureVest.Application.ViewModels.CountryIndicator;
using FutureVest.Application.ViewModels.Macroindicator;
using FutureVest.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FutureVest.Web.Controllers
{
    public class CountryIndicatorController : Controller
    {
        private readonly CountryIndicatorService _countryIndicatorService;
        private readonly CountryService _countryService;
        private readonly MacroindicatorService _macroindicatorService;

        public CountryIndicatorController(FutureVestContext futureVestContext)
        {
            _countryIndicatorService = new CountryIndicatorService(futureVestContext);
            _countryService = new CountryService(futureVestContext);
            _macroindicatorService = new MacroindicatorService(futureVestContext);
        }

        private async Task LoadFormSelectsAsync()
        {
            var countryDtos = await _countryService.GetAll();
            ViewBag.Countries = countryDtos.Select(c => new CountryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                IsoCode = c.IsoCode
            }).ToList();

            var macroindicatorDtos = await _macroindicatorService.GetAll();
            ViewBag.Macroindicators = macroindicatorDtos.Select(m => new MacroindicatorViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Weight = m.Weight,
                IsHigherBetter = m.IsHigherBetter
            }).ToList();
        }

        public async Task<IActionResult> Index(int? countryId, int? year)
        {
            await LoadFormSelectsAsync();
            ViewBag.SelectedCountryId = countryId;
            ViewBag.SelectedYear = year;

            var countryIndicatorDtos = await _countryIndicatorService
                .GetAllIncludeFilter(countryId, year);

            var listCountryMacroindicatorVms = countryIndicatorDtos.Select(s => new CountryIndicatorViewModel()
            {
                Id = s.Id,
                Value = s.Value,
                Year = s.Year,
                CountryId = s.CountryId,
                MacroindicatorId = s.MacroindicatorId,
                Country = s.Country == null ? null : new CountryViewModel
                {
                    Id = s.Country.Id,
                    Name = s.Country.Name,
                    IsoCode = s.Country.IsoCode
                },
                Macroindicator = s.Macroindicator == null ? null : new MacroindicatorViewModel
                {
                    Id = s.Macroindicator.Id,
                    Name = s.Macroindicator.Name,
                    Weight = s.Macroindicator.Weight,
                    IsHigherBetter = s.Macroindicator.IsHigherBetter
                }
            }).ToList();

            return View(listCountryMacroindicatorVms);
        }

        public async Task<IActionResult> Create()
        {
            await LoadFormSelectsAsync();
            return View("Save", new SaveCountryIndicatorViewModel { Id = 0, Value = 0, Year = DateTime.Now.Year });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCountryIndicatorViewModel scivm)
        {
            if (!ModelState.IsValid)
            {
                await LoadFormSelectsAsync();
                return View("Save", scivm);
            }

            CountryIndicatorDto countryIndicatorDto = new()
            {
                Id = 0,
                Value = scivm.Value,
                Year = scivm.Year,
                CountryId = scivm.CountryId ?? 0,
                MacroindicatorId = scivm.MacroindicatorId ?? 0
            };

            var result = await _countryIndicatorService.AddAsync(countryIndicatorDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                await LoadFormSelectsAsync();
                return View("Save", scivm);
            }

            return RedirectToRoute(new { controller = "CountryIndicator", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;
            await LoadFormSelectsAsync();

            var countryIndicatorDto = await _countryIndicatorService.GetById(id);
            if (countryIndicatorDto == null)
            {
                return RedirectToRoute(new { controller = "CountryIndicator", action = "Index" });
            }

            SaveCountryIndicatorViewModel scivm = new()
            {
                Id = countryIndicatorDto.Id,
                Value = countryIndicatorDto.Value,
                Year = countryIndicatorDto.Year,
                CountryId = countryIndicatorDto.CountryId,
                MacroindicatorId = countryIndicatorDto.MacroindicatorId
            };

            return View("Save", scivm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCountryIndicatorViewModel scivm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                await LoadFormSelectsAsync();
                return View("Save", scivm);
            }

            CountryIndicatorDto countryIndicatorDto = new()
            {
                Id = scivm.Id,
                Value = scivm.Value,
                Year = scivm.Year,
                CountryId = scivm.CountryId ?? 0,
                MacroindicatorId = scivm.MacroindicatorId ?? 0
            };

            var result = await _countryIndicatorService.UpdateAsync(countryIndicatorDto);

            if (!result.Success)
            {
                ViewBag.EditMode = true;
                ModelState.AddModelError("", result.ErrorMessage!);
                await LoadFormSelectsAsync();
                return View("Save", scivm);
            }

            return RedirectToRoute(new { controller = "CountryIndicator", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var countryIndicatorDto = await _countryIndicatorService.GetById(id);
            if (countryIndicatorDto == null)
            {
                return RedirectToRoute(new { controller = "CountryIndicator", action = "Index" });
            }

            DeleteCountryIndicatorViewModel scivm = new()
            {
                Id = countryIndicatorDto.Id,
                Value = countryIndicatorDto.Value,
                Year = countryIndicatorDto.Year,
                Country = countryIndicatorDto.Country == null ? null : new CountryViewModel
                {
                    Id = countryIndicatorDto.Country.Id,
                    Name = countryIndicatorDto.Country.Name,
                    IsoCode = countryIndicatorDto.Country.IsoCode
                },
                Macroindicator = countryIndicatorDto.Macroindicator == null ? null : new MacroindicatorViewModel
                {
                    Id = countryIndicatorDto.Macroindicator.Id,
                    Name = countryIndicatorDto.Macroindicator.Name,
                    Weight = countryIndicatorDto.Macroindicator.Weight,
                    IsHigherBetter = countryIndicatorDto.Macroindicator.IsHigherBetter
                }
            };

            return View(scivm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCountryIndicatorViewModel scivm)
        {
            var result = await _countryIndicatorService.DeleteAsync(scivm.Id);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View(scivm);
            }

            return RedirectToRoute(new { controller = "CountryIndicator", action = "Index" });
        }
    }
}