using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.User.Models.AccountViewModels;

namespace PayCloud.WebApp.Mappers
{
    public class UserAccountViewModelMapper : IViewModelMapper<AccountDto, UserAccountViewModel>
    {
        public UserAccountViewModel MapFrom(AccountDto entity)
        =>
            new UserAccountViewModel()
            {
                AccountId = entity.AccountId,
                AccountNumber = entity.AccountNumber,
                NickName = entity.NickName,
                Balance = entity.Balance,
                ClientName = entity.ClientName,
            };
    }

}