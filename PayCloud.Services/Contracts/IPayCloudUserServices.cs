using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayCloud.Services.Contracts
{
    public interface IPayCloudUserServices
    {
        Task<PayCloudUser> FindPayCloudUserByUserNameAsync(string searchString);
        //Task<PayCloudUser> GetPayCloudUserByUserNameAsync(string searchString);
        //Task<PayCloudUser> FindPayCloudUserByNameAsync(string searchString);
        //Task<PayCloudUser> GetPayCloudUserByNameAsync(string searchString);
        Task<PayCloudUser> FindPayCloudUserByIDAsync(int id);
        //Task<PayCloudUser> GetPayCloudUserByIDAsync(int id);
        Task<PayCloudUser> CreatePayCloudUserAsync(
            string name,
            string userName,
            string password,
            string role);
        //Task<PayCloudUser> UpdatePayCloudUserAsync(
        //    int id,
        //    string name,
        //    string userName,
        //    string password,
        //    string role);
        //Task<bool> DeletePayCloudUserAsync(int id);
        Task<List<PayCloudUser>> GetAllPayCloudUsersAsync();
        //Task<(List<PayCloudUser> filteredList, int allCount)> GetAllPayCloudUsersByFilterAsync(
        //    int from,
        //    int to,
        //    string contains);
        //Task<(List<PayCloudUser> filteredList, int allCount, int pageNumber, int elementsPerPage)> GetPagedAllPayCloudUsersByFilterAsync(
        //    int pageNumber,
        //    int elementsPerPage,
        //    string contains);
        Task<bool> AssignAcountToUserAsync(int clientId, int accountId, int userId);
        Task<bool> RemoveAcountFromUserAsync(int clientId, int accountId, int userId);
        Task<bool> AssignClientToUserAsync(int clientId, int userId);
        Task<bool> RemoveUserFromClientAsync(int clientId, int userId);
        Task<List<PayCloudUser>> PayCloudUsersAssignedToClientAsync(int? clientId);
        Task<List<PayCloudUser>> PayCloudUsersNotAssignedToClientAsync(int? clientId);
        Task<bool> DeleteAllAccountsOfUser(int clientId, int userId);
    }
}
