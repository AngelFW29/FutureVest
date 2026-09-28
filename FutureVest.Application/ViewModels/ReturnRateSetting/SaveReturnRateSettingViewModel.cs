using FutureVest.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace FutureVest.Application.ViewModels.ReturnRateSetting
{
    public class SaveReturnRateSettingViewModel
    {
        public int Id { get; set; }

        [ReturnRateLimits(ErrorMessage = "La tasa mínima debe ser menor que la tasa máxima.")]
        [Required(ErrorMessage = "La tasa mínima estimada de retorno es requerida.")]
        public required decimal MinReturnRate { get; set; }

        [Required(ErrorMessage = "La tasa máxima estimada de retorno es requerida.")]
        public required decimal MaxReturnRate { get; set; }
    }
}