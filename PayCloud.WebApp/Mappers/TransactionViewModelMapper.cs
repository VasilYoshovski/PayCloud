using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.User.Models.TransactionViewModels;

namespace PayCloud.WebApp.Mappers
{
    public class TransactionViewModelMapper : IViewModelMapper<TransactionDto, TransactionViewModel>
    {
        public TransactionViewModel MapFrom(TransactionDto dto)
        =>
            new TransactionViewModel()
            {
                TransactionId=dto.TransactionId,
                Amount = dto.Amount,
                CreatedOn = dto.CreatedOn,
                SentOn = dto.SentOn,
                MainAccountNum = dto.MainAccountNum,
                SecondAccountNum = dto.SecondAccountNum,
                StatusCode = dto.StatusCode,
                Description = dto.Description,
                MainNickname = dto.MainNickname,
                SecondNickname = dto.SecondNickname,
                MainClientName = dto.MainClientName,
                SecondClientName = dto.SecondClientName
            };
    }
}
