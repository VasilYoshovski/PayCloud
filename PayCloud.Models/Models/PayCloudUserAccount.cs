using System;
using System.ComponentModel.DataAnnotations;

namespace PayCloud.Data.Models
{
    public class PayCloudUserAccount
    {
        [Required]
        public int PayCloudUserId { get; set; }
        public PayCloudUser PayCloudUser { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        [Required]
        public DateTime AddedOn { get; set; }

        [StringLength(35, MinimumLength = 3)]
        public string AccountNickname { get; set; }


    }
}
