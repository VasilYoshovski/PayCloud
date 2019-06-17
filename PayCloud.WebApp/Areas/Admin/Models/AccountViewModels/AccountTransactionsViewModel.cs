using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.Admin.Models.AccountViewModels;
using System.Collections.Generic;

namespace PayCloud.WebApp.Areas.Admin.Models.AccountViewModels_old
{
    public class AccountTransactionsViewModel
    {
        public AccountViewModel Account { get; set; }
        public IReadOnlyCollection<AccountTransactionDto> AccountTransactions { get; set; }

    }
}
