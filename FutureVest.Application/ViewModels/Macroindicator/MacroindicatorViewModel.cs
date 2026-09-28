namespace FutureVest.Application.ViewModels.Macroindicator
{
    public class MacroindicatorViewModel : BasicViewModel<int>
    {
        public required decimal Weight { get; set; }
        public required bool IsHigherBetter { get; set; }
    }
}