using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PayCloud.Services.Dto
{
    public class ClientAccountDto
    {
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }
    }
}
