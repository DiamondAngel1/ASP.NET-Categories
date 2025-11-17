using AutoMapper;
using WorkingMVC.Areas.Admin.Models.Product;
using WorkingMVC.Data.Entitys;

namespace WorkingMVC.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateModel, ProductEntity>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<ProductEntity, ProductItemModel>();
        }
    }
}
