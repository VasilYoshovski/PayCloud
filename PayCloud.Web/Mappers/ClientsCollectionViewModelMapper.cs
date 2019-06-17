using PayCloud.Data.Models;
using PayCloud.Web.Models.ClientViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Mappers
{
    public class ClientsCollectionViewModelMapper : IViewModelMapper<List<Client>, ClientsCollectionViewModel>
    {
        private readonly IViewModelMapper<Client, ClientViewModel> clientMapper;

        public ClientsCollectionViewModelMapper(IViewModelMapper<Client, ClientViewModel> clientMapper)
        {
            this.clientMapper = clientMapper ?? throw new ArgumentNullException(nameof(clientMapper));
        }

        public ClientsCollectionViewModel MapFrom(List<Client> entity)
             => new ClientsCollectionViewModel
             {
                 Clients = entity.Select(d => clientMapper.MapFrom(d)).ToList()
             };
    }
}
