using System.Collections.Generic;
using System.Threading.Tasks;
using PayCloud.Services.Dto;

namespace PayCloud.Services
{
    public interface ITransactionServices
    {
        Task<ICollection<TransactionDto>> GetUserTransactionsAsync(int userId, int? accountId = null, int skip = 0, int take = int.MaxValue, string contains = "*", string sortOrder = "CreatedOn_desc");
        Task<int> GetUserTransactionsCountAsync(int userId, int? accountId = null, string contains = "*");

        Task<IReadOnlyCollection<TransactionDto>> GetTransactionsListAsync(int userId, int? accountId = null, int skip = 0, int take = int.MaxValue, string contains = "*", string sortOrder = "CreatedOn_desc");
        Task<int> GetTransactionsCountAsync(int userId, int? accountId = null, string contains = "*");

        Task MakePaymentAsync(TransactionCUDto transactionDto);
        Task SavePaymentAsync(TransactionCUDto transactionDto);
        Task SendPayment(int transactionId);

        Task CancelPaymentAsync(int transactionId);

    }
}