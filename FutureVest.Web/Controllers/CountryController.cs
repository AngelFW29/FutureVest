using FutureVest.Application.Dtos.Country;
using FutureVest.Application.Services;
using FutureVest.Application.ViewModels.Country;
using FutureVest.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FutureVest.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountryService _countryService;

        public CountryController(FutureVestContext futureVestContext)
        {
            _countryService = new CountryService(futureVestContext);
        }

        public async Task<IActionResult> Index()
        {
            var countryDtos = await _countryService.GetAll();

            var listCountryVms = countryDtos.Select(s => new CountryViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                IsoCode = s.IsoCode
            }).ToList();

            return View(listCountryVms);
        }

        public async Task<IActionResult> Create()
        {
            return View("Save", new SaveCountryViewModel() { Name = "", IsoCode = "", });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCountryViewModel scvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", scvm);
            }

            CountryDto countryDto = new()
            {
                Id = 0,
                Name = scvm.Name,
                IsoCode = scvm.IsoCode
            };

            var result = await _countryService.AddAsync(countryDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View("Save", scvm);
            }

            return RedirectToRoute(new { controller = "Country", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;

            var countryDto = await _countryService.GetById(id);

            if (countryDto == null)
            {
                return RedirectToRoute(new { controller = "Country", action = "Index" });
            }

            SaveCountryViewModel scvm = new()
            {
                Id = countryDto.Id,
                Name = countryDto.Name,
                IsoCode = countryDto.IsoCode
            };
            return View("Save", scvm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCountryViewModel scvm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Save", scvm);
            }

            CountryDto countryDto = new()
            {
                Id = scvm.Id,
                Name = scvm.Name,
                IsoCode = scvm.IsoCode
            };
            
            var result = await _countryService.UpdateAsync(countryDto);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View("Save", scvm);
            }

            return RedirectToRoute(new { controller = "Country", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var countryDto = await _countryService.GetById(id);
            if (countryDto == null)
            {
                return RedirectToRoute(new { controller = "Country", action = "Index" });
            }

            DeleteCountryViewModel dcvm = new() { Id = countryDto.Id, Name = countryDto.Name };
            return View(dcvm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCountryViewModel dcvm)
        {
            if (!ModelState.IsValid)
            {
                return View(dcvm);
            }

            await _countryService.DeleteAsync(dcvm.Id);
            return RedirectToRoute(new { controller = "Country", action = "Index" });
        }
    }
}