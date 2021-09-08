using System.ComponentModel.DataAnnotations;
using Logic.Users.Options;

namespace Api.Areas.Api.Models
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(UserConfigurationOptions.UserFIOMaxLength)]
        public string FIO { get; set; } = string.Empty;

        [Required]
        [StringLength(UserConfigurationOptions.UserPhoneMaxLength)]
        [RegularExpression(UserConfigurationOptions.UserPhoneRegexPattern,
            ErrorMessage = "Only 11 digits starting with 7")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        [Required]
        [StringLength(UserConfigurationOptions.UserEmailMaxLength)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(UserConfigurationOptions.UserPasswordMaxLength)]
        public string Password { get; set; } = string.Empty;


        [Required]
        [StringLength(20)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}