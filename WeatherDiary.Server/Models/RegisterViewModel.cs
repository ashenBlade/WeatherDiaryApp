using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WeatherDiary.Server.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [Display(Name = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}