using System.ComponentModel.DataAnnotations;

namespace FutureVest.Application.ViewModels
{
    public class BasicViewModel<TKey>
    {
        public required TKey Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public required string Name { get; set; }
    }
}