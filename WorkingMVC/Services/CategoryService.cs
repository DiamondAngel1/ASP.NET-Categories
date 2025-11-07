using AutoMapper;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Interfaces;
using WorkingMVC.Models.Category;
using WorkingMVC.Repositories;

namespace WorkingMVC.Services
{
    public class CategoryService(ICategoryRepository categoryRepository,
        IMapper mapper,
        IImageService imageService) : ICategoryService
    {
        public async Task CreateAsync(CategoyCreateModel model)
        {
            var entity =await categoryRepository.FindByNameAsync(model.Name);

            if (entity != null)
            {
                throw new Exception("Категорія з такою назвою вже існує");
            
            }

            entity = new CategoryEntity
            {
                Name = model.Name
            };

            if (model.Image != null)
            {
                entity.Image = await imageService.UploadImageAsync(model.Image);
            }
            await categoryRepository.AddAsync(entity);
        }
        public async Task<List<CategoryItemModel>> GetAllAsync()
        {
            var listTest = await categoryRepository.GetAllAsync();
            var model = mapper.Map<List<CategoryItemModel>>(listTest);
            return model;
        }
        public async Task<CategoryEditModel> GetEditModelAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Категорія Не знайдена");
            }
            var model = new CategoryEditModel
            {
                Id = category.Id,
                Name = category.Name,
                ExistingImage = category.Image
            };
            return model;
        }
        public async Task UpdateAsync(CategoryEditModel model)
        {
            var category = await categoryRepository.FindByIdAsync(model.Id);
            if (category == null)
            {
                throw new Exception("Категорія Не знайдена");
            }

            var normalizedName = model.Name.Trim().ToLower();
            var duplicate = await categoryRepository.FindByNameAsync(model.Name);
                
            if (duplicate!=null && duplicate.Id != model.Id)
            {
                model.ExistingImage = category.Image; //щоб не втратити посилання на існуюче зображення
                throw new Exception("Категорія з такою назвою вже існує");
            }

            category.Name = model.Name;

            if (model.NewImage != null)
            {
                await imageService.DeleteImageAsync(category.Image);
                category.Image = await imageService.UploadImageAsync(model.NewImage);
            }

            await categoryRepository.UpdateAsync(category);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null || category.IsDeleted)
            {
                throw new Exception("Категорія Не знайдена або вже видалена");
            }
            category.IsDeleted = true;
            await categoryRepository.UpdateAsync(category);
        }
    }
}
