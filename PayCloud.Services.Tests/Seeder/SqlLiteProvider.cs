//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using Microsoft.Extensions.Logging;
//using PayCloud.Data.DbContext;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace PayCloud.Services.Tests.Seeder
//{
//    class SqlLiteProvider
//    {
//        public FakeLogger EfLogger { get; private set; }

//        public PayCloudDbContext GetContextWithData(bool useSqlite = false)
//        {
//            EfLogger = new FakeLogger();

//            var factoryMock = Substitute.For<ILoggerFactory>();
//            factoryMock.CreateLogger(Arg.Any<string>()).Returns(EfLogger);

//            DbContextOptions<PayCloudDbContext> options;

//            if (useSqlite)
//            {
//                // In-memory database only exists while the connection is open
//                var connection = new SqliteConnection("DataSource=:memory:");
//                connection.Open();

//                options = new DbContextOptionsBuilder<PayCloudDbContext>()
//                    .UseSqlite(connection)
//                    .UseLoggerFactory(factoryMock)
//                    .Options;
//            }
//            else
//            {
//                options = new DbContextOptionsBuilder<PayCloudDbContext>()
//                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
//                    // don't raise the error warning us that the in memory db doesn't support transactions
//                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
//                    .UseLoggerFactory(factoryMock)
//                    .Options;
//            }

//            var ctx = new PayCloudDbContext(options);

//            if (useSqlite)
//            {
//                ctx.Database.EnsureCreated();
//            }

//            // code to populate the context with test data

//            ctx.SaveChanges();

//            return ctx;
//        }
//    }
//}
