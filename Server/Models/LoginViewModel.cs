using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}