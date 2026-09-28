using System.ComponentModel.DataAnnotations;

namespace FutureVest.Application.ViewModels.Macroindicator
{
    public class SaveMacroindicatorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del macroindicador es requerido.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El peso es requerido.")]
        [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1.")]
        public required decimal Weight { get; set; }

        [Required(ErrorMessage = "Debe indicar si es mejor un valor alto.")]
        public required bool IsHigherBetter { get; set; }
    }
}