using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels
{
    public class PayCloudUsersCollectionViewModel
    {
        public IReadOnlyCollection<PayCloudUserViewModel> PayCloudUsers { get; set; }
    }
}
