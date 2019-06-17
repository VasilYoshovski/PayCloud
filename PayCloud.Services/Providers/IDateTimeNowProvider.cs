using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Providers
{
    public interface IDateTimeNowProvider
    {
        DateTime Now { get; }
    }
}
