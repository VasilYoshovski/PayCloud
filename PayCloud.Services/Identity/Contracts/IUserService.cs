using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Identity.Contracts
{
    public interface IUserService
    {
        Task<PayCloudUser> GetUserAsync(string username, string password);
        Task<bool> IsUserAuthorizedForAccount(int accountId);

        Task<PayCloudUser> GetLoggedUserAsync();
    }
}
