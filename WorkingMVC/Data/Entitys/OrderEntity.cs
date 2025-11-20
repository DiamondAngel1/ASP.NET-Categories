using System.ComponentModel.DataAnnotations.Schema;
using WorkingMVC.Data.Entitys.Identity;

namespace WorkingMVC.Data.Entitys
{
    [Table("tblOrder")]
    public class OrderEntity : BaseEntity<int>
    {
        [ForeignKey(nameof(OrderStatus))]
        public int OrderStatusId { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public OrderStatusEntity? OrderStatus { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; } = null!;
    }
}
