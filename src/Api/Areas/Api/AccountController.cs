using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Api
{
    public class AccountController : Controller
    {
        public IActionResult Register() => View();
        public IActionResult Login() => View();
    }
}