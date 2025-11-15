using WorkingMVC.Models.Users;

namespace WorkingMVC.Interfaces
{
    public interface IUserServices
    {
        Task<List<UserItemModel>> GetUsersAsync();
        Task<EditUserRolesViewModel> EditRoleAsync(string userId);
        Task<bool> UpdateRolesAsync(EditUserRolesViewModel model);
    }
}
