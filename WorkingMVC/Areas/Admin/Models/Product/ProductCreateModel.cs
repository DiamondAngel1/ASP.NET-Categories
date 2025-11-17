using System.ComponentModel.DataAnnotations;

namespace WorkingMVC.Areas.Admin.Models.Product
{
    public class ProductCreateModel
    {
        [Required(ErrorMessage = "Вкажіть назву")]
        [Display(Name = "Назва товару")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть ціну")]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }

        [Display(Name = "Опис")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Вкажіть ID категорії")]
        [Display(Name = "ID категорії")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Фото обовязкове")]
        [Display(Name = "Фото товару")]
        [MinLength(1, ErrorMessage = "Потрібно завантажити хоча б одне фото")]
        public List<IFormFile> Images { get; set; } = new();
    }
}
