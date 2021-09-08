using System.Threading;
using System.Threading.Tasks;
using Api.Controllers;
using AutoMapper;
using Logic.Accounts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Api
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

        [Authorize]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var user = await _accountManager.GetUserInfoAsync(cancellationToken);
            var model = _mapper.Map<CabinetViewModel>(user);
            return View(model);
        }
    }
}