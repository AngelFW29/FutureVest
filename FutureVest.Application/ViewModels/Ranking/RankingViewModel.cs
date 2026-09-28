using FutureVest.Application.Common;

namespace FutureVest.Application.ViewModels.Ranking
{
    public class RankingViewModel
    {
        public List<int> AvailableYears { get; set; } = [];
        public int SelectedYear { get; set; }
        public List<RankingResultViewModel> ListRanking { get; set; } = [];
        public string? ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; } = ErrorType.None;
    }
}