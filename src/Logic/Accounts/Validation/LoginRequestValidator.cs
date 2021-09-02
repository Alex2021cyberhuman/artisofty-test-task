using FluentValidation;
using Logic.Accounts.Models;
using Logic.Users.Options;

namespace Logic.Accounts.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.Phone)
                .NotEmpty()
                .MaximumLength(UserConfigurationOptions.UserPhoneMaxLength)
                .Matches(UserConfigurationOptions.UserPhoneRegex);

            RuleFor(request => request.Password)
                .NotEmpty()
                .MaximumLength(UserConfigurationOptions.UserPasswordMaxLength);
        }
    }
}