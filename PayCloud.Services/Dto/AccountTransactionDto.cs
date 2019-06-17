using System;
using System.ComponentModel.DataAnnotations;

namespace PayCloud.Services.Dto
{
    public class AccountTransactionDto
    {
        [Required]
        public int TransactionId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? SendOn { get; set; }

        [Required]
        public int StatusCode { get; set; }
    }
}
