using System.ComponentModel.DataAnnotations;

namespace FutureVest.Application.ViewModels.Country
{
    public class SaveCountryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del país es requerido.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$", ErrorMessage = "El nombre solo puede contener letras, espacios, guiones o apóstrofes.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El código ISO del país es requerido.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El código ISO del país debe tener 2 caracteres (ej: US, MX).")]
        [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "El código ISO solo debe contener letras (ej: US, MX).")]
        public required string IsoCode { get; set; }
    }
}