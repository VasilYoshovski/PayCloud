using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Identity.Contracts
{
    public interface ICookieManager
    {
        void AddSessionCookieForToken(string token, string userName);

        void DeleteSessionCookies();
    }
}
