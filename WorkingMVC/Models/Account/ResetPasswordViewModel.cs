using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    
    [Required(ErrorMessage = "Вкажіть пароль")]
    [Display(Name = "Новий пароль")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть підтвердження пролю")]
    [Compare("NewPassword")]
    [Display(Name = "Підтвердження паролю")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
