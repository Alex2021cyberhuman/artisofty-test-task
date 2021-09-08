using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Logic.Accounts.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CabinetController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IMapper _mapper;

        public CabinetController(IAccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes =
            CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Index(
            CancellationToken cancellationToken)
        {
            var user =
                await _accountManager.GetUserInfoAsync(cancellationToken);
            var model = _mapper.Map<CabinetViewModel>(user);
            return View(model);
        }

        [Authorize(AuthenticationSchemes =
            CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults
                .AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}