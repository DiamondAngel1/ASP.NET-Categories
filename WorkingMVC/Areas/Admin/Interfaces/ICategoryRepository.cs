using WorkingMVC.Data.Entitys;
using WorkingMVC.Interfaces;

namespace WorkingMVC.Areas.Admin.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<CategoryEntity, int>
    {
        Task<CategoryEntity?> FindByNameAsync(string name);
        Task<CategoryEntity?> FindByIdAsync(int id);
    }
}
