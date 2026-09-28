using FutureVest.Application.Dtos.ReturnRateSetting;
using FutureVest.Application.Services;
using FutureVest.Application.ViewModels.ReturnRateSetting;
using FutureVest.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FutureVest.Web.Controllers
{
    public class ReturnRateSettingController : Controller
    {
        private readonly ReturnRateSettingService _returnRateSettingService;

        public ReturnRateSettingController(FutureVestContext futureVestContext)
        {
            _returnRateSettingService = new ReturnRateSettingService(futureVestContext);
        }

        public async Task<IActionResult> Save()
        {
            var returnRateSettingDto = await _returnRateSettingService.GetAll();

            SaveReturnRateSettingViewModel srrsvm = returnRateSettingDto == null
                ? new SaveReturnRateSettingViewModel
                {
                    Id = 0,
                    MinReturnRate = 0,
                    MaxReturnRate = 0
                }
                : new SaveReturnRateSettingViewModel
                {
                    Id = returnRateSettingDto.Id,
                    MinReturnRate = returnRateSettingDto.MinReturnRate,
                    MaxReturnRate = returnRateSettingDto.MaxReturnRate
                };

            return View("Save", srrsvm);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SaveReturnRateSettingViewModel srrsvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", srrsvm);
            }

            ReturnRateSettingDto returnRateSettingDto = new()
            {
                Id = srrsvm.Id,
                MinReturnRate = srrsvm.MinReturnRate,
                MaxReturnRate = srrsvm.MaxReturnRate
            };

            var result = await _returnRateSettingService.SaveAsync(returnRateSettingDto);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View("Save", srrsvm);
            }

            return RedirectToRoute(new { controller = "ReturnRateSetting", action = "Save" });
        }
    }
}