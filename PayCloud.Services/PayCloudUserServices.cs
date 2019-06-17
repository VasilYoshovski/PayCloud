using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Contracts;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services
{
    public class PayCloudUserServices : IPayCloudUserServices
    {
        private readonly PayCloudDbContext context;
        private readonly IAccountService accountService;
        private readonly IClientService clientService;
        private readonly IHashingService hashingService;
        private readonly IDateTimeNowProvider dateTimeNowProvider;
        private readonly ILogger<PayCloudUserServices> logger;

        public PayCloudUserServices(
            PayCloudDbContext context,
            IAccountService accountService,
            IClientService clientService,
            IHashingService hashingService,
            IDateTimeNowProvider dateTimeNowProvider,
            ILogger<PayCloudUserServices> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            this.clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            this.hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
            this.dateTimeNowProvider = dateTimeNowProvider ?? throw new ArgumentNullException(nameof(dateTimeNowProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PayCloudUser> FindPayCloudUserByUserNameAsync(string searchString)
        {
            searchString = string.IsNullOrWhiteSpace(searchString) ? "" : searchString.Trim().ToLower();
            return await this.context.PayCloudUsers.FirstOrDefaultAsync(b => b.Username.ToLower().Equals(searchString));
        }

        //public async Task<PayCloudUser> GetPayCloudUserByUserNameAsync(string searchString)
        //{
        //    return await FindPayCloudUserByUserNameAsync(searchString) ?? throw new ArgumentException($"Can not find PayCloudUser with user name {searchString}");
        //}

        //public async Task<PayCloudUser> FindPayCloudUserByNameAsync(string searchString)
        //{
        //    searchString = string.IsNullOrWhiteSpace(searchString) ? "" : searchString.Trim().ToLower();
        //    return await this.context.PayCloudUsers.FirstOrDefaultAsync(b => b.Name.ToLower().Contains(searchString));
        //}

        //public async Task<PayCloudUser> GetPayCloudUserByNameAsync(string searchString)
        //{
        //    return await FindPayCloudUserByNameAsync(searchString) ?? throw new ArgumentException($"Can not find PayCloudUser with name {searchString}");
        //}

        public async Task<PayCloudUser> FindPayCloudUserByIDAsync(int id)
        {
            return await this.context.PayCloudUsers.FirstOrDefaultAsync(b => b.UserId == id);
        }

        //public async Task<PayCloudUser> GetPayCloudUserByIDAsync(int id)
        //{
        //    return await FindPayCloudUserByIDAsync(id) ?? throw new ArgumentException($"Can not find PayCloudUser with ID {id}");
        //}

        public async Task<PayCloudUser> CreatePayCloudUserAsync(
            string name,
            string userName,
            string password,
            string role)
        {
            name = NormalizeString(name, "name");
            userName = NormalizeString(userName, "user name");
            role = NormalizeString(role, "role");

            if (null != (await FindPayCloudUserByUserNameAsync(userName)))
            {
                throw new ArgumentException($"PayCloudUser with name {userName} already exists!");
            }
            var user = new PayCloudUser
            {
                Name = name,
                Username = userName,
                Password = hashingService.GetHashedString(password),
                Role = role
            };
            await this.context.PayCloudUsers.AddAsync(user);
            await this.context.SaveChangesAsync();

            return await FindPayCloudUserByIDAsync(user.UserId);
        }

        //public async Task<PayCloudUser> UpdatePayCloudUserAsync(
        //    int id,
        //    string name,
        //    string userName,
        //    string password,
        //    string role)
        //{
        //    name = NormalizeString(name, "name");
        //    userName = NormalizeString(userName, "user name");
        //    role = NormalizeString(role, "role");

        //    var user = await FindPayCloudUserByIDAsync(id);
        //    if (user == null)
        //    {
        //        throw new ArgumentException($"User with name {userName} does not exist!");
        //    }
        //    else
        //    {
        //        user.Name = name;
        //        user.Username = userName;
        //        user.Password = password;
        //        user.Role = role;
        //        try
        //        {
        //            context.Update(user);
        //            await context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            throw;
        //        }
        //    }

        //    return await GetPayCloudUserByUserNameAsync(user.Username);
        //}

        //public async Task<bool> DeletePayCloudUserAsync(int id)
        //{
        //    var user = await FindPayCloudUserByIDAsync(id);
        //    if (null != user)
        //    {
        //        this.context.PayCloudUsers.Remove(user);
        //        await context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        throw new ArgumentException($"User with ID {id} does not exist!");
        //    }
        //    return true;
        //}

        public async Task<List<PayCloudUser>> GetAllPayCloudUsersAsync()
        {
            return (await this.context.PayCloudUsers.ToListAsync())
                        .OrderBy(b => b.Name)
                        .ThenBy(b => b.Username)
                        .ThenBy(b => b.Role)
                        .ToList();
        }

        //public async Task<(List<PayCloudUser> filteredList, int allCount)> GetAllPayCloudUsersByFilterAsync(
        //    int from,
        //    int to,
        //    string contains)
        //{
        //    var allUsers = await GetAllPayCloudUsersAsync();
        //    var allCount = 0;
        //    if (allUsers != null)
        //    {
        //        allCount = allUsers.Count;
        //        if ((from + to) > allCount)
        //        {
        //            if (allCount < to)
        //            {
        //                from = 0;
        //            }
        //            else
        //            {
        //                from = allCount - to;
        //            }
        //        }
        //        contains = string.IsNullOrWhiteSpace(contains) ? "" : contains.Trim().ToLower();
        //        var filteredPayCloudUsers = allUsers
        //            .Where(x => (x.Name.ToLower().Contains(contains) ||
        //            x.Username.ToString().ToLower().Contains(contains) ||
        //            x.UserId.ToString().ToLower().Contains(contains) ||
        //            x.Role.ToString().ToLower().Contains(contains)))
        //            .Skip(from)
        //            .Take(to);
        //        return (filteredPayCloudUsers.ToList(), allCount);
        //    }
        //    return (allUsers, allCount);
        //}

        //public async Task<(List<PayCloudUser> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllPayCloudUsersByFilterAsync(
        //    int pageNumber,
        //    int elementsPerPage,
        //    string contains)
        //{
        //    if (pageNumber < 0)
        //    {
        //        pageNumber = 0;
        //    }
        //    if (elementsPerPage < 1)
        //    {
        //        elementsPerPage = 1;
        //    }
        //    var result = await GetAllPayCloudUsersByFilterAsync(
        //        pageNumber * elementsPerPage,
        //        elementsPerPage,
        //        contains);

        //    return (result.filteredList, result.allCount, pageNumber, elementsPerPage);
        //}

        public async Task<bool> AssignAcountToUserAsync(int clientId, int accountId, int userId)
        {
            int result = 0;
            var user = await this.context.PayCloudUsers.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (null == user)
            {
                throw new ArgumentException($"User with ID {userId} does not exists!");
                //return false;
            }

            var client = await this.context.Clients.Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            if (null == client)
            {
                throw new ArgumentException($"Client with ID {clientId} does not exists!");
                //return false;
            }

            var accounts = await this.clientService.GetClientAccounts(clientId);
            if (null == accounts)
            {
                throw new ArgumentException($"Acount with ID {accountId} does not exists!");
                //return false;
            }
            else
            {
                var account = accounts.Where(a => a.AccountId == accountId).FirstOrDefault();
                if (null == account)
                {
                    throw new ArgumentException($"Acount with ID {accountId} does not exists!");
                    //return false;
                }
                else
                {
                    PayCloudUserAccount payCloudUserAccount = new PayCloudUserAccount()
                    {
                        AccountId = account.AccountId,
                        AddedOn = this.dateTimeNowProvider.Now,
                        PayCloudUserId = userId,
                        AccountNickname = account.AccountNumber
                    };

                    var findResult = await this.context.UsersAccounts.Where(x => (x.AccountId == payCloudUserAccount.AccountId && x.PayCloudUserId == payCloudUserAccount.PayCloudUserId)).FirstOrDefaultAsync();
                    if (null == findResult)
                    {
                        var addResult = await this.context.UsersAccounts.AddAsync(payCloudUserAccount);
                        result = await this.context.SaveChangesAsync();
                    }
                    else
                    {
                        // already exists
                    }
                }
            }
            return (1 == result) ? true : false;
        }

        public async Task<bool> RemoveAcountFromUserAsync(int clientId, int accountId, int userId)
        {
            int result = 0;
            var user = await this.context.PayCloudUsers.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (null == user)
            {
                throw new ArgumentException($"User with ID {userId} does not exists!");
                //return false;
            }

            var client = await this.context.Clients.Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            if (null == client)
            {
                throw new ArgumentException($"Client with ID {clientId} does not exists!");
                //return false;
            }

            var accounts = await this.clientService.GetClientAccounts(clientId);
            if (null == accounts)
            {
                throw new ArgumentException($"Acount with ID {accountId} does not exists!");
                //return false;
            }
            else
            {
                var account = accounts.Where(a => a.AccountId == accountId).FirstOrDefault();
                if (null == account)
                {
                    throw new ArgumentException($"Acount with ID {accountId} does not exists!");
                    //return false;
                }
                else
                {
                    PayCloudUserAccount payCloudUserAccount = new PayCloudUserAccount()
                    {
                        AccountId = account.AccountId,
                        AddedOn = this.dateTimeNowProvider.Now,
                        PayCloudUserId = userId
                    };

                    var findResult = await this.context.UsersAccounts.Where(x => (x.AccountId == payCloudUserAccount.AccountId && x.PayCloudUserId == payCloudUserAccount.PayCloudUserId)).FirstOrDefaultAsync();
                    if (null == findResult)
                    {
                        // does not exist
                    }
                    else
                    {
                        var addResult = this.context.UsersAccounts.Remove(findResult);
                        result = await this.context.SaveChangesAsync();
                    }
                }
            }
            return (1 == result) ? true : false;
        }

        public async Task<bool> AssignClientToUserAsync(int clientId, int userId)
        {
            int result = 0;
            var user = await this.context.PayCloudUsers.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (null == user)
            {
                throw new ArgumentException($"User with ID {userId} does not exists!");
                //return false;
            }

            var client = await this.context.Clients.Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            if (null == client)
            {
                throw new ArgumentException($"Client with ID {clientId} does not exists!");
                //return false;
            }
            else
            {
                PayCloudUserClient payCloudUserClient = new PayCloudUserClient()
                {
                    ClientId = clientId,
                    PayCloudUserId = userId
                };

                var findResult = await this.context.UsersClients.Where(x => (x.ClientId == payCloudUserClient.ClientId && x.PayCloudUserId == payCloudUserClient.PayCloudUserId)).FirstOrDefaultAsync();
                if (null == findResult)
                {
                    var addResult = await this.context.UsersClients.AddAsync(payCloudUserClient);
                    result = await this.context.SaveChangesAsync();
                }
                else
                {
                    // already exists
                    throw new ArgumentException($"User already assigned to client!");
                }
            }
            return (1 == result) ? true : false;
        }

        public async Task<bool> DeleteAllAccountsOfUser(int clientId, int userId)
        {
            bool removeResult = false;
            var user = await this.context.PayCloudUsers.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (null == user)
            {
                throw new ArgumentException($"User with ID {userId} does not exists!");
                //return false;
            }

            var client = await this.context.Clients.Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            if (null == client)
            {
                throw new ArgumentException($"Client with ID {clientId} does not exists!");
                //return false;
            }
            else
            {
                var accounts = await this.accountService.GetAllUserAccountsAsync(userId, 0, int.MaxValue, "*", clientId);
                foreach (var accountItem in accounts)
                {
                    removeResult = await RemoveAcountFromUserAsync(client.ClientId, accountItem.AccountId, user.UserId);
                    if (false == removeResult)
                    {
                        throw new ArgumentException($"Account with accountNumber {accountItem.AccountNumber} could not be removed from user with name {user.Name} that is assigned to client with name: {client.Name}!");
                    }
                }
            }
            return removeResult;
        }

        public async Task<bool> RemoveUserFromClientAsync(int clientId, int userId)
        {
            int result = 0;
            var user = await this.context.PayCloudUsers.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (null == user)
            {
                throw new ArgumentException($"User with ID {userId} does not exists!");
                //return false;
            }

            var client = await this.context.Clients.Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            if (null == client)
            {
                throw new ArgumentException($"Client with ID {clientId} does not exists!");
                //return false;
            }
            else
            {
                PayCloudUserClient payCloudUserClient = new PayCloudUserClient()
                {
                    ClientId = clientId,
                    PayCloudUserId = userId
                };

                var findResult = await this.context.UsersClients.Where(x => (x.ClientId == payCloudUserClient.ClientId && x.PayCloudUserId == payCloudUserClient.PayCloudUserId)).FirstOrDefaultAsync();
                if (null == findResult)
                {
                    // does not exist
                    result = 1;
                }
                else
                {
                    var addResult = this.context.UsersClients.Remove(findResult);
                    result = await this.context.SaveChangesAsync();
                }
            }
            return (1 == result) ? true : false;
        }

        public async Task<List<PayCloudUser>> PayCloudUsersAssignedToClientAsync(int? clientId)
        {
            var usersOfClientList = await this.context.UsersClients.ToListAsync();
            if (clientId != null)
            {
                usersOfClientList = usersOfClientList.Where(uc => uc.ClientId == clientId.Value).ToList();
            }


            var idsOfUsersOfClientList = usersOfClientList
                .Select(cu => cu.PayCloudUserId).ToList();

            var allUsers = await GetAllPayCloudUsersAsync();

            var usersAssignedToClient = allUsers
                .Where(u => idsOfUsersOfClientList.Any(i => i == u.UserId))
                .ToList();
            return usersAssignedToClient;
        }

        public async Task<List<PayCloudUser>> PayCloudUsersNotAssignedToClientAsync(int? clientId)
        {
            var usersOfClientList = await this.context.UsersClients
                .Where(uc => uc.ClientId == clientId)
                .ToListAsync();

            var idsOfUsersOfClientList = usersOfClientList
                .Select(cu => cu.PayCloudUserId).ToList();

            var allUsers = await GetAllPayCloudUsersAsync();

            var usersNotAssignedToClient = allUsers
                .Where(u => idsOfUsersOfClientList.All(i => i != u.UserId))
                .ToList();
            return usersNotAssignedToClient;
        }

        private string NormalizeString(string stringToCheck, string exceptionText)
        {
            if (string.IsNullOrEmpty(stringToCheck))
            {
                throw new ArgumentException($"{exceptionText} could not be null or empty!");
            }

            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentException($"{exceptionText} could not be WhiteSpace!");
            }
            return stringToCheck.Trim();
        }
    }
}
