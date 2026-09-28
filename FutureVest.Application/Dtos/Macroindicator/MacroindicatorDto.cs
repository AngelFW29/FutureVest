namespace FutureVest.Application.Dtos.Macroindicator
{
    public class MacroindicatorDto : BasicViewModel<int>
    {
        public required decimal Weight { get; set; }
        public required bool IsHigherBetter { get; set; }
    }
}
