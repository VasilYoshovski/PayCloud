using System.Collections.Generic;
using System.Threading.Tasks;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;

namespace PayCloud.Services
{
    public interface IAccountService
    {
        Task<AccountDto> GetAccountByIdAsync(int accountId);
        //Task<bool> AccountExists(int accountId);
        Task<ClientAccountDto> CreateAccountAsync(decimal balance, int clientId);
        //Task<Account> GetAccountInfo(int accountId);
        Task<IReadOnlyCollection<AccountDto>> GetAllAccountsAsync
            (int skip = 0, int take = int.MaxValue, bool addBalance=false,string contains = "*", int? clientId = null, int? userId = null, string sortOrder = "AccountId_desc", bool haveBalance = false);
        Task<int> GetAcountsCountAsync (string contains = "*", int? clientId = null, bool haveBalance = false);
        Task<IReadOnlyCollection<AccountPieChart>> GetAccountsForPieAsync(int userId);
        //Task<IReadOnlyCollection<Account>> GetAllAccountsOfUserAsync(int skip, int take, string contains = "*", int? clientId = null, int? userId = null, string sortOrder = "Name", bool haveBalance = false);
        Task<int> GetAcountsOfUserCountAsync(int skip, int take, string contains = "*", int? clientId = null, int? userId = null, string sortOrder = "Name", bool haveBalance = false);
		Task<List<int>> AccountsIdsAssignedToPayCloudUserAsync(int userId);

        Task<IReadOnlyCollection<AccountDto>> GetAllUserAccountsAsync(int userId, int skip = 0, int take = int.MaxValue, string contains = "*", int? clientId = null, string sortOrder = "AccountId_desc", bool haveBalance = false);
        Task<int> GetUserAcountsCountAsync(int userId, string contains = "*", int? clientId = null, bool haveBalance = false);

        Task<string> GetAccountNicknameAsync(int accountId, int userId);


        Task<int> ChangeNicknameAsync(int accountId, int userId, string nickname);

        Task<ICollection<DateBalanceDto>> LineChartInfo(int accountId, int days);



    }
}
