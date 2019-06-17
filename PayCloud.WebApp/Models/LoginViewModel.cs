using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(16, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; }

        public IEnumerable<Banner> Banners { get; set; }

        public string Error { get; set; }
    }
}
