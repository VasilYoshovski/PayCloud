using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Utils
{
    public interface IRandomProvider
    {
        int Next { get; }
    }
}
