using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace WorkingMVC.Data.Entitys
{
    [Table("tbl_products")]
    public class ProductEntity : BaseEntity<int>
    {
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;
        public ICollection<ProductImageEntity>? ProductImages { get; set; }
        public ICollection<CartEntity>? Carts { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; } = null!;
    }
}