using System.ComponentModel.DataAnnotations;

namespace WorkingMVC.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Вкажіть пошту")]
        [EmailAddress]
        [Display(Name = "Пошта")]
        public string Email { get; set; } = string.Empty;
    }

}
