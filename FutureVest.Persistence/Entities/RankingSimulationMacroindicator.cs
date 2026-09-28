namespace FutureVest.Persistence.Entities
{
    public class RankingSimulationMacroindicator
    {
        public int Id { get; set; }
        public int MacroindicatorId { get; set; }
        public required decimal Weight { get; set; }
        public Macroindicator? Macroindicator { get; set; }
    }
}