using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PayCloud.Data.Models
{
    public class Client
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<PayCloudUserClient> ClientUsers { get; set; }

    }
}
