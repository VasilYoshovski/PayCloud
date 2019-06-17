using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Utils
{
    public class ServiceErrorException:ArgumentException
    {
        public ServiceErrorException(string message):base(message)
        {

        }
    }
}
