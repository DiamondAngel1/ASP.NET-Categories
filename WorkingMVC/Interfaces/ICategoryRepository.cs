using WorkingMVC.Data.Entitys;

namespace WorkingMVC.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<CategoryEntity, int>
    {
        Task<CategoryEntity?> FindByNameAsync(string name);
        Task<CategoryEntity?> FindByIdAsync(int id);
    }
}
