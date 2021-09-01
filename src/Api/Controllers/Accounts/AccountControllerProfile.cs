using AutoMapper;
using Logic.Accounts.Models;

namespace Api.Controllers.Accounts
{
    public class AccountControllerProfile : Profile
    {
        public AccountControllerProfile()
        {
            CreateMap<RegisterInputModel, RegisterRequest>();
        }
    }
}