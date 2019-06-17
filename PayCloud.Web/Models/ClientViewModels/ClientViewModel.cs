using PayCloud.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Models.ClientViewModels
{
    public class ClientViewModel
    {
        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<PayCloudUser> PayCloudUsers { get; set; }
    }
}
