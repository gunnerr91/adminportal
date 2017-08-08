using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Models.AuthViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberUser { get; set; }
    }

    public class Register
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch, please re-enter password")]
        public string ConfirmPassword { get; set; }
        
        public int Role { get; set; }

    }
}