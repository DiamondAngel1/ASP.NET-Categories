using System.ComponentModel.DataAnnotations;

namespace WorkingMVC.Models.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Пошта")]
        [Required(ErrorMessage = "Вкажіть пошту")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Це поле обов'язкове!")]
        [DataType(DataType.Password)]// Вказує, що поле має бути саме паролем
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
