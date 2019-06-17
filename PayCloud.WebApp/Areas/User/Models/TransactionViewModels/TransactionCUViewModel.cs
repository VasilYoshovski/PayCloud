using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.User.Models.TransactionViewModels
{
    public class TransactionCUViewModel
    {
        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public int SenderAccountId { get; set; }

        [Required]
        public int ReceiverAccountId { get; set; }

        public int CreatedByUserId { get; set; }

    }
}
