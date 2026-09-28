using FutureVest.Application.Dtos.Macroindicator;
using FutureVest.Application.ViewModels.Macroindicator;

namespace FutureVest.Application.ViewModels.SimulationMacroindicator
{
    public class SimulationMacroindicatorViewModel
    {
        public int Id { get; set; }
        public int MacroindicatorId { get; set; }
        public required decimal Weight { get; set; }
        public MacroindicatorViewModel? Macroindicator { get; set; }
    }
}