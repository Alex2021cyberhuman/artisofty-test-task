using System.ComponentModel.DataAnnotations;
using Logic.Users.Options;

namespace Api.Areas.Api.Models
{
    public class LoginRequest
    {
        [Required]
        [StringLength(UserConfigurationOptions.UserPhoneMaxLength)]
        [RegularExpression(UserConfigurationOptions.UserPhoneRegexPattern, ErrorMessage = "Only 11 digits starting with 7")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(UserConfigurationOptions.UserPasswordMaxLength)]
        public string Password { get; set; } = string.Empty;
    }
}