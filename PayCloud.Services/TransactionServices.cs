using Microsoft.EntityFrameworkCore;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Identity.Contracts;
using PayCloud.Services.Mappers;
using PayCloud.Services.Providers;
using PayCloud.Services.Utils;
using PayCloud.Services.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services
{
    public class TransactionServices : ITransactionServices
    {
        private readonly PayCloudDbContext context;
        private readonly IUserService userService;
        private readonly IDateTimeNowProvider dateTimeNow;

        public TransactionServices(
            PayCloudDbContext context,
            IUserService userService,
            IDateTimeNowProvider dateTimeNow
            )
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.dateTimeNow = dateTimeNow ?? throw new ArgumentNullException(nameof(dateTimeNow));
        }

        private IDictionary<int, string> MyNicknames(int userId)
        {
            var query = this.context.UsersAccounts.Where(x => x.PayCloudUserId == userId);
            return query.ToDictionary(x => x.AccountId, x => x.AccountNickname);
        }

        private IQueryable<TransactionDto> GetAllTransactionsByUserQuery
           (int userId, int skip = 0, int take = int.MaxValue, int? accountId = null, string contains = "*", bool onlySent = true, string sortOrder = "CreatedOn_desc")
        {
            var query = this.context.UsersAccounts.Where(x => x.PayCloudUserId == userId);

            //query = query.Include(x => x.Account.ReciveTransactions).Include(x => x.Account.SentTransactions);
            var accounts = query.Select(x => x.Account);

            if (accountId != null)
            {
                accounts = accounts.Where(x => x.AccountId == accountId);
            }

            // accounts = accounts.Include(x => x.SentTransactions).Include(x => x.ReciveTransactions);

            var nicknames = this.MyNicknames(userId);

            var receivedTransactions = accounts.SelectMany(x => x.ReciveTransactions);
            var sentTransactions = accounts.SelectMany(x => x.SentTransactions);

            var receivedTransDtos = receivedTransactions
                          .Select(x => new TransactionDto()
                          {
                              TransactionId = x.TransactionId,
                              Amount = x.Amount,
                              Description = x.Description,
                              MainAccountNum = x.ReceiverAccount.AccountNumber,
                              SecondAccountNum = x.SenderAccount.AccountNumber,
                              CreatedOn = x.CreatedOn,
                              SentOn = x.SentOn,
                              StatusCode = x.StatusCode,
                              MainNickname = (nicknames.ContainsKey(x.ReceiverAccountId)) ? nicknames[x.ReceiverAccount.AccountId] : null,
                              SecondNickname = (nicknames.ContainsKey(x.SenderAccountId)) ? nicknames[x.SenderAccount.AccountId] : null,
                              MainClientName = x.ReceiverAccount.Client.Name,
                              SecondClientName = x.SenderAccount.Client.Name
                          });

            var sentTransDtos = sentTransactions
              .Select(x => new TransactionDto()
              {
                  TransactionId = x.TransactionId,
                  Amount = -x.Amount,
                  Description = x.Description,
                  MainAccountNum = x.SenderAccount.AccountNumber,
                  SecondAccountNum = x.ReceiverAccount.AccountNumber,
                  CreatedOn = x.CreatedOn,
                  SentOn = x.SentOn,
                  StatusCode = x.StatusCode,
                  MainNickname = (nicknames.ContainsKey(x.SenderAccountId)) ? nicknames[x.SenderAccount.AccountId] : null,
                  SecondNickname = (nicknames.ContainsKey(x.ReceiverAccountId)) ? nicknames[x.ReceiverAccount.AccountId] : null,
                  MainClientName = x.ReceiverAccount.Client.Name,
                  SecondClientName = x.SenderAccount.Client.Name

              });

            var transactions = receivedTransDtos.Concat(sentTransDtos);

            //var transactions = accounts.SelectMany(x => x.ReciveTransactions).Concat(accounts.SelectMany(x => x.SentTransactions));

            //var transactions = this.context.Transactions.Include(x => x.ReceiverAccount).Include(x => x.SenderAccount).AsQueryable();

            if (onlySent)
            {
                transactions = transactions.Where(x => x.StatusCode == (int)StatusCode.Sent);
            }

            if (contains != "*" && !string.IsNullOrEmpty(contains))
            {
                transactions = transactions.Where(x => (x.MainAccountNum + x.SecondAccountNum).Contains(contains));
            }

            //=====sorting====
            //Stakata: sortOder must match property name (case sens.)
            transactions = string.IsNullOrEmpty(sortOrder) ? transactions.Sort("CreatedOn_desc") : transactions.Sort(sortOrder);

            transactions = transactions.Skip(skip).Take(take);

            return transactions;
        }
        //////private IQueryable<Transaction> GetAllTransactionsQuery
        //////   (int skip = 0, int take = int.MaxValue, string contains = "*", bool onlySent = true, int? receiverAccount = null, int? senderAccount = null, string sortOrder = "CreatedOn_desc")
        //////{

        //////    var query = this.context.Transactions.Include(x => x.ReceiverAccount).Include(x => x.SenderAccount).AsQueryable();

        //////    if (onlySent)
        //////    {
        //////        query = query.Where(x => x.StatusCode == (int)StatusCode.Sent);
        //////    }

        //////    if ((receiverAccount != null) && (senderAccount != null))
        //////        if (receiverAccount != null)
        //////        {
        //////    {
        //////        query = query.Where(x => x.ReceiverAccountId == receiverAccount || x.SenderAccountId == senderAccount);
        //////    }
        //////    else
        //////    {
        //////            query = query.Where(x => x.ReceiverAccountId == receiverAccount);
        //////        }

        //////        if (senderAccount != null)
        //////        {
        //////            query = query.Where(x => x.SenderAccountId == senderAccount);
        //////        }
        //////    }

        //////    if (contains != "*" && !string.IsNullOrEmpty(contains))
        //////    {
        //////        query = query.Where(x => (x.ReceiverAccount.AccountNumber + x.SenderAccount.AccountNumber).Contains(contains));
        //////    }

        //////    //=====sorting====
        //////    //Stakata: sortOder must match property name (case sens.)
        //////    query = string.IsNullOrEmpty(sortOrder) ? query.Sort("CreatedOn_desc") : query.Sort(sortOrder);

        //////    query = query.Skip(skip).Take(take);

        //////    return query;
        //////}

        public async Task<IReadOnlyCollection<TransactionDto>> GetTransactionsListAsync
           (int userId, int? accountId = null, int skip = 0, int take = int.MaxValue, string contains = "*", string sortOrder = "CreatedOn_desc")
        {
            return await this.GetAllTransactionsByUserQuery(userId, skip, take, accountId, contains, true, sortOrder).ToListAsync();
        }

        public async Task<int> GetTransactionsCountAsync
           (int userId, int? accountId = null, string contains = "*")
        {
            var query = this.GetAllTransactionsByUserQuery(userId: userId, accountId: accountId, contains: contains, onlySent: true);
            return await query.CountAsync();
        }


        private IQueryable<TransactionDto> GetUserTransactionsQuery
            (int userId, int? accountId = null, int skip = 0, int take = int.MaxValue, string contains = "*", string sortOrder = "CreatedOn_desc")
        {
            var query = this.context.PayCloudUsers
                .Where(x => x.UserId == userId)
                .Include(x => x.UserTransactions)
                .SelectMany(x => x.UserTransactions);

            if (accountId != null)
            {
                query = query.Where(x => x.SenderAccountId == accountId);
            }
            if (contains != "*" && !string.IsNullOrEmpty(contains))
            {
                query = query.Where(x => (x.ReceiverAccount.AccountNumber + x.SenderAccount.AccountNumber).Contains(contains));
            }
            //=====sorting====
            //Stakata: sortOder must match property name (case sens.)
            query = string.IsNullOrEmpty(sortOrder) ? query.Sort("CreatedOn_desc") : query.Sort(sortOrder);


            query = query.Skip(skip).Take(take);

            var nicknames = this.MyNicknames(userId);

            return query.Select(x =>
                new TransactionDto()
                {
                    Amount = x.Amount,
                    MainAccountNum = x.SenderAccount.AccountNumber,
                    SecondAccountNum = x.ReceiverAccount.AccountNumber,
                    Description = x.Description,
                    StatusCode = x.StatusCode,
                    CreatedOn = x.CreatedOn,
                    SentOn = x.SentOn,
                    TransactionId = x.TransactionId,
                    MainNickname = (nicknames.ContainsKey(x.SenderAccountId)) ? nicknames[x.SenderAccount.AccountId] : null,
                    SecondNickname = (nicknames.ContainsKey(x.ReceiverAccountId)) ? nicknames[x.ReceiverAccount.AccountId] : null,
                    MainClientName = x.ReceiverAccount.Client.Name,
                    SecondClientName = x.SenderAccount.Client.Name
                }
            );
        }

        public async Task<ICollection<TransactionDto>> GetUserTransactionsAsync
        (int userId, int? accountId = null, int skip = 0, int take = int.MaxValue, string contains = "*", string sortOrder = "CreatedOn_desc")
        {
            return await this.GetUserTransactionsQuery(userId, accountId, skip, take, contains, sortOrder).ToListAsync();
        }

        public Task<int> GetUserTransactionsCountAsync(int userId, int? accountId = null, string contains = "*")
        {
            return this.GetUserTransactionsQuery(userId: userId, accountId: accountId, contains: contains).CountAsync();
        }

        public async Task SavePaymentAsync(TransactionCUDto transactionDto)
        {
            if (!await this.userService.IsUserAuthorizedForAccount(transactionDto.SenderAccountId))
            {
                throw new AccountAccessSuspendedException(string.Format(Constants.AccountAccessSuspended, transactionDto.SenderAccountId));
            }

            if (transactionDto.SenderAccountId == transactionDto.ReceiverAccountId)
            {
                throw new ServiceErrorException(Constants.SameAccounts);
            }

            var senderAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transactionDto.SenderAccountId) ??
                throw new ServiceErrorException(string.Format(Constants.AccountWithIdNotExist, transactionDto.SenderAccountId));

            var receiverAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transactionDto.ReceiverAccountId) ??
                throw new ServiceErrorException(string.Format(Constants.AccountWithIdNotExist, transactionDto.ReceiverAccountId));

            var newTransaction =
                 new Transaction()
                 {
                     SenderAccountId = transactionDto.SenderAccountId,
                     ReceiverAccountId = transactionDto.ReceiverAccountId,
                     Description = transactionDto.Description,
                     Amount = transactionDto.Amount,
                     CreatedByUserId = transactionDto.CreatedByUserId,
                     CreatedOn = this.dateTimeNow.Now,
                     StatusCode = (int)StatusCode.Saved
                 };
            this.context.Transactions.Add(newTransaction);

            await this.context.SaveChangesAsync();
        }

        public async Task CancelPaymentAsync(int transactionId)
        {
            var transaction = await this.context.Transactions.FindAsync(transactionId) ??
                    throw new ServiceErrorException(string.Format(Constants.TransactionNotExist));

            if (!await this.userService.IsUserAuthorizedForAccount(transaction.SenderAccountId))
            {
                throw new AccountAccessSuspendedException(string.Format(Constants.AccountAccessSuspended, transaction.SenderAccountId));
            }

            transaction.StatusCode = (int)StatusCode.Canceled;
            await this.context.SaveChangesAsync();
        }

        public async Task SendPayment(int transactionId)
        {
            using (var scope = await this.context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {

                var transaction =
                await this.context.Transactions.SingleOrDefaultAsync(x => x.TransactionId == transactionId && x.StatusCode == (int)StatusCode.Saved)
                ?? throw new ServiceErrorException(string.Format(Constants.AccountNotExist));

                if (!await this.userService.IsUserAuthorizedForAccount(transaction.SenderAccountId))
                {
                    throw new AccountAccessSuspendedException(string.Format(Constants.AccountAccessSuspended, transaction.SenderAccountId));
                }

                if (transaction.SenderAccountId == transaction.ReceiverAccountId)
                {
                    throw new ServiceErrorException(Constants.SameAccounts);
                }

                var senderAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transaction.SenderAccountId) ??
                    throw new ServiceErrorException(string.Format(Constants.AccountWithIdNotExist, transaction.SenderAccountId));

                var receiverAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transaction.ReceiverAccountId) ??
                    throw new ServiceErrorException(string.Format(Constants.AccountWithIdNotExist, transaction.ReceiverAccountId));


                if (senderAccount.Balance < transaction.Amount)
                {
                    throw new ServiceErrorException(string.Format(Constants.InsufficientBalance, senderAccount.AccountNumber));
                }

                transaction.StatusCode = (int)StatusCode.Sent;
                transaction.SentOn = this.dateTimeNow.Now;

                senderAccount.Balance -= transaction.Amount;
                receiverAccount.Balance += transaction.Amount;
                try
                {
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw new ServiceErrorException(string.Format(Constants.DbError));
                }
                scope.Commit();
            }
        }


        public async Task MakePaymentAsync(TransactionCUDto transactionDto)
        {
            using (var scope = await this.context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {

                if (!await this.userService.IsUserAuthorizedForAccount(transactionDto.SenderAccountId))
                {
                    throw new AccountAccessSuspendedException(string.Format(Constants.AccountAccessSuspended, transactionDto.SenderAccountId));
                }

                if (transactionDto.SenderAccountId == transactionDto.ReceiverAccountId)
                {
                    throw new ServiceErrorException(Constants.SameAccounts);
                }

                var senderAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transactionDto.SenderAccountId) ??
                    throw new ServiceErrorException(string.Format(Constants.AccountNotExist, transactionDto.SenderAccountId));

                var receiverAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transactionDto.ReceiverAccountId) ??
                    throw new ServiceErrorException(string.Format(Constants.AccountNotExist, transactionDto.ReceiverAccountId));

                if (senderAccount.Balance < transactionDto.Amount)
                {
                    throw new ServiceErrorException(string.Format(Constants.InsufficientBalance, senderAccount.AccountNumber));
                }

                var newTransaction =
                             new Transaction()
                             {
                                 SenderAccountId = transactionDto.SenderAccountId,
                                 ReceiverAccountId = transactionDto.ReceiverAccountId,
                                 Description = transactionDto.Description,
                                 Amount = transactionDto.Amount,
                                 CreatedByUserId = transactionDto.CreatedByUserId,
                                 CreatedOn = this.dateTimeNow.Now,
                                 SentOn = this.dateTimeNow.Now,
                                 StatusCode = (int)StatusCode.Sent
                             };
                this.context.Transactions.Add(newTransaction);

                senderAccount.Balance -= transactionDto.Amount;
                receiverAccount.Balance += transactionDto.Amount;
                try
                {
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new ServiceErrorException(string.Format(Constants.DbError));
                }
                scope.Commit();
            }
        }

    }
}






//Rowversin (Timestamp) concurrency resolve:

////////public async Task SendPayment(int transactionId, int attempts = 5)
////////{
////////    var nextTry = true;
////////    var count = 0;

////////    while (nextTry)
////////    {
////////        var transaction =
////////            await this.context.Transactions.SingleOrDefaultAsync(x => x.TransactionId == transactionId && x.StatusCode == (int)StatusCode.Saved)
////////            ?? throw new ServiceErrorException(string.Format(Constants.AccountNotExist));

////////        if (!await this.userService.IsUserAuthorizedForAccount(transaction.SenderAccountId))
////////        {
////////            throw new AccountAccessSuspendedException(string.Format(Constants.AccountAccessSuspended, transaction.SenderAccountId));
////////        }

////////        if (transaction.SenderAccountId == transaction.ReceiverAccountId)
////////        {
////////            throw new ServiceErrorException(Constants.SameAccounts);
////////        }

////////        var senderAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transaction.SenderAccountId) ??
////////            throw new ServiceErrorException(string.Format(Constants.AccountNotExist, transaction.SenderAccountId));

////////        var receiverAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transaction.ReceiverAccountId) ??
////////            throw new ServiceErrorException(string.Format(Constants.AccountNotExist, transaction.ReceiverAccountId));


////////        var rowVersion = senderAccount.ConcurrencyRowVersion;

////////        if (senderAccount.Balance >= transaction.Amount)
////////        {
////////            transaction.StatusCode = (int)StatusCode.Sent;
////////            transaction.SentOn = this.dateTimeNow.Now;
////////            this.context.Entry(senderAccount).Property("ConcurrencyRowVersion").OriginalValue = rowVersion;

////////            senderAccount.Balance -= transaction.Amount;
////////            receiverAccount.Balance += transaction.Amount;
////////            try
////////            {
////////                await this.context.SaveChangesAsync();
////////                nextTry = false;
////////            }
////////            catch (DbUpdateConcurrencyException ex)
////////            {
////////                var exceptionEntry = ex.Entries.Single();
////////                var databaseEntry = exceptionEntry.GetDatabaseValues();
////////                var databaseValues = (Account)databaseEntry.ToObject();
////////                if (databaseValues.Balance < transaction.Amount)
////////                {
////////                    throw new ServiceErrorException(string.Format(Constants.InsufficientBalance, senderAccount.AccountNumber));
////////                }
////////                else
////////                {
////////                    nextTry = (++count <= attempts) ? true : false;
////////                }
////////            }
////////        }
////////        else
////////        {
////////            throw new ServiceErrorException(string.Format(Constants.InsufficientBalance, senderAccount.AccountNumber));
////////        }
////////    }

////////}

////////public async Task MakePaymentAsync(TransactionCUDto transactionDto, int attempts = 5)
////////{
////////    var nextTry = true;
////////    var count = 0;

////////    while (nextTry)
////////    {

////////        if (!await this.userService.IsUserAuthorizedForAccount(transactionDto.SenderAccountId))
////////        {
////////            throw new AccountAccessSuspendedException(string.Format(Constants.AccountAccessSuspended, transactionDto.SenderAccountId));
////////        }

////////        if (transactionDto.SenderAccountId == transactionDto.ReceiverAccountId)
////////        {
////////            throw new ServiceErrorException(Constants.SameAccounts);
////////        }

////////        var senderAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transactionDto.SenderAccountId) ??
////////            throw new ServiceErrorException(string.Format(Constants.AccountNotExist, transactionDto.SenderAccountId));

////////        var receiverAccount = this.context.Accounts.SingleOrDefault(x => x.AccountId == transactionDto.ReceiverAccountId) ??
////////            throw new ServiceErrorException(string.Format(Constants.AccountNotExist, transactionDto.ReceiverAccountId));


////////        var rowVersion = senderAccount.ConcurrencyRowVersion;

////////        if (senderAccount.Balance >= transactionDto.Amount)
////////        {
////////            var newTransaction =
////////                 new Transaction()
////////                 {
////////                     SenderAccountId = transactionDto.SenderAccountId,
////////                     ReceiverAccountId = transactionDto.ReceiverAccountId,
////////                     Description = transactionDto.Description,
////////                     Amount = transactionDto.Amount,
////////                     CreatedByUserId = transactionDto.CreatedByUserId,
////////                     CreatedOn = this.dateTimeNow.Now,
////////                     SentOn = this.dateTimeNow.Now,
////////                     StatusCode = (int)StatusCode.Sent
////////                 };
////////            this.context.Transactions.Add(newTransaction);

////////            this.context.Entry(senderAccount).Property("ConcurrencyRowVersion").OriginalValue = rowVersion;

////////            senderAccount.Balance -= transactionDto.Amount;
////////            receiverAccount.Balance += transactionDto.Amount;
////////            try
////////            {
////////                await this.context.SaveChangesAsync();
////////                nextTry = false;
////////            }
////////            catch (DbUpdateConcurrencyException ex)
////////            {
////////                var exceptionEntry = ex.Entries.Single();
////////                var databaseEntry = exceptionEntry.GetDatabaseValues();
////////                var databaseValues = (Account)databaseEntry.ToObject();
////////                if (databaseValues.Balance < transactionDto.Amount)
////////                {
////////                    throw new ServiceErrorException(string.Format(Constants.InsufficientBalance, senderAccount.AccountNumber));
////////                }
////////                else
////////                {
////////                    nextTry = (++count <= attempts);
////////                }
////////            }
////////        }
////////        else
////////        {
////////            throw new ServiceErrorException(string.Format(Constants.InsufficientBalance, senderAccount.AccountNumber));
////////        }
////////    }

////////}