using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace WorkingMVC.Data.Entitys
{
    [Table("tbl_products")]
    public class ProductEntity : BaseEntity<int>
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryEntity Category { get; set; } = null!;

        [Required]
        public string ImagesJson { get; set; } = "[]";

        [NotMapped]
        public List<string> Images
        {
            get => JsonSerializer.Deserialize<List<string>>(ImagesJson) ?? new();
            set => ImagesJson = JsonSerializer.Serialize(value);
        }
    }
}