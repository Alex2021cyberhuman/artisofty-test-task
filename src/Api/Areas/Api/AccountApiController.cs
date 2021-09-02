using System.Threading;
using System.Threading.Tasks;
using Api.Authorization;
using Api.Controllers;
using AutoMapper;
using Logic.Accounts;
using Logic.Accounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Api
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/account")]
    public class AccountApiController : ControllerBase
    {
        private readonly AccountManager _accountManager;
        private readonly IMapper _mapper;

        public AccountApiController(AccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInputModel model, CancellationToken cancellationToken = default)
        {
            var registerRequest = _mapper.Map<RegisterRequest>(model);
            var result = await _accountManager.RegisterAsync(registerRequest, cancellationToken);
            if (result.IsSuccessful)
                return Ok();

            return BadRequest(new
            {
                result.Code, result.Message
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel model, CancellationToken cancellationToken = default)
        {
            var loginRequest = _mapper.Map<LoginRequest>(model);
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