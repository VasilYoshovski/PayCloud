using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Dto
{
    public class UserTransactionsDto
    {
        public ICollection<Transaction> ReceivedTransactions { get; set; }
        public ICollection<Transaction> SendTransactions { get; set; }
        public ICollection<Transaction> AllTransactions { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal SendAmount { get; set; }

    }
}
