using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Identity.Contracts
{
    public interface IAuthorizationService
    {
        //bool IsAuthorized();
        int GetLoggedUserId();
        bool IsInRole(string role);
    }
}
