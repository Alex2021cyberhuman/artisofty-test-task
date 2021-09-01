using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Logic.Accounts.Interfaces;
using Logic.Accounts.Models;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.Models;

namespace Logic.Accounts
{
    public class AccountManager : IAccountManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<RegisterRequest> _registerRequestValidator;
        private readonly IValidator<LoginRequest> _loginRequestValidator;
        private readonly IMapper _mapper;
        private readonly ILoginProcessor _loginProcessor;

        private static LoginResult AuthorizationError => new()
        {
            Code = "AuthorizationError", Message = "Error of authorization. It may wrong password or phone"
        };

        public AccountManager(
            IUserRepository userRepository,
            IValidator<RegisterRequest> registerRequestValidator,
            IValidator<LoginRequest> loginRequestValidator,
            IMapper mapper,
            ILoginProcessor loginProcessor)
        {
            _userRepository = userRepository;
            _registerRequestValidator = registerRequestValidator;
            _loginRequestValidator = loginRequestValidator;
            _mapper = mapper;
            _loginProcessor = loginProcessor;
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

        public async Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _loginRequestValidator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
                return AuthorizationError;
            var user = await _userRepository.FindUserByPhonePasswordAsync(request.Phone, request.Password, cancellationToken);
            if (user is null)
                return AuthorizationError;

            user = user with
            {
                LastLogin = DateTime.UtcNow
            };
            await _userRepository.UpdateLastLoginAsync(user.Id, user.LastLogin, cancellationToken);
            await _loginProcessor.ProcessLoginAsync(user, cancellationToken);
            return new(user);
        }
    }
}