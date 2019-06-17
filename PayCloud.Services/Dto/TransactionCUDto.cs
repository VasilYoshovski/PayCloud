using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Dto
{
    public class TransactionCUDto
    {
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public int StatusCode { get; set; }

        public int SenderAccountId { get; set; }

        public int ReceiverAccountId { get; set; }

        public int CreatedByUserId { get; set; }

    }
}
