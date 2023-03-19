using System.ComponentModel.DataAnnotations;

namespace ToDoManager.Asp.Net.Core.Models.ModelForController
{
    public class AuthenticationModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
