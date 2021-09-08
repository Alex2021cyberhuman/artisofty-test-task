using System.Text.RegularExpressions;

namespace Logic.Users.Options
{
    public static class UserConfigurationOptions
    {
        // ReSharper disable once InconsistentNaming
        public const int UserFIOMaxLength = 250;
        public const int UserPhoneMaxLength = 11;
        public const int UserEmailMaxLength = 150;
        public const int UserPasswordMaxLength = 20;
        public const string UserPhoneRegexPattern = @"7(\d){10}";

        public static readonly Regex UserPhoneRegex =
            new(UserPhoneRegexPattern, RegexOptions.Compiled);
    }
}