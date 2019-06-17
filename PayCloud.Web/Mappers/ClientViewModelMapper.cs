using PayCloud.Data.Models;
using PayCloud.Web.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Mappers
{
    public class ClientViewModelMapper : IViewModelMapper<Client, ClientViewModel>
    {
        public ClientViewModel MapFrom(Client entity)
             => new ClientViewModel
             {
                 Name = entity.Name,
                 Accounts = entity.Accounts,
                 PayCloudUsers = entity.PayCloudUsers
             };
    }
}
