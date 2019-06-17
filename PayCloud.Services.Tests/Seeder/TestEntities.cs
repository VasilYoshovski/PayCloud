using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PayCloud.Services.Tests
{
    internal static class TestEntities
    {
        public static readonly string exampleToken = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        public static readonly string exampleToken2 = @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InN0YWthdGEiLCJ1c2VyUm9sZSI6IkFkbWluIiwibmJmIjoxNTU4MzM2OTcxLCJleHAiOjE1NTg0NDQ5NzAsImlhdCI6MTU1ODMzNjk3MX0.LUz95iTGgKvKqwVaWmRJSG_gnuZ0yS7DPVTkj9FC4Sw";

        public static readonly string exampleSecret = "d8bec9dfa11de0c600a88b0d5dec0acce9deb0cc812f0865671ca2c2cfa40da8";

        //public static readonly string TestUserPassword = "500E0B7BFD3540CED17D8EFBAB9AADAD2C86D3ADC39E848A8A52EA914A335811";

        public static readonly IList<Claim> eaxmpleClaimCollection = new List<Claim>
        {
            new Claim("userRole", "User")
        };

        public static readonly string exampleReceiverAccountNumber = "1234567890";

        public static readonly ClaimsIdentity exampleClaimsIdentity = new ClaimsIdentity(eaxmpleClaimCollection);

        public static readonly ClaimsPrincipal exampleClaimPrincipal = new ClaimsPrincipal(exampleClaimsIdentity);

        public static Client TestClient1 => new Client()
        {
            ClientId = 1,
            Name = "TestClient1"
        };

        public static Client TestClient2 => new Client()
        {
            ClientId = 2,
            Name = "TestClient2"
        };

        public static Client TestClient3 => new Client()
        {
            ClientId = 3,
            Name = "TestClient3"
        };

        public static Client TestClient4 => new Client()
        {
            ClientId = 4,
            Name = "TestClient31"
        };

        public static Client TestClient5 => new Client()
        {
            ClientId = 5,
            Name = "TestClient32"
        };

        public static Client TestClient6 => new Client()
        {
            ClientId = 6,
            Name = "TestClient33"
        };
        //-----------------saved-----------------------------
        public static Transaction TestSavedTransaction1 => new Transaction()
        {
            TransactionId = 9,
            Amount = 10,
            SenderAccountId = 1,
            ReceiverAccountId = 2,
            StatusCode = (int)StatusCode.Saved,
            Description = "Test saved transaction",
            CreatedOn = new DateTime(2019, 6, 10),
            CreatedByUserId = 1
        };

        public static Transaction TestSavedTransaction2 => new Transaction()
        {
            TransactionId = 10,
            Amount = 20,
            SenderAccountId = 1,
            ReceiverAccountId = 3,
            StatusCode = (int)StatusCode.Saved,
            Description = "Test saved transaction",
            CreatedOn = new DateTime(2019, 6, 10),
            CreatedByUserId = 2
        };

        public static Transaction TestSavedTransaction3 => new Transaction()
        {
            TransactionId = 11,
            Amount = 30,
            SenderAccountId = 3,
            ReceiverAccountId = 2,
            StatusCode = (int)StatusCode.Saved,
            Description = "Test saved transaction",
            CreatedOn = new DateTime(2019, 6, 10),
            CreatedByUserId = 1
        };
        //-------------------sent---------------------------------------
        public static Transaction TestSentTransaction1 => new Transaction()
        {
            TransactionId = 1,
            Amount = 10,
            SenderAccountId = 1,
            ReceiverAccountId = 2,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            SentOn = new DateTime(2019, 6, 7),
            CreatedByUserId = 1
        };

        public static Transaction TestSentTransaction2 => new Transaction()
        {
            TransactionId = 2,
            Amount = 20,
            SenderAccountId = 3,
            ReceiverAccountId = 1,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            CreatedOn = new DateTime(2019, 6, 7),
            SentOn = new DateTime(2019, 6, 7),
            CreatedByUserId = 1
        };

        public static Transaction TestSentTransaction3 => new Transaction()
        {
            TransactionId = 3,
            Amount = 30,
            SenderAccountId = 2,
            ReceiverAccountId = 1,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            CreatedOn = new DateTime(2019, 6, 7),
            SentOn = new DateTime(2019, 6, 7),
            CreatedByUserId = 1
        };


        public static Transaction TestSentTransaction4 => new Transaction()
        {
            TransactionId = 4,
            Amount = 40,
            SenderAccountId = 1,
            ReceiverAccountId = 3,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            CreatedOn = new DateTime(2019, 6, 8),
            SentOn = new DateTime(2019, 6, 8),
            CreatedByUserId = 1
        };

        public static Transaction TestSentTransaction5 => new Transaction()
        {
            TransactionId = 5,
            Amount = 50,
            SenderAccountId = 1,
            ReceiverAccountId = 2,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            CreatedOn = new DateTime(2019, 6, 8),
            SentOn = new DateTime(2019, 6, 8),
            CreatedByUserId = 1
        };


        public static Transaction TestSentTransaction6 => new Transaction()
        {
            TransactionId = 6,
            Amount = 60,
            SenderAccountId = 2,
            ReceiverAccountId = 3,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            CreatedOn = new DateTime(2019, 6, 9),
            SentOn = new DateTime(2019, 6, 9),
            CreatedByUserId = 1
        };

        public static Transaction TestSentTransaction7 => new Transaction()
        {
            TransactionId = 7,
            Amount = 70,
            SenderAccountId = 1,
            ReceiverAccountId = 3,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            CreatedOn = new DateTime(2019, 6, 10),
            SentOn = new DateTime(2019, 6, 10),
            CreatedByUserId = 1
        };


        public static Transaction TestSentTransaction8 => new Transaction()
        {
            TransactionId = 8,
            Amount = 80,
            SenderAccountId = 3,
            ReceiverAccountId = 1,
            StatusCode = (int)StatusCode.Sent,
            Description = "Test sent transaction",
            CreatedOn = new DateTime(2019, 6, 10),
            SentOn = new DateTime(2019, 6, 10),
            CreatedByUserId = 1
        };


        public static Account TestAccount1 => new Account()
        {
            AccountId = 1,
            AccountNumber = "0000000001",
            Balance = 100,
            ClientId = 3,
        };

        public static Account TestAccount2 => new Account()
        {
            AccountId = 2,
            AccountNumber = "0000000002",
            Balance = 200,
            ClientId = 1,
        };

        public static Account TestAccount3 => new Account()
        {
            AccountId = 3,
            AccountNumber = "0000000003",
            Balance = 300,
            ClientId = 1,
        };

        public static Account TestAccount4 => new Account()
        {
            AccountId = 4,
            AccountNumber = "1000000004",
            Balance = 400,
            ClientId = 2,
        };


        public static Account TestAccount5 => new Account()
        {
            AccountId = 5,
            AccountNumber = "1000000005",
            Balance = 400,
            ClientId = 2,
        };

        public static Account TestAccount6 => new Account()
        {
            AccountId = 6,
            AccountNumber = "1000000006",
            Balance = 600,
            ClientId = 1,
        };

        public static PayCloudUser TestUser1 => new PayCloudUser()
        {
            UserId = 1,
            Name = "Test User1",
            Username = "TestUserName1",
            Password = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F",
            Role = "User",
            UserAccounts = new List<PayCloudUserAccount>(),
            UserClients = new List<PayCloudUserClient>(),
            UserTransactions = new List<Transaction>(),
        };

        public static PayCloudUser TestUser2 => new PayCloudUser()
        {
            UserId = 2,
            Name = "Test User2",
            Username = "TestUserName2",
            Password = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F",
            Role = "User",
            UserAccounts = new List<PayCloudUserAccount>(),
            UserClients = new List<PayCloudUserClient>(),
            UserTransactions = new List<Transaction>(),
        };

        public static PayCloudUser TestUser3 => new PayCloudUser()
        {
            UserId = 3,
            Name = "Test User3",
            Username = "TestUserName3",
            Password = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F",
            Role = "User",
            UserAccounts = new List<PayCloudUserAccount>(),
            UserClients = new List<PayCloudUserClient>(),
            UserTransactions = new List<Transaction>(),
        };

        public static PayCloudAdmin TestAdmin => new PayCloudAdmin()
        {
            AdminId = 1,
            Name = "Test Admin",
            Username = "testadmin",
            Password = "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F",
            Role = "Admin"
        };

        public static PayCloudUserAccount TestUserAccounts11 => new PayCloudUserAccount()
        {
            PayCloudUserId = 1,
            AccountId = 1,
            AccountNickname = "TestUserAccounts11"
        };

        public static PayCloudUserAccount TestUserAccounts12 => new PayCloudUserAccount()
        {
            PayCloudUserId = 1,
            AccountId = 2,
            AccountNickname = "TestUserAccounts12"
        };

        public static PayCloudUserAccount TestUserAccounts13 => new PayCloudUserAccount()
        {
            PayCloudUserId = 1,
            AccountId = 3,
            AccountNickname = "TestUserAccounts13"
        };

        public static PayCloudUserAccount TestUserAccounts21 => new PayCloudUserAccount()
        {
            PayCloudUserId = 2,
            AccountId = 1,
            AccountNickname = "TestUserAccounts21"
        };

        //-----------------PayCloudUserTests-----------------------------
        public static PayCloudUserClient TestUserClient1 => new PayCloudUserClient()
        {
            PayCloudUserId = 1,
            ClientId = 1
        };

        public static PayCloudUserClient TestUserClient2 => new PayCloudUserClient()
        {
            PayCloudUserId = 2,
            ClientId = 1
        };

        public static PayCloudUserClient TestUserClient3 => new PayCloudUserClient()
        {
            PayCloudUserId = 3,
            ClientId = 1
        };


    }
}
