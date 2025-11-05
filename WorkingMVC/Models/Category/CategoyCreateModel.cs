using System.ComponentModel.DataAnnotations;

namespace WorkingMVC.Models.Category
{
    public class CategoyCreateModel
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Вкажіть категорію")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Фото")]
        [Required(ErrorMessage = "Оберіть фото")]
        public IFormFile? Image { get; set; }
        
    }
}
