using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels
{
    public class PayCloudUserViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Role { get; set; }

        //public ICollection<PayCloudUserClient> UserClients { get; set; }
        //public ICollection<PayCloudUserAccount> UserAccounts { get; set; }
    }
}
