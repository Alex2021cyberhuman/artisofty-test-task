using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Logic.Accounts.Models;
using Logic.Accounts.Validation;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.Models;

namespace Logic.Accounts
{
    public class AccountManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterRequest> _registerRequestValidator;
        private readonly IMapper _mapper;

        public AccountManager(IUserRepository userRepository,  IValidator<RegisterRequest> registerRequestValidator, IMapper mapper)
        {
            _userRepository = userRepository;
            _registerRequestValidator = registerRequestValidator;
            _mapper = mapper;
        }

        public async Task<AccountResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _registerRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var error = validationResult.Errors.First();
                return new()
                {
                    Code = error.ErrorCode, Message = error.ErrorMessage
                };
            }
            var user = _mapper.Map<User>(request);
            await _userRepository.CreateUserAsync(user, cancellationToken);
            return new();
        }
    }
}