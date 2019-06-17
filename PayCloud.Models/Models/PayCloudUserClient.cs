using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Data.Models
{
    public class PayCloudUserClient
    {
        public int PayCloudUserId { get; set; }
        public PayCloudUser PayCloudUser { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
