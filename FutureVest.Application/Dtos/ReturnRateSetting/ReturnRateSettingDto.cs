namespace FutureVest.Application.Dtos.ReturnRateSetting
{
    public class ReturnRateSettingDto
    {
        public int Id { get; set; }
        public required decimal MinReturnRate { get; set; }
        public required decimal MaxReturnRate { get; set; }
    }
}