using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberUser { get; set; }
    }
}