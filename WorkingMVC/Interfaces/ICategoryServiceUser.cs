using WorkingMVC.Models.CategoryUser;

namespace WorkingMVC.Interfaces
{
    public interface ICategoryServiceUser
    {
        Task<List<CategoryItemModelUser>> GetAllAsync();

    }
}
