using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkingMVC.Data.Entitys
{
    [Table("tblProductImages")]
    public class ProductImageEntity : BaseEntity<int>
    {
        [StringLength(250)]
        public string Name { get; set; } = String.Empty;

        public short Priority { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
