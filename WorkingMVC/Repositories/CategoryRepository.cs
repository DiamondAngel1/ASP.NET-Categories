using Microsoft.EntityFrameworkCore;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Interfaces;

namespace WorkingMVC.Repositories
{
    public class CategoryRepository : BaseRepository<CategoryEntity, int>,
        ICategoryRepository
    {
        public CategoryRepository(MyAppDbContext myAppDbContext) 
            : base(myAppDbContext)
        { }

        public async Task<CategoryEntity?> FindByNameAsync(string name)
        {
            var nameLower = name.Trim().ToLower();
            var entity = await _dbSet
                .SingleOrDefaultAsync(c => c.Name.ToLower() == nameLower);
            return entity;
        }
        public async Task<CategoryEntity?> FindByIdAsync(int id)
        {
            var category = await _dbSet .SingleOrDefaultAsync( 
                _dbContext=>_dbContext.Id == id);
            return category;
        }
    }
}
