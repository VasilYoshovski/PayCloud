using System.ComponentModel.DataAnnotations;

namespace PayCloud.WebApp.Areas.Admin.Models.LoginViewModels
{
    public class AdminLoginViewModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(16)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; }

        public string Error { get; set; }
    }
}
