using AutoMapper;
using Logic.Accounts.Models;
using Logic.Users.Models;

namespace Logic.Accounts.Mapping
{
    public class AccountsProfile : Profile
    {
        public AccountsProfile()
        {
            CreateMap<RegisterRequest, User>();
        }
    }
}