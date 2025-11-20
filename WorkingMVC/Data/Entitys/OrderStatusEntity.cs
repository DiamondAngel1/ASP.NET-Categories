using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkingMVC.Data.Entitys
{
    [Table("tblOrderStatuses")]
    public class OrderStatusEntity : BaseEntity<int>
    {
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;
    }
}
