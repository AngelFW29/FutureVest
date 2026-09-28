namespace FutureVest.Application.Dtos.Ranking
{
    public class RankingMacroindicatorDto
    {
        public required int MacroindicatorId { get; set; }
        public required decimal Weight { get; set; }
        public required bool IsHigherBetter { get; set; }
    }
}