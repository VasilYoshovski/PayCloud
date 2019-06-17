using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayCloud.Web.Areas.User.Models.UserAccountsViewModels
{
    public class UserAccountsViewModel
    {
        public string NickName { set; get; }
        public uint AccountNumber { set; get; }
        public decimal CurrentBalance { set; get; }
    }
}
