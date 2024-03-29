﻿using FluentValidation;
using Logic.Accounts.Models;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.Options;

namespace Logic.Accounts.Validation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterModel>
    {
        public RegisterRequestValidator(IUserRepository userRepository)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(request => request.Phone)
                .NotEmpty()
                .MaximumLength(UserConfigurationOptions.UserPhoneMaxLength)
                .Matches(UserConfigurationOptions.UserPhoneRegex)
                .SetAsyncValidator(
                    new PhoneUniqueAsyncValidator<RegisterModel>(
                        userRepository));

            RuleFor(request => request.Email)
                .NotEmpty()
                .MaximumLength(UserConfigurationOptions.UserEmailMaxLength)
                .EmailAddress()
                .SetAsyncValidator(
                    new EmailUniqueAsyncValidator<RegisterModel>(
                        userRepository));

            RuleFor(request => request.Password)
                .NotEmpty()
                .Equal(request => request.PasswordConfirm)
                .MaximumLength(UserConfigurationOptions.UserPasswordMaxLength);

            RuleFor(request => request.FIO)
                .NotEmpty()
                .MaximumLength(UserConfigurationOptions.UserFIOMaxLength);
        }
    }
}