using WorkingMVC.Areas.Admin.Models.Product;

namespace WorkingMVC.Areas.Admin.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductCreateModel model);
        Task<List<ProductItemModel>> GetAllAsync();
        Task<ProductEditModel> GetEditModelAsync(int id);
        Task UpdateAsync(ProductEditModel model);
        Task DeleteAsync(int id);
    }
}
