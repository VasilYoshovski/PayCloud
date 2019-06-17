using System.Threading.Tasks;
using PayCloud.Data.Models;

namespace PayCloud.Services.Identity.Contracts
{
    public interface IAdminServices
    {
        Task<PayCloudAdmin> GetAdminAsync(string username, string password);
        Task<PayCloudAdmin> GetLoggedAdminAsync();
    }
}