using WorkingMVC.Data.Entitys;

namespace WorkingMVC.Areas.Admin.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(ProductEntity entity);
        Task<List<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(int id);
        Task UpdateAsync(ProductEntity entity);
        Task DeleteAsync(ProductEntity entity);
    }
}
