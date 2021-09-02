using Api.Areas.Api.Models;
using AutoMapper;
using Logic.Users.Models;

namespace Api.Areas.Api.Mapping
{
    public class AccountApiMapping : Profile
    {
        public AccountApiMapping()
        {
            CreateMap<LoginRequest, Logic.Accounts.Models.LoginModel>();
            CreateMap<RegisterRequest, Logic.Accounts.Models.RegisterModel>();
            CreateMap<User, UserInfoResponse>();
        }
    }
}