using FutureVest.Application.Dtos.Macroindicator;

namespace FutureVest.Application.Dtos.SimulationMacroindicator
{
    public class SimulationMacroindicatorDto
    {
        public int Id { get; set; }
        public int MacroindicatorId { get; set; }
        public required decimal Weight { get; set; }
        public MacroindicatorDto? Macroindicator { get; set; }
    }
}