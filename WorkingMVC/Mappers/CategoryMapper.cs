using AutoMapper;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Models.Category;

namespace WorkingMVC.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryEntity, CategoryItemModel>();

        }
    }
}
