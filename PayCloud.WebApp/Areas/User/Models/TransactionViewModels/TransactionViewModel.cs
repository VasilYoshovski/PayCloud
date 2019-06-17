using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.User.Models.TransactionViewModels
{
    public class TransactionViewModel
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
        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "Account")]
        public string MainAccountNum { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "Part. account")]
        public string SecondAccountNum { get; set; }

        public string MainNickname { get; set; }
        public string SecondNickname { get; set; }

        public string MainClientName { get; set; }

        public string SecondClientName { get; set; }

    }
}
