using System.Threading;
using System.Threading.Tasks;
using Api.Areas.Api.Authorization;
using Api.Areas.Api.Authorization.Interfaces;
using Api.Areas.Api.Models;
using Api.Authorization;
using AutoMapper;
using Logic.Accounts.Interfaces;
using Logic.Accounts.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Areas.Api
{
    [Area("Api")]
    [ApiController]
    [Route("api/account")]
    public class AccountApiController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;
        private readonly IJwtLoginProcessor _processor;
        private readonly JwtWriteOptions _jwtWriteOptions;

        public AccountApiController(IAccountManager accountManager,
            IMapper mapper, IJwtLoginProcessor processor,
            IOptions<JwtWriteOptions> jwtOptionsHolder)
        {
            _accountManager = accountManager;
            _mapper = mapper;
            _processor = processor;
            _jwtWriteOptions = jwtOptionsHolder.Value;
        }

        /// <summary>
        /// Returns logged in user's information.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/account/get-my-info
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Returns user info</response>
        /// <returns>Information about user such as FIO, phone, email and last login time</returns>
        [Authorize(AuthenticationSchemes =
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("get-my-info")]
        [ProducesResponseType(typeof(UserInfoResponse),
            StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyInfo(
            CancellationToken cancellationToken)
        {
            var userInfo =
                await _accountManager.GetUserInfoAsync(cancellationToken);
            if (userInfo is null)
                return Unauthorized();
            return Ok(_mapper.Map<UserInfoResponse>(userInfo));
        }

        /// <summary>
        /// Sign out user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/account/logout
        /// </remarks>
        /// <response code="200">Successful sign out</response>
        /// <returns>Empty response</returns>
        [Authorize(AuthenticationSchemes =
            JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            HttpContext.Response.Headers.Authorization = new();
            HttpContext.Response.Cookies
                .Delete(_jwtWriteOptions.AccessTokenCookieName,
                    _jwtWriteOptions.CookieOptions);
            return Ok();
        }

        /// <summary>
        /// Sign up user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/account/register
        ///     {
        ///         "fio": "Example Example Example",
        ///         "phone": "71234123456",
        ///         "email": "example@example",
        ///         "lastLogin": "2020-09-07T10:00:00.00000"
        ///         "password": "password",
        ///         "passwordConfirm": "password"
        ///     }
        /// </remarks>
        /// <response code="200">Successful sign up</response>
        /// <response code="400">Something is wrong, such as non-unique phone number or non-unique email address or other validation error </response>
        /// <returns>Empty response</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest model,
            CancellationToken cancellationToken)
        {
            var registerRequest = _mapper.Map<RegisterModel>(model);
            var result =
                await _accountManager.RegisterAsync(registerRequest,
                    cancellationToken);
            if (result.IsSuccessful)
                return Ok();

            return BadRequest(_mapper.Map<ErrorResponse>(result));
        }

        /// <summary>
        /// Sign in user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/account/login
        ///     {
        ///         "phone": "71234123456",
        ///         "password": "password"
        ///     }
        /// </remarks>
        /// <response code="200">Successful sign in</response>
        /// <response code="400">Something is wrong, such as validation error</response>
        /// <returns>Empty response with Set-Cookie header that contains access token</returns>
        [HttpPost("login-cookie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginCookie(
            [FromBody] LoginRequest model, CancellationToken cancellationToken)
        {
            var loginRequest = _mapper.Map<LoginModel>(model);
            var result =
                await _accountManager.LoginAsync(loginRequest,
                    cancellationToken);

            if (result.IsSuccessful)
            {
                var jwtResult =
                    await _processor.ProcessLoginAsync(result.User!,
                        cancellationToken);
                var cookies = HttpContext.Response.Cookies;
                cookies.Append(_jwtWriteOptions.AccessTokenCookieName,
                    jwtResult.AccessToken, _jwtWriteOptions.CookieOptions);
                return Ok();
            }

            return BadRequest(_mapper.Map<ErrorResponse>(result));
        }


        /// <summary>
        /// Sign in user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/account/login
        ///     {
        ///         "phone": "71234123456",
        ///         "password": "password"
        ///     }
        /// </remarks>
        /// <response code="200">Successful sign in</response>
        /// <response code="400">Something is wrong, such as validation error</response>
        /// <returns>Response with access token and expiration time</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest model,
            CancellationToken cancellationToken)
        {
            var loginRequest = _mapper.Map<LoginModel>(model);
            var result =
                await _accountManager.LoginAsync(loginRequest,
                    cancellationToken);

            if (result.IsSuccessful)
            {
                var jwtResult =
                    await _processor.ProcessLoginAsync(result.User!,
                        cancellationToken);
                return Ok(_mapper.Map<TokenResponse>(jwtResult));
            }

            return BadRequest(_mapper.Map<ErrorResponse>(result));
        }
    }
}