using PayCloud.Services.Dto;
using PayCloud.WebApp.Areas.User.Models.TransactionViewModels;

namespace PayCloud.WebApp.Mappers
{
    public class TransactionCUDtoMapper : IViewModelMapper<TransactionCUViewModel,TransactionCUDto >
    {
        public TransactionCUDto MapFrom(TransactionCUViewModel entity)
        =>
            new TransactionCUDto()
            {
                Amount = entity.Amount,
                SenderAccountId = entity.SenderAccountId,
                ReceiverAccountId = entity.ReceiverAccountId,
                Description = entity.Description,
                CreatedByUserId = entity.CreatedByUserId
            };
    }
}