using Microsoft.AspNetCore.Identity;

namespace WorkingMVC.Data.Entitys.Identity
{
    //клас ролі успадковується від IdentityRole з ключем типу int
    public class RoleEntity : IdentityRole<int>
    {
        //конструктори без параметрів
        public RoleEntity() { }
        //конструктор з параметром імені ролі для швидкого створення ролі з заданим ім'ям
        public RoleEntity(string name) { 
            this.Name = name;
        }
        //Навігаційна властивість для зв'язку з користувачами, які мають цю роль
        public ICollection<UserRoleEntity> UserRoles { get; set; } = null!;
    }
}
