using WorkingMVC.Models.Category;

namespace WorkingMVC.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryItemModel>> GetAllAsync();
        Task CreateAsync(CategoyCreateModel model);
        Task<CategoryEditModel> GetEditModelAsync(int id);
        Task UpdateAsync(CategoryEditModel model);
        Task SoftDeleteAsync(int id);

    }
}
