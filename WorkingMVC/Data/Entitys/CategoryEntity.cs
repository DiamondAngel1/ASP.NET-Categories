using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkingMVC.Data.Entitys
{
    [Table("tbl_categories")]
    public class CategoryEntity : BaseEntity<int>
    {
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string Image { get; set; } = string.Empty;
    }
}
