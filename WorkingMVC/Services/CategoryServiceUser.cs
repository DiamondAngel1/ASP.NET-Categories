using AutoMapper;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Interfaces;
using WorkingMVC.Repositories;
using WorkingMVC.Models;
using WorkingMVC.Models.CategoryUser;

namespace WorkingMVC.Services
{
    public class CategoryServiceUser(ICategoryRepositoryUser categoryRepository,
        IMapper mapper,
        IImageService imageService) : ICategoryServiceUser
    {
        
        public async Task<List<CategoryItemModelUser>> GetAllAsync()
        {
            var listTest = await categoryRepository.GetAllAsync();
            var model = mapper.Map<List<CategoryItemModelUser>>(listTest);
            return model;
        }
    }
}
