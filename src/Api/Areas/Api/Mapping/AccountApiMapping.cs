using System;
using Api.Areas.Api.Models;
using Api.Authorization;
using AutoMapper;
using Logic.Accounts.Models;
using Logic.Users.Models;

namespace Api.Areas.Api.Mapping
{
    public class AccountApiMapping : Profile
    {
        public AccountApiMapping()
        {
            CreateMap<LoginRequest, LoginModel>();
            CreateMap<RegisterRequest, RegisterModel>();
            CreateMap<User, UserInfoResponse>();
            CreateMap<AccountResult, ErrorResponse>();
            CreateMap<JwtLoginResult, TokenResponse>()
                .ForMember(tokenResponse => tokenResponse.Expires,
                    options => options.MapFrom(
                        jwtLoginResult => new DateTimeOffset(jwtLoginResult.Expires).ToUnixTimeSeconds()));
        }
    }
}