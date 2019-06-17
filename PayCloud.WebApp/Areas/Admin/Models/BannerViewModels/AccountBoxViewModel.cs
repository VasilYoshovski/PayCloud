using PayCloud.WebApp.Areas.Admin.Models.AccountViewModels;

namespace PayCloud.WebApp.Areas.Admin.Models.ClientViewModels
{
    public class AccountBoxViewModel
    {
        public AccountViewModel Account { get; set; }
        public decimal Received { get; set; }
        public decimal Sent { get; set; }
    }
}
