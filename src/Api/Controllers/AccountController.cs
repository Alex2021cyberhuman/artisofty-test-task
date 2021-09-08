using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Api.Authorization.Interfaces;
using AutoMapper;
using Logic.Accounts.Interfaces;
using Logic.Accounts.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;

        public AccountController(IAccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromForm] RegisterViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                model.PasswordConfirm = string.Empty;
                return View(model);
            }
            var registerModel = _mapper.Map<RegisterModel>(model);
            var result =
                await _accountManager.RegisterAsync(registerModel,
                    cancellationToken);

            if (result.IsSuccessful)
                return View("SuccessRegister", model);

            ModelState.AddModelError("", result.Message);

            model.Password = string.Empty;
            model.PasswordConfirm = string.Empty;
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model,
            [FromServices] IClaimsFactory claimsFactory,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                return View(model);
            }
            var registerModel = _mapper.Map<LoginModel>(model);
            var result =
                await _accountManager.LoginAsync(registerModel,
                    cancellationToken);

            if (result.IsSuccessful)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults
                    .AuthenticationScheme);
                var claims = await
                    claimsFactory.GetClaims(result.User!, cancellationToken);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new(new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme)));
                return RedirectToAction("Index", "Cabinet");
            }

            ModelState.AddModelError("Phone", result.Message);

            model.Password = string.Empty;
            return View(model);
        }
    }
}