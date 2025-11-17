using System.ComponentModel.DataAnnotations;

namespace WorkingMVC.Areas.Admin.Models.Product
{
    public class ProductEditModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<string> ExistingImages { get; set; } = new();
        public List<string> ImagesToDelete { get; set; } = new();
        public List<IFormFile> NewImages { get; set; } = new();
    }
}
