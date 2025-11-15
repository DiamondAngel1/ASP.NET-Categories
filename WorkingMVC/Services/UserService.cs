using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys.Identity;
using WorkingMVC.Interfaces;
using WorkingMVC.Models.Users;

namespace WorkingMVC.Services
{
    public class UserService(MyAppDbContext context,
        IMapper mapper,
        UserManager<UserEntity> userManager,
        RoleManager<RoleEntity> roleManager) : IUserServices
    {
        public async Task<List<UserItemModel>> GetUsersAsync()
        {
            var query = context.Users;
            var model = await query
                .ProjectTo<UserItemModel>(mapper.ConfigurationProvider).ToListAsync();
            return model;

        }
        public async Task<EditUserRolesViewModel?> EditRoleAsync(string userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) return null;

            var currentRoles = await userManager.GetRolesAsync(user);
            var allRoles = await roleManager.Roles
                .Select(r => r.Name!)
                .ToListAsync();

            return new EditUserRolesViewModel
            {
                UserId = user.Id.ToString(),
                SelectedRoles = currentRoles.ToList(),
                AllRoles = allRoles
            };
        }
        public async Task<bool> UpdateRolesAsync(EditUserRolesViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null) return false;

            var currentRoles = await userManager.GetRolesAsync(user);
            var rolesToAdd = model.SelectedRoles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(model.SelectedRoles);

            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);

            return removeResult.Succeeded && addResult.Succeeded;
        }


    }
}
