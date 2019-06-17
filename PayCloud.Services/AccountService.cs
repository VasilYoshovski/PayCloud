using Microsoft.EntityFrameworkCore;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Contracts;
using PayCloud.Services.Dto;
using PayCloud.Services.Mappers;
using PayCloud.Services.Providers;
using PayCloud.Services.Utils;
using PayCloud.Services.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services
{
    public class AccountService : IAccountService
    {
        private readonly PayCloudDbContext context;
        private readonly IRandom10Generator random;
        private readonly IDateTimeNowProvider dateTimeNow;
        private readonly IDtoMapper<Account, ClientAccountDto> clientAccountMapper;
        private readonly IDtoMapper<Account, AccountDto> accountMapper;

        public AccountService(
            PayCloudDbContext context,
            IRandom10Generator random,
            IDateTimeNowProvider dateTimeNow,
            IDtoMapper<Account, ClientAccountDto> clientAccountMapper,
            IDtoMapper<Account, AccountDto> accountMapper
            )
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            this.dateTimeNow = dateTimeNow ?? throw new ArgumentNullException(nameof(dateTimeNow));
            this.clientAccountMapper = clientAccountMapper ?? throw new ArgumentNullException(nameof(clientAccountMapper));
            this.accountMapper = accountMapper ?? throw new ArgumentNullException(nameof(accountMapper));
        }

        //public Task<Account> GetAccountInfo(int accountId)
        //{
        //    if (accountId <= 0)
        //    {
        //        throw new ServiceErrorException(Constants.WrongArguments);
        //    }
        //    return this.context.Accounts
        //        .Include(x => x.Client)
        //        .Include(x => x.ReciveTransactions)
        //        .Include(x => x.SentTransactions)
        //        .FirstOrDefaultAsync(x => x.AccountId == accountId);
        //}

        public async Task<string> GetAccountNicknameAsync(int accountId, int userId)
        {
            var account = await this.context.UsersAccounts.FindAsync(userId, accountId)
                ?? throw new ServiceErrorException(Constants.AccountNotExist);

            return account.AccountNickname;
        }


        //public async Task<bool> AccountExists(int accountId)
        //{
        //    if (accountId <= 0)
        //    {
        //        throw new ServiceErrorException(Constants.WrongArguments);
        //    }
        //    return (await this.context.Accounts.FindAsync(accountId)) == null ? false : true;
        //}

        public async Task<AccountDto> GetAccountByIdAsync(int accountId)
        {
            var account = await this.context.Accounts.Include(x => x.Client).SingleOrDefaultAsync(x => x.AccountId == accountId)
                ?? throw new ServiceErrorException(Constants.AccountNotExist);
            var t = this.accountMapper.MapFrom(account);
            return t;
        }

        public async Task<bool> AccountNumberExists(string accountNumber)
        {
            if (accountNumber == null)
            {
                throw new ServiceErrorException(Constants.WrongArguments);
            }
            return (await this.context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber)) == null ? false : true;
        }

        public async Task<ClientAccountDto> CreateAccountAsync(decimal balance, int clientId)
        {
            if (balance <= 0 || clientId <= 0)
            {
                throw new ServiceErrorException(Constants.WrongArguments);
            }

            if (await this.context.Clients.FindAsync(clientId) == null)
            {
                throw new ServiceErrorException(Constants.ClientNotFound);

            }
            var random10 = this.random.GenerateNumber();

            while (await this.AccountNumberExists(random10))
            {
                random10 = this.random.GenerateNumber();
            }

            var newAccount = new Account()
            {
                ClientId = clientId,
                Balance = balance,
                AccountNumber = random10
            };
            this.context.Accounts.Add(newAccount);
            await this.context.SaveChangesAsync();
            return this.clientAccountMapper.MapFrom(newAccount);
        }

        public async Task<int> ChangeNicknameAsync(int accountId, int userId, string nickname)
        {
            if (nickname == null || accountId <= 0 || userId <= 0)
            {
                throw new ServiceErrorException(Constants.WrongArguments);
            }

            var account = new PayCloudUserAccount()
            {
                AccountId = accountId,
                PayCloudUserId = userId,
                AccountNickname = nickname
            };

                this.context.UsersAccounts.Attach(account);
                this.context.Entry(account).Property(x => x.AccountNickname).IsModified = true;
            try
            {
                return await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ServiceErrorException(Constants.AccountNotExist);
            }
        }

        private IQueryable<AccountDto> GetAllUserAccountsQuery
            (int userId, int skip = 0, int take = int.MaxValue, string contains = "*", int? clientId = null, string sortOrder = "AccountId_desc", bool haveBalance = false)
        {
            var queryInner = this.context.UsersAccounts.Where(x => x.PayCloudUserId == userId);
            queryInner = queryInner.Include(x => x.Account.Client).AsQueryable();

            if (clientId != null)
            {
                queryInner = queryInner.Where(x => x.Account.ClientId == clientId);

            }

            var query = queryInner.Select(x =>
            new AccountDto
            {
                AccountId = x.AccountId,
                AccountNumber = x.Account.AccountNumber,
                Balance = x.Account.Balance,
                ClientName = x.Account.Client.Name,
                NickName = x.AccountNickname
            }
            );


            if (contains != "*" && !string.IsNullOrEmpty(contains))
            {
                query = query.Where(x => $"{x.ClientName} {x.Balance.ToString()} {x.NickName} {x.AccountNumber}".ToLower().Contains(contains.ToLower()));
            }

            if (haveBalance)
            {
                query = query.Where(x => x.Balance > 0);
            }

            //=====sorting====
            //Stakata: sortOder must match property name (case sens.)
            //Extension method sort
            query = string.IsNullOrEmpty(sortOrder) ? query.Sort("AccountId_desc") : query.Sort(sortOrder);

            query = query.Skip(skip).Take(take);

            return query;
        }


        private IQueryable<Account> GetAllAccountsQuery
             (int skip = 0, int take = int.MaxValue, string contains = "*", int? clientId = null, int? userId = null, string sortOrder = "AccountId_desc", bool haveBalance = false)
        {
            IQueryable<Account> query;
            if (userId != null)
            {
                var queryInner = this.context.UsersAccounts.Where(x => x.PayCloudUserId == userId);
                query = queryInner.Select(x => x.Account);
            }
            else
            {
                query = this.context.Accounts;
            }

            query = query.Include(x => x.Client).AsQueryable();

            if (contains != "*" && !string.IsNullOrEmpty(contains))
            {
                query = query.Where(x => $"{x.Client.Name} {x.Balance.ToString()} {x.AccountNumber}".ToLower().Contains(contains.ToLower()));
            }

            if (clientId != null)
            {
                query = query.Where(x => x.ClientId == clientId);
            }

            if (haveBalance)
            {
                query = query.Where(x => x.Balance > 0);
            }

            //=====sorting====
            //Stakata: sortOder must match property name (case sens.)
            //Extension method sort
            query = string.IsNullOrEmpty(sortOrder) ? query.Sort("AccountId_desc") : query.Sort(sortOrder);

            query = query.Skip(skip).Take(take);

            return query;
        }

        public async Task<List<int>> AccountsIdsAssignedToPayCloudUserAsync(int userId)
        {
            var accountsIdsOfUserList = await this.context.UsersAccounts
                .Where(ua => ua.PayCloudUserId == userId)
                .Select(a => a.AccountId)
                .ToListAsync();
            return accountsIdsOfUserList;
        }

        public async Task<int> GetAcountsOfUserCountAsync
            (int skip, int take, string contains = "*", int? clientId = null, int? userId = null, string sortOrder = "Name", bool haveBalance = false)
        {
            var query = await this.GetAllAccountsQuery(skip, take, contains, clientId, userId, sortOrder, haveBalance).ToListAsync();
            var queryAccountsOfUser = await this.AccountsIdsAssignedToPayCloudUserAsync(userId ?? -1);
            return query.Where(q => queryAccountsOfUser.Any(j => j == q.AccountId)).ToList().Count;
        }

        //public async Task<IReadOnlyCollection<Account>> GetAllAccountsOfUserAsync//TODO Dto
        //    (int skip, int take, string contains = "*", int? clientId = null, int? userId = null, string sortOrder = "Name", bool haveBalance = false)
        //{
        //    var query = await this.GetAllAccountsQuery(skip, take, contains, clientId, userId, sortOrder, haveBalance).ToListAsync();
        //    var queryAccountsOfUser = await this.AccountsIdsAssignedToPayCloudUserAsync(userId ?? -1);
        //    return query.Where(q => queryAccountsOfUser.Any(j => j == q.AccountId)).ToList();
        //}

        public Task<int> GetAcountsCountAsync
            (string contains = "*", int? clientId = null, bool haveBalance = false)
        {
            return this.GetAllAccountsQuery(contains:contains, clientId:clientId, haveBalance: haveBalance).CountAsync();
        }

        public async Task<IReadOnlyCollection<AccountDto>> GetAllAccountsAsync
            (int skip = 0, int take = int.MaxValue, bool addBalance = false, string contains = "*", int? clientId = null, int? userId = null, string sortOrder = "AccountId_desc", bool haveBalance = false)

        {
            var accounts = await this.GetAllAccountsQuery(skip, take, contains, clientId, userId, sortOrder, haveBalance).ToListAsync();

            return accounts.Select(accountMapper.MapFrom).ToList();

        }

        public async Task<IReadOnlyCollection<AccountDto>> GetAllUserAccountsAsync
            (int userId, int skip = 0, int take = int.MaxValue, string contains = "*", int? clientId = null, string sortOrder = "AccountId_desc", bool haveBalance = false)
        {
            return await this.GetAllUserAccountsQuery(userId, skip, take, contains, clientId, sortOrder, haveBalance).ToListAsync();
        }

        public Task<int> GetUserAcountsCountAsync
            (int userId, string contains = "*", int? clientId = null, bool haveBalance = false)
        {
            return this.GetAllUserAccountsQuery(userId, contains: contains, clientId: clientId, haveBalance: haveBalance).CountAsync();
        }

        //public async Task<IReadOnlyCollection<AccountUserDto>> GetAccountUsersAsync(int accountId)
        //{
        //    if (await this.context.Accounts.FindAsync(accountId) == null)
        //    {
        //        throw new ServiceErrorException(Constants.AccountNotExist);
        //    }

        //    var users = await this.context.UsersAccounts
        //        .Include(x => x.PayCloudUser)
        //        .Where(x => x.AccountId == accountId)
        //        .Select(x => new AccountUserDto()
        //        {
        //            Name = x.PayCloudUser.Name,
        //            UserId = x.PayCloudUser.UserId,
        //            Username = x.PayCloudUser.Username
        //        }).ToListAsync();

        //    return users;
        //}

        public async Task<IReadOnlyCollection<AccountPieChart>> GetAccountsForPieAsync(int userId)
        {
            var res = await this.context.UsersAccounts.Include(x => x.Account)
                .Where(x => x.PayCloudUserId == userId)
                .Select(x =>
                new AccountPieChart()
                {
                    AccountNickname = x.AccountNickname,
                    Balance = x.Account.Balance
                }).ToListAsync();
            return res;
        }

        public async Task<ICollection<DateBalanceDto>> LineChartInfo(int accountId, int days)
        {
            var t = dateTimeNow.Now.Date.AddDays(-days);

            var query = this.context.Transactions
               .Where(x => x.StatusCode == (int)StatusCode.Sent &&
                    x.SentOn >= dateTimeNow.Now.Date.AddDays(-days) &&
                    (x.SenderAccountId == accountId || x.ReceiverAccountId == accountId)
                );

            if (query.Count()==0)
            {
                return new List<DateBalanceDto>();
            }

            var groupTransactions = await (from transAmount in
                                (
                                 from transactions in query
                                 orderby transactions.SentOn descending
                                 select new
                                 {
                                     SentOn = ((DateTime)transactions.SentOn).Date,
                                     Amount = transactions.ReceiverAccountId == accountId ? transactions.Amount : -transactions.Amount
                                 })
                                           group transAmount by transAmount.SentOn.Date into g
                                           orderby g.Key descending
                                           select new
                                           {
                                               Amount = g.Sum(x => x.Amount),
                                               SentOn = g.Key
                                           }).ToListAsync();

            var now = dateTimeNow.Now;

            var balance = this.context.Accounts.Find(accountId).Balance;

            var res = new List<DateBalanceDto>();

            res.Add(new DateBalanceDto
            {
                Date = now,
                Balance = balance
            });

            foreach (var item in groupTransactions)
            {
                var d = item.SentOn;
                var r = item.Amount;

                balance -= r;

                res.Add(new DateBalanceDto
                {
                    Date = d,
                    Balance = balance
                });

            }
            res.Reverse();
            return res;
        }

    }



}
