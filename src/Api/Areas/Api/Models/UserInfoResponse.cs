using System;

namespace Api.Areas.Api.Models
{
    public class UserInfoResponse
    {
        // ReSharper disable once InconsistentNaming
        public string FIO { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime LastLogin { get; set; }
    }
}