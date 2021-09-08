using System.ComponentModel.DataAnnotations;
using Logic.Users.Options;

namespace Api.Controllers
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(UserConfigurationOptions.UserPhoneMaxLength)]
        [RegularExpression(UserConfigurationOptions.UserPhoneRegexPattern,
            ErrorMessage = "Only 11 digits starting with 7")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(UserConfigurationOptions.UserPasswordMaxLength)]
        public string Password { get; set; } = string.Empty;
    }
}