using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Logic.Accounts;
using Logic.Accounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Accounts
{
    public class AccountController : Controller
    {
        private readonly AccountManager _accountManager;
        private readonly IMapper _mapper;

        public AccountController(AccountManager accountManager, IMapper mapper)
        {
            _accountManager = accountManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                model.Password = model.PasswordConfirm = string.Empty;
                return View(model);
            }
            var registerRequest = _mapper.Map<RegisterRequest>(model);
            var result = await _accountManager.RegisterAsync(registerRequest, cancellationToken);
            if (result.IsSuccessful)
                return View("Success", BuildSuccessViewModel(model));
                    
            ModelState.AddModelError(string.Empty, result.Message);
            ClearPassword(model);
            return View(model);

        }

        // TODO: Localize output
        private static SuccessViewModel BuildSuccessViewModel(RegisterInputModel model) =>
            new("Поздравляем!", $"Поздравляем {model.FIO}, вы стали пользователем системы !");

        private static void ClearPassword(RegisterInputModel model)
        {
            model.Password = model.PasswordConfirm = string.Empty;
        }
    }
}