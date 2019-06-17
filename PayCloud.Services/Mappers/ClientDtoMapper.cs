using PayCloud.Data.Models;
using PayCloud.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayCloud.Services.Mappers
{
    public class ClientDtoMapper : IDtoMapper<Client, ClientDto>
    {
        public ClientDto MapFrom(Client entity)
             => new ClientDto
             {
                 ClientId = entity.ClientId,
                 ClientName = entity.Name
             };
    }
}
