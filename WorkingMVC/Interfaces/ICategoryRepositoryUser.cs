using WorkingMVC.Data.Entitys;

namespace WorkingMVC.Interfaces
{
    public interface ICategoryRepositoryUser : IGenericRepositoryUser<CategoryEntity, int>
    {
        Task<CategoryEntity?> FindByNameAsync(string name);
        Task<CategoryEntity?> FindByIdAsync(int id);
    }
}
