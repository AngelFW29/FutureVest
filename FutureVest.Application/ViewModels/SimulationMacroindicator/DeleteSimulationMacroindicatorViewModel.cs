using FutureVest.Application.ViewModels.Macroindicator;

namespace FutureVest.Application.ViewModels.SimulationMacroindicator
{
    public class DeleteSimulationMacroindicatorViewModel
    {
        public int Id { get; set; }
        public int MacroindicatorId { get; set; }
        public required decimal Weight { get; set; }
        public MacroindicatorViewModel? Macroindicator { get; set; }
    }
}