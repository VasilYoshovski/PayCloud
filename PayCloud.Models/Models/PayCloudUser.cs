using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PayCloud.Data.Models
{
    public class PayCloudUser
    {
        [Required]
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public ICollection<PayCloudUserClient> UserClients { get; set; }
        public ICollection<PayCloudUserAccount> UserAccounts { get; set; }
        public ICollection<Transaction> UserTransactions { get; set; }
    }
}
