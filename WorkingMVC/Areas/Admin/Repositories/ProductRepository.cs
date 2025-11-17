using Microsoft.EntityFrameworkCore;
using WorkingMVC.Areas.Admin.Interfaces;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;

namespace WorkingMVC.Areas.Admin.Repositories
{
    public class ProductRepository(MyAppDbContext db) : IProductRepository
    {
        public async Task AddAsync(ProductEntity entity)
        {
            await db.Products.AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task<List<ProductEntity>> GetAllAsync()
        {
            return await db.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            return await db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(ProductEntity entity)
        {
            db.Products.Update(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductEntity entity)
        {
            db.Products.Remove(entity);
            await db.SaveChangesAsync();
        }
    }
}