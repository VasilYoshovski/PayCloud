//using PayCloud.Data.Models;
//using PayCloud.Services.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace PayCloud.Services.Mappers
//{
//    public class TransactionDtoMapper : IDtoMapper<Transaction, TransactionDto>
//    {
//        public TransactionDto MapFrom(Transaction entity)
//             => new TransactionDto
//             {
//                 TransactionId = entity.TransactionId,
//                 Amount = entity.Amount,
//                 Description = entity.Description,
//                 MainAccountNum = entity.ReceiverAccount.AccountNumber,
//                 SecondAccountNum = entity.SenderAccount.AccountNumber,
//                 CreatedOn = entity.CreatedOn,
//                 SentOn = entity.SentOn,
//                 StatusCode = entity.StatusCode
//             };
//    }
//}

