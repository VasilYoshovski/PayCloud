using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Models.ClientViewModels
{
    public class ClientsCollectionViewModel
    {
        public IReadOnlyCollection<ClientViewModel> Clients { get; set; }
    }
}
