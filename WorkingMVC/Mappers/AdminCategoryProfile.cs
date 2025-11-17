using AutoMapper;
using WorkingMVC.Areas.Admin.Models.Category;
using WorkingMVC.Data.Entitys;

namespace WorkingMVC.Mappers
{
    public class AdminCategoryProfile : Profile
    {
        public AdminCategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryItemModel>();
            CreateMap<CategoryEntity, CategoryEditModel>();
        }
    }
}
