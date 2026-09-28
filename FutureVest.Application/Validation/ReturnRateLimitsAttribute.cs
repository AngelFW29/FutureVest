using FutureVest.Application.ViewModels.ReturnRateSetting;
using System.ComponentModel.DataAnnotations;

namespace FutureVest.Application.Validation
{
    public class ReturnRateLimitsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var minReturnRate = (decimal)value!;

            var maxReturnRateProperty = validationContext.ObjectType
                .GetProperty(nameof(SaveReturnRateSettingViewModel.MaxReturnRate));

            var maxReturnRate = (decimal)maxReturnRateProperty!
                .GetValue(validationContext.ObjectInstance)!;

            if (minReturnRate >= maxReturnRate)
            {
                return new ValidationResult(ErrorMessage ?? "La tasa mínima debe ser menor que la tasa máxima.");
            }

            return ValidationResult.Success;
        }
    }
}