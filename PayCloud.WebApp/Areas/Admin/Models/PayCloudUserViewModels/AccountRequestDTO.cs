using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.WebApp.Areas.Admin.Models.PayCloudUserViewModels
{
    public class AccountRequestDTO
    {
        public int AccountId { set; get; }
        public string AccountNumber { set; get; }
    }
}
