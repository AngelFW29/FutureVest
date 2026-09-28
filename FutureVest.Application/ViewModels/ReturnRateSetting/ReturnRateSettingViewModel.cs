namespace FutureVest.Application.ViewModels.ReturnRateSetting
{
    public class ReturnRateSettingViewModel
    {
        public int Id { get; set; }
        public required decimal MinReturnRate { get; set; }
        public required decimal MaxReturnRate { get; set; }
    }
}