using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PayCloud.Data.Models
{
    public class Account
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string AccountNumber { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal Balance { get; set; }

        //[Timestamp]
        //public byte[] ConcurrencyRowVersion { get; set; }

        public ICollection<Transaction> ReciveTransactions { get; set; }

        public ICollection<Transaction> SentTransactions { get; set; }

        public ICollection<PayCloudUserAccount> AccountUsers { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

    }
}
