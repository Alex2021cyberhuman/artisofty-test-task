using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Logic.Users.DataAccess.Interfaces;

namespace Logic.Accounts.Validation
{
    public class EmailUniqueAsyncValidator<T> : AsyncPropertyValidator<T, string>
    {
        public const string EmailUniqueAsyncValidatorName = "EmailUniqueAsyncValidator";

        private readonly IUserRepository _userRepository;

        public EmailUniqueAsyncValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<bool> IsValidAsync(ValidationContext<T> context, string value, CancellationToken cancellation)
        {
            return await _userRepository.CheckUniqueEmailAsync(value, cancellation);
        }

        public override string Name => EmailUniqueAsyncValidatorName;
    }
}