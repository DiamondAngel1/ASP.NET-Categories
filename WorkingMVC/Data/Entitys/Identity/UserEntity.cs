using Microsoft.AspNetCore.Identity;

namespace WorkingMVC.Data.Entitys.Identity
{
    //клас користувача успадковується від IdentityUser з ключем типу int
    public class UserEntity : IdentityUser<int>
    {
        //додаткові поля для зберігання імені, прізвища та зображення користувача
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Image { get; set; } = null;
        //навігаційна властивість для зв'язку з ролями користувача
        public ICollection<UserRoleEntity> UserRoles { get; set; } = null!;
    }
}
