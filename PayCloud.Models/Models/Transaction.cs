using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayCloud.Data.Models
{
    public class Transaction
    {
        [Required]
        public int TransactionId { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? SentOn { get; set; }

        [Required]
        public int StatusCode { get; set; }

        [Required]
        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }
        public PayCloudUser CreatedByUser { get; set; }

        [Required]
        public int SenderAccountId { get; set; }
        public Account SenderAccount { get; set; }

        [Required]
        public int ReceiverAccountId { get; set; }
        public Account ReceiverAccount { get; set; }

    }
}
