using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayCloud.Services.Dto
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? SentOn { get; set; }

        public int StatusCode { get; set; }

        public string MainAccountNum { get; set; }

        public string SecondAccountNum { get; set; }

        public string MainNickname { get; set; }
        public string SecondNickname { get; set; }

        public string MainClientName { get; set; }

        public string SecondClientName { get; set; }
    }
}
