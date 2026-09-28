namespace FutureVest.Application.Dtos
{
    public class BasicViewModel<TKey>
    {
        public required TKey Id { get; set; }
        public required string Name { get; set; }
    }
}