using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Utils
{
    class Constants
    {
        internal static string UserExists = "User with user name {0} already exists!";
        internal static string UserError = "Error! Can not get user from sesion!";
        internal static string UserNotFound = "User with user name {0} can not be found!";
        internal static string AdminNotFound = "Admin with user name {0} can not be found!";
        internal static string ClientNotFound = "Client can not be found!";
        internal static string AccountNotFound = "Can not find account!";
        internal static string WrongCredentials = "Username or password is wrong";
        internal static string AccountNotExist = "Account does not exists!";
        internal static string AccountWithIdNotExist = "Account does not exists!";
        internal static string TransactionNotExist = "Transaction does not exists!";
        internal static string ClientNameExist = "Client with name {0} already exists!";
        internal static string WrongArguments = "Wrong arguments passed to service!";
        internal static string InsufficientBalance = "Insufficient balance on account with Id \"{0}\"!";
        internal static string SameAccounts = "Sender and receiver accounts can not be same!";
        internal static string AccountAccessSuspended = "Access to account {0} was suspended!";
        internal static string DbError = "Databse concurrency exception!";
        internal static string NotAuthorized = "You are not authorized!";


    }
}
