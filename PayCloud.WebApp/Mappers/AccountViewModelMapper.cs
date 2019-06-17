using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.Admin.Models.AccountViewModels;

namespace PayCloud.WebApp.Mappers
{
    public class AccountViewModelMapper : IViewModelMapper<AccountDto, AccountViewModel>
    {
        public AccountViewModel MapFrom(AccountDto entity)
        =>
            new AccountViewModel()
            {
                AccountId = entity.AccountId,
                AccountNumber = entity.AccountNumber,
                NickName = entity.NickName,
                Balance = entity.Balance,
                ClientName = entity.ClientName,
            };
    }

}