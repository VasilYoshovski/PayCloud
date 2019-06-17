using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using PayCloud.Data.DbContext;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PayCloud.Services.Tests
{
    internal static class Seeder
    {
        internal static DbContextOptions<PayCloudDbContext> GetOptions(string databaseName)
        {
            var provider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<PayCloudDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(provider)
                .Options;

            return options;
        }

        internal static bool SeedDatabase(DbContextOptions<PayCloudDbContext> options,
            bool seed3Clients = false, bool seed3Accounts = false, bool seed3Users = false,
            bool seed3UserAccounts = false, bool seed3MoreAccounts = false, bool seed3MoreClients = false, bool seed8Transactions = false,
            bool seed3SavedTrans = false, bool seed1Admin = false, bool seed3UserClients = false)
        {
            using (var seedContext = new PayCloudDbContext(options))
            {
                if (seed3Clients)
                    seedContext.Clients.AddRange(
                        TestEntities.TestClient1,
                        TestEntities.TestClient2,
                        TestEntities.TestClient3
                    );

                if (seed3MoreClients)
                    seedContext.Clients.AddRange(
                        TestEntities.TestClient4,
                        TestEntities.TestClient5,
                        TestEntities.TestClient6
                    );

                if (seed3Accounts)
                    seedContext.Accounts.AddRange(
                        TestEntities.TestAccount1,
                        TestEntities.TestAccount2,
                        TestEntities.TestAccount3
                    );

                if (seed3MoreAccounts)
                    seedContext.Accounts.AddRange(
                        TestEntities.TestAccount4,
                        TestEntities.TestAccount5,
                        TestEntities.TestAccount6
                    );


                if (seed3Users)
                    seedContext.PayCloudUsers.AddRange(
                        TestEntities.TestUser1,
                        TestEntities.TestUser2,
                        TestEntities.TestUser3
                    );
                if (seed1Admin)
                    seedContext.PayCloudAdmins.AddRange(
                        TestEntities.TestAdmin
                    );

                if (seed3UserAccounts)
                    seedContext.UsersAccounts.AddRange(
                        TestEntities.TestUserAccounts11,
                        TestEntities.TestUserAccounts12,
                        TestEntities.TestUserAccounts21,
                        TestEntities.TestUserAccounts13
                );

                if (seed8Transactions)
                    seedContext.Transactions.AddRange(
                        TestEntities.TestSentTransaction1,
                        TestEntities.TestSentTransaction2,
                        TestEntities.TestSentTransaction3,
                        TestEntities.TestSentTransaction4,
                        TestEntities.TestSentTransaction5,
                        TestEntities.TestSentTransaction6,
                        TestEntities.TestSentTransaction7,
                        TestEntities.TestSentTransaction8
                );

                if (seed3SavedTrans)
                    seedContext.Transactions.AddRange(
                        TestEntities.TestSavedTransaction1,
                        TestEntities.TestSavedTransaction2,
                        TestEntities.TestSavedTransaction3
                );

                if (seed3UserClients)
                {
                    seedContext.UsersClients.AddRange(
                        TestEntities.TestUserClient1,
                        TestEntities.TestUserClient2,
                        TestEntities.TestUserClient3
                        );
                }

                var entityTracked = seedContext.SaveChanges();
                return entityTracked > 0 ? true : false;
            }
        }
    }
}
