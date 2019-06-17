using System.Collections.Generic;
using System.Threading.Tasks;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;

namespace PayCloud.Services
{
    public interface IClientService
    {
        Task<ClientDto> CreateClientAsync(string clientName);
        Task<IReadOnlyCollection<ClientAccountDto>> GetClientAccounts(int clientId);
        Task<IReadOnlyCollection<ClientDto>> GetClientsListAsync(int skip = 0, int take = int.MaxValue, string term = null, string sortOrder = "ClientId");
        Task<int> GetClientsCountAsync(string contains = "*");

    }
}