using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayCloud.Services.Mappers
{
    public class ClientAccountDtoMapper : IDtoMapper<Account, ClientAccountDto>
    {
        public ClientAccountDto MapFrom(Account entity)
             => new ClientAccountDto
             {
                 AccountId = entity.AccountId,
                 AccountNumber = entity.AccountNumber,
                 Balance = entity.Balance
             };
    }
}
