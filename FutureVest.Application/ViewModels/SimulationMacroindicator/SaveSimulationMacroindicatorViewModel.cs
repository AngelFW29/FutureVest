using System.ComponentModel.DataAnnotations;

namespace FutureVest.Application.ViewModels.SimulationMacroindicator
{
    public class SaveSimulationMacroindicatorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un macroindicador.")]
        [Range(1, int.MaxValue, ErrorMessage = "El macroindicador seleccionado no es válido.")]
        public int? MacroindicatorId { get; set; }
        
        [Required(ErrorMessage = "El peso es requerido.")]
        [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1.")]
        public required decimal Weight { get; set; }
    }
}