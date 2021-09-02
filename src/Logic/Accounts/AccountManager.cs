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
        private readonly IValidator<RegisterModel> _registerRequestValidator;
        private readonly IValidator<LoginModel> _loginRequestValidator;
        private readonly IMapper _mapper;
        private readonly ILoginProcessor _loginProcessor;
        private readonly IAuthenticatedUserIdentifierProvider _identifierProvider;

        private static LoginErrorResult AuthorizationError => new()
        {
            Code = "AuthorizationError", Message = "Error of authorization. It may wrong password or phone"
        };

        public AccountManager(
            IUserRepository userRepository,
            IValidator<RegisterModel> registerRequestValidator,
            IValidator<LoginModel> loginRequestValidator,
            IMapper mapper,
            ILoginProcessor loginProcessor,
            IAuthenticatedUserIdentifierProvider identifierProvider)
        {
            _userRepository = userRepository;
            _registerRequestValidator = registerRequestValidator;
            _loginRequestValidator = loginRequestValidator;
            _mapper = mapper;
            _loginProcessor = loginProcessor;
            _identifierProvider = identifierProvider;
        }

        public async Task<AccountResult> RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await _registerRequestValidator.ValidateAsync(model, cancellationToken);
            if (!validationResult.IsValid)
            {
                var error = validationResult.Errors.First();
                return new()
                {
                    Code = error.ErrorCode, Message = error.ErrorMessage
                };
            }
            var user = _mapper.Map<User>(model);
            await _userRepository.CreateUserAsync(user, cancellationToken);
            return new();
        }

        public async Task<LoginResult> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
        {
            var result = await _loginRequestValidator.ValidateAsync(model, cancellationToken);
            if (!result.IsValid)
                return AuthorizationError;
            var user = await _userRepository.FindUserByPhonePasswordAsync(model.Phone, model.Password, cancellationToken);
            if (user is null)
                return AuthorizationError;

            user = user with
            {
                LastLogin = DateTime.UtcNow
            };
            await _userRepository.UpdateLastLoginAsync(user.Id, user.LastLogin, cancellationToken);
            return await _loginProcessor.ProcessLoginAsync(user, cancellationToken);
        }

        public async Task<User?> GetUserInfoAsync(CancellationToken cancellationToken = default)
        {
            var userId = await _identifierProvider.GetUserIdAsync(cancellationToken);
            return await _userRepository.FindUserByIdAsync(userId, cancellationToken);
        }
    }
}