using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayCloud.Services.Mappers
{
    public class AccountDtoMapper : IDtoMapper<Account, AccountDto>
    {
        public AccountDto MapFrom(Account entity)
             => new AccountDto
             {
                 AccountId = entity.AccountId,
                 AccountNumber = entity.AccountNumber,
                 Balance = entity.Balance,
                 ClientName = entity.Client.Name
                 
             };
    }
}
