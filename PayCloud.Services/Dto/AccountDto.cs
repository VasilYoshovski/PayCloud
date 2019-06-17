using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayCloud.Services.Dto
{
    public class AccountDto
    {
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public string NickName { get; set; }

        public decimal Balance { get; set; }

        public string ClientName { get; set; }
    }
}
