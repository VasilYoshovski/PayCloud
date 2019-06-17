using Microsoft.EntityFrameworkCore;
using PayCloud.Data.DbContext;
using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using PayCloud.Services.Mappers;
using PayCloud.Services.Utils;
using PayCloud.Services.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Services
{
    public class ClientService : IClientService
    {
        private readonly PayCloudDbContext context;
        private readonly IDtoMapper<Client, ClientDto> clientDtoMapper;
        private readonly IDtoMapper<Account, ClientAccountDto> clientAccountMapper;

        public ClientService(PayCloudDbContext context,
            IDtoMapper<Client, ClientDto> clientDtoMapper,
            IDtoMapper<Account, ClientAccountDto> clientAccountMapper
        )
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.clientDtoMapper = clientDtoMapper ?? throw new System.ArgumentNullException(nameof(clientDtoMapper));
            this.clientAccountMapper = clientAccountMapper ?? throw new System.ArgumentNullException(nameof(clientAccountMapper));
        }

        private IQueryable<Client> GetAllClientsQuery
            (int skip = 0, int take = int.MaxValue, string contains = "*", string sortOrder = "ClientId")
        {

            var query = this.context.Clients.AsQueryable();

            if (contains != "*" && !string.IsNullOrEmpty(contains))
            {
                query = query.Where(x => ($"{x.Name} {x.ClientId.ToString()}".ToLower()).Contains(contains.ToLower()));
            }

            //=====sorting====
            //Stakata: sortOder must match property name (case sens.)
            //Extension method sort
            query = string.IsNullOrEmpty(sortOrder) ? query.Sort("ClientId") : query.Sort(sortOrder);

            query = query.Skip(skip).Take(take);

            return query;
        }

        public async Task<IReadOnlyCollection<ClientDto>> GetClientsListAsync(int skip = 0, int take = int.MaxValue, string term = null, string sortOrder = "ClientId")
        {
            var clients = await this.GetAllClientsQuery(skip, take, term, sortOrder).ToListAsync();
            return clients.Select(clientDtoMapper.MapFrom).ToList();
        }

        public Task<int> GetClientsCountAsync(string contains = "*")
        {
            var query = this.GetAllClientsQuery(contains: contains);
            return query.CountAsync();
        }

        public async Task<ClientDto> CreateClientAsync(string clientName)
        {
            if (clientName == null)
            {
                throw new ServiceErrorException(Constants.WrongArguments);
            }

            var client = await this.context.Clients.FirstOrDefaultAsync(x => x.Name == clientName);
            if ( client != null)
            {
                throw new ServiceErrorException(string.Format(Constants.ClientNameExist, clientName));
            }

            var newClient = new Client()
            {
                Name = clientName
            };

            this.context.Clients.Add(newClient);
            await this.context.SaveChangesAsync();
            return this.clientDtoMapper.MapFrom(newClient);
        }

        public async Task<IReadOnlyCollection<ClientAccountDto>> GetClientAccounts(int clientId)
        {
            if (clientId <= 0)
            {
                throw new ServiceErrorException(Constants.WrongArguments);
            }

            var ddd = await this.context.Accounts.ToListAsync();
            var accounts = await this.context.Accounts.Where(x => x.ClientId == clientId).ToListAsync();
                
            return accounts.Select(clientAccountMapper.MapFrom).ToList();
        }

    }
}
