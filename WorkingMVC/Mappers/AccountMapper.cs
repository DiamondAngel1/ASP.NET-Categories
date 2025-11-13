using AutoMapper;
using WorkingMVC.Data.Entitys.Identity;
using WorkingMVC.Models.Account;

namespace WorkingMVC.Mappers;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        //Автоматичне мапування RegisterViewModel в UserEntity
        //UserName буде дорівнювати Email, а Image ігноруємо
        CreateMap<RegisterViewModel, UserEntity>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.Image, opt => opt.Ignore());

        CreateMap<UserEntity, UserLinkViewModel>()
    .ForMember(x => x.Name, opt =>
        opt.MapFrom(x => $"{x.LastName} {x.FirstName}"))
    
    .ForMember(x => x.Image, opt =>
        opt.MapFrom(x => x.Image ?? "default.webp"));
    }
}
