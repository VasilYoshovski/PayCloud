using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Areas.User.Models.UserTransactionsViewModels
{
    public class UserTransactionsViewModel
    {
        public uint PayerAccount { set; get; }
        public string PayerClientName { set; get; }
        public uint PayeeAccount { set; get; }
        public string PayeeClientName { set; get; }
        public string Description { set; get; }
        public decimal Amount { set; get; }
        public DateTime TimeStamp { set; get; }
        public bool TransactionDirection { set; get; }
        public string Status { set; get; }
    }
}
