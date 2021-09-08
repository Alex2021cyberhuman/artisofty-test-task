using AutoMapper;
using Logic.Users.Models;

namespace Api.Controllers.Mapping
{
    public class MvcProfile: Profile
    {
        public MvcProfile()
        {
            CreateMap<User, CabinetViewModel>();
        }
    }
}