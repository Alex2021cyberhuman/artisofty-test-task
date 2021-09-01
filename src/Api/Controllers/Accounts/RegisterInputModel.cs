using System.ComponentModel.DataAnnotations;

namespace Api.Controllers
{
    public class RegisterInputModel
    {
        [Required]
        [StringLength(250)]
        public string FIO { get; set; }
        
        [Required]
        [StringLength(11)]
        [RegularExpression(@"7(\d){10}", ErrorMessage = "Only 11 digits starting with 7")]
        public string Phone { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        
        [Required]
        [StringLength(20)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}