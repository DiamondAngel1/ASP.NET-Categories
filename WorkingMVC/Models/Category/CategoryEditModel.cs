using System.ComponentModel.DataAnnotations;

namespace WorkingMVC.Models.Category
{
    public class CategoryEditModel
    {
        public int Id { get; set; }
        [Display(Name = "Імя категорії")]
        [Required(ErrorMessage = "Вкажіть назву категорії")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Поточне фото категорії")]
        public string ExistingImage { get; set; } = string.Empty;
        public IFormFile? NewImage { get; set; }
    }
}
