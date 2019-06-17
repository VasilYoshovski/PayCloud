using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Utils
{
    public class Constants
    {
        internal static string AccountCreated = "New acount with account number {0} successfully crated!";
        internal static string ClientCreated = "New client with name {0} successfully crated!";
        internal static string TransactionSent = "Payment successfully sent!";
        internal static string TransactionSaved = "Payment successfully saved!";
        internal static string AccountClientIdNull = "Client Id can not be null or 0!";
        internal static string AccountBalanceNull = "Balance can not be 0 or less!";
        internal static string CommonError = "Ooops something goes wrong!";
        internal static string NicknameChanged = "Account nickname changed to {0}!";
        internal static string NotAuthorized = "You are not authorized!";
    }
}
