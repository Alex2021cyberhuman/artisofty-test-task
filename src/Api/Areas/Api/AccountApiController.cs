using System.Threading;
using System.Threading.Tasks;
using Api.Areas.Api.Models;
using Api.Authorization;
using Api.Authorization.Interfaces;
using AutoMapper;
using Logic.Accounts.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Api.Areas.Api
{
    [ApiController]
    [Route("api/account")]
    public class AccountApiController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;
        private readonly IJwtLoginProcessor _processor;
        private readonly JwtWriteOptions _jwtWriteOptions;

        public AccountApiController(IAccountManager accountManager, IMapper mapper, IJwtLoginProcessor processor, IOptions<JwtWriteOptions> jwtOptionsHolder)
        {
            _accountManager = accountManager;
            _mapper = mapper;
            _processor = processor;
            _jwtWriteOptions = jwtOptionsHolder.Value;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("get-my-info")]
        public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken)
        {
            var userInfo = await _accountManager.GetUserInfoAsync(cancellationToken);
            if (userInfo is null)
                return Unauthorized();
            var model = _mapper.Map<UserInfoResponse>(userInfo);
            return Ok(model);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var cookies = HttpContext.Response.Cookies;
            cookies.Delete(_jwtWriteOptions.AccessTokenCookieName, _jwtWriteOptions.CookieOptions);
            return Ok();
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model, CancellationToken cancellationToken = default)
        {
            var registerRequest = _mapper.Map<Logic.Accounts.Models.RegisterModel>(model);
            var result = await _accountManager.RegisterAsync(registerRequest, cancellationToken);
            if (result.IsSuccessful)
                return Ok();

            return BadRequest(new
            {
                result.Code, result.Message
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model, CancellationToken cancellationToken = default)
        {
            var loginRequest = _mapper.Map<Logic.Accounts.Models.LoginModel>(model);
            var result = await _accountManager.LoginAsync(loginRequest, cancellationToken);

            if (result is { IsSuccessful: true, User: not null })
            {
                var jwtResult = await _processor.ProcessLoginAsync(result.User, cancellationToken);
                var cookies = HttpContext.Response.Cookies;
                cookies.Append(_jwtWriteOptions.AccessTokenCookieName, jwtResult.AccessToken, _jwtWriteOptions.CookieOptions);
                return Ok();
            }

            return BadRequest(new
            {
                result.Code, result.Message
            });
        }
    }
}