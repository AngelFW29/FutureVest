using FutureVest.Application.Dtos.Macroindicator;
using FutureVest.Application.Services;
using FutureVest.Application.ViewModels.Macroindicator;
using FutureVest.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FutureVest.Web.Controllers
{
    public class MacroindicatorController : Controller
    {
        private readonly MacroindicatorService _macroindicatorService;

        public MacroindicatorController(FutureVestContext futureVestContext)
        {
            _macroindicatorService = new MacroindicatorService(futureVestContext);
        }

        public async Task<IActionResult> Index()
        {
            var macroindicatorDtos = await _macroindicatorService.GetAll();

            var listMacroindicatorVms = macroindicatorDtos.Select(s => new MacroindicatorViewModel()
            {
                Id = s.Id,
                Name = s.Name,
                Weight = s.Weight,
                IsHigherBetter = s.IsHigherBetter
            }).ToList();

            return View(listMacroindicatorVms);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveMacroindicatorViewModel() { Name = "", Weight = 0, IsHigherBetter = false });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveMacroindicatorViewModel smvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", smvm);
            }

            MacroindicatorDto macroindicatorDto = new()
            {
                Id = 0,
                Name = smvm.Name,
                Weight = smvm.Weight,
                IsHigherBetter = smvm.IsHigherBetter
            };

            var result = await _macroindicatorService.AddAsync(macroindicatorDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View("Save", smvm);
            }

            return RedirectToRoute(new { controller = "Macroindicator", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;

            var macroindicatorDto = await _macroindicatorService.GetById(id);

            if (macroindicatorDto == null)
            {
                return RedirectToRoute(new { controller = "Macroindicator", action = "Index" });
            }

            SaveMacroindicatorViewModel smvm = new()
            {
                Id = macroindicatorDto.Id,
                Name = macroindicatorDto.Name,
                Weight = macroindicatorDto.Weight,
                IsHigherBetter = macroindicatorDto.IsHigherBetter
            };
            return View("Save", smvm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveMacroindicatorViewModel smvm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Save", smvm);
            }

            MacroindicatorDto macroindicatorDto = new()
            {
                Id = smvm.Id,
                Name = smvm.Name,
                Weight = smvm.Weight,
                IsHigherBetter = smvm.IsHigherBetter
            };

            var result = await _macroindicatorService.UpdateAsync(macroindicatorDto);

            if (!result.Success)
            {
                ViewBag.EditMode = true;
                ModelState.AddModelError("", result.ErrorMessage!);
                return View("Save", smvm);
            }

            return RedirectToRoute(new { controller = "Macroindicator", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var macroindicatorDto = await _macroindicatorService.GetById(id);
            if (macroindicatorDto == null)
            {
                return RedirectToRoute(new { controller = "Macroindicator", action = "Index" });
            }

            DeleteMacroindicatorViewModel dmvm = new() { Id = macroindicatorDto.Id, Name = macroindicatorDto.Name };
            return View(dmvm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMacroindicatorViewModel dmvm)
        {
            if (!ModelState.IsValid)
            {
                return View(dmvm);
            }

            var result = await _macroindicatorService.DeleteAsync(dmvm.Id);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View(dmvm);
            }

            return RedirectToRoute(new { controller = "Macroindicator", action = "Index" });
        }
    }
}