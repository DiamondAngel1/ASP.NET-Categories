using Microsoft.AspNetCore.Identity;

namespace WorkingMVC.Data.Entitys.Identity
{
    //клас для зв'язку між користувачами та ролями, успадковується від IdentityUserRole з ключем типу int
    public class UserRoleEntity : IdentityUserRole<int>
    {
        //навігаційна властивість для користувача
        public UserEntity User { get; set; } = null!;
        //навігаційна властивість для ролі
        public RoleEntity Role { get; set; } = null!;

    }
}
