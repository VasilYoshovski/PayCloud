using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Utils
{
    public class AccountAccessSuspendedException : Exception
    {
        public AccountAccessSuspendedException(string message):base(message)
        {

        }
    }
}
