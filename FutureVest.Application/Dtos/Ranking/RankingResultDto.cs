
namespace FutureVest.Application.Dtos.Ranking
{
    public class RankingResultDto
    {
        public required string CountryName { get; set; }
        public required string IsoCode { get; set; }
        public required decimal Scoring { get; set; }
        public required decimal EstimatedReturnRate { get; set; }
    }
}