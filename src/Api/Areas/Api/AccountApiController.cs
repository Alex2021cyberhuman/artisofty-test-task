using System.Threading;
using System.Threading.Tasks;
using Api.Areas.Api.Models;
using Api.Authorization;
using AutoMapper;
using Logic.Accounts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Api.Areas.Api
{
    [ApiController]
    [Route("api/account")]
    public class AccountApiController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;

        public AccountApiController(IAccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("get-my-info")]
        public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken)
        {
            var userInfo = await _accountManager.GetUserInfoAsync(cancellationToken);
            if (userInfo is null)
                return Unauthorized();
            var model = _mapper.Map<UserInfoResponse>(userInfo);
            return Ok(model);
        }
        
        [Authorize]
        [HttpPost("logout")]
        // TODO: Make refresh tokens
        public IActionResult Logout()
        {
            var response = HttpContext.Response;
            response.Headers.Authorization = StringValues.Empty;
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

            if (result.IsSuccessful
                && result is JwtLoginResult jwtLoginResult)
                return Ok(jwtLoginResult);

            return BadRequest(new
            {
                result.Code, result.Message
            });
        }
    }
}