using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Logic.Users.DataAccess.Interfaces;

namespace Logic.Accounts.Validation
{
    public class PhoneUniqueAsyncValidator<T> : AsyncPropertyValidator<T, string>
    {
        public const string PhoneUniqueAsyncValidatorName = "PhoneUniqueAsyncValidator";

        private readonly IUserRepository _userRepository;

        public PhoneUniqueAsyncValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "{PropertyName} must be unique.";
        }

        public override async Task<bool> IsValidAsync(ValidationContext<T> context, string value, CancellationToken cancellation)
        {
            return await _userRepository.CheckUniquePhoneAsync(value, cancellation);
        }


        public override string Name => PhoneUniqueAsyncValidatorName;
    }
}