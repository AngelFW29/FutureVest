namespace FutureVest.Persistence.Entities
{
    public class ReturnRateSetting
    {
        public int Id { get; set; }
        public required decimal MinReturnRate { get; set; }
        public required decimal MaxReturnRate { get; set; }
    }
}