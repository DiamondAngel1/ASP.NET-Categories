using AutoMapper;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Models.CategoryUser;

namespace WorkingMVC.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryEntity, CategoryItemModelUser>();

        }
    }
}
