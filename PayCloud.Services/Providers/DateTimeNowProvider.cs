using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Providers
{
    public class DateTimeNowProvider : IDateTimeNowProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
