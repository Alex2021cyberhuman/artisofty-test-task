using System;

namespace Api.Controllers
{
    public class CabinetViewModel
    {
        // ReSharper disable once InconsistentNaming
        public string FIO { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime LastLogin { get; set; }
    }
}