using System.ComponentModel.DataAnnotations;

namespace FutureVest.Application.ViewModels.CountryIndicator
{
    public class SaveCountryIndicatorViewModel
    {
        public required int Id { get; set; }

        [Range(-999999999999, 999999999999, ErrorMessage = "El valor ingresado no es válido")]
        public required decimal Value { get; set; }

        [Range(1900, 2100, ErrorMessage = "Ingrese un año válido.")]
        public required int Year { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un país")]
        [Range(1, int.MaxValue, ErrorMessage = "El país seleccionado no es válido")]
        public int? CountryId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un macroindicador")]
        [Range(1, int.MaxValue, ErrorMessage = "El macroindicador seleccionado no es válido")]
        public int? MacroindicatorId { get; set; }
    }
}