using AutoMapper;
using Logic.Accounts.Models;
using Logic.Users.Models;

namespace Api.Controllers.Mapping
{
    public class MvcProfile : Profile
    {
        public MvcProfile()
        {
            CreateMap<User, CabinetViewModel>();
            CreateMap<RegisterViewModel, RegisterModel>();
            CreateMap<LoginViewModel, LoginModel>();
        }
    }
}